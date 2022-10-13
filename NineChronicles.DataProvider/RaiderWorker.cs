using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bencodex.Types;
using Libplanet;
using Nekoyume;
using Nekoyume.Extensions;
using Nekoyume.Model.State;
using Nekoyume.TableData;
using NineChronicles.DataProvider.Store.Models;
using NineChronicles.Headless.GraphTypes.States;

namespace NineChronicles.DataProvider
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NineChronicles.DataProvider.Store;
    using NineChronicles.Headless;

    public class RaiderWorker : BackgroundService
    {
        private readonly StateContext _stateContext;
        private readonly MySqlStore _mySqlStore;

        public RaiderWorker(StateContext stateContext, MySqlStore mySqlStore)
        {
            _stateContext = stateContext;
            _mySqlStore = mySqlStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                var blockIndex = _mySqlStore.GetTip();
                var worldBossListSheetAddress = Addresses.GetSheetAddress<WorldBossListSheet>();
                var runeSheetAddress = Addresses.GetSheetAddress<RuneSheet>();
                var rewardSheetAddress = Addresses.GetSheetAddress<WorldBossRankingRewardSheet>();
                var values = _stateContext.GetStates(new[]
                    {worldBossListSheetAddress, runeSheetAddress, rewardSheetAddress});
                if (values[0] is Text wbs && values[1] is Text rs && values[2] is Text wrs)
                {
                    var sheet = new WorldBossListSheet();
                    sheet.Set(wbs);
                    var runeSheet = new RuneSheet();
                    runeSheet.Set(rs);
                    var rankingRewardSheet = new WorldBossRankingRewardSheet();
                    rankingRewardSheet.Set(wrs);
                    int raidId = sheet.FindPreviousRaidIdByBlockIndex(blockIndex);
                    var bossRow = sheet.OrderedList.First(r => r.Id == raidId);
                    if (bossRow.EndedBlockIndex > blockIndex)
                    {
                        var raiderListAddress = Addresses.GetRaiderListAddress(raidId);
                        if (_stateContext.GetState(raiderListAddress) is List raiderList)
                        {
                            var avatarAddresses = raiderList.ToList(StateExtensions.ToAddress);
                            var updateList = new List<RaiderModel>();
                            var storeRaiders = _mySqlStore.GetRaiderList();
                            UpdateRaider(_stateContext, avatarAddresses, raidId, storeRaiders, updateList);
                        }
                    }
                }
            }
        }

        private void UpdateRaider(StateContext stateContext, List<Address> avatarAddresses, int raidId, List<RaiderModel> storeRaiders, List<RaiderModel> updateList)
        {
            var raiderAddresses = avatarAddresses
                .Select(avatarAddress => Addresses.GetRaiderAddress(avatarAddress, raidId)).ToList();
            var raiderList = new List<RaiderModel>();
            var result = stateContext.GetStates(raiderAddresses);
            foreach (var value in result)
            {
                if (value is List list)
                {
                    var raiderState = new RaiderState(list);
                    var model = new RaiderModel(
                        raidId,
                        raiderState.AvatarName,
                        raiderState.HighScore,
                        raiderState.TotalScore,
                        raiderState.Cp,
                        raiderState.IconId,
                        raiderState.Level,
                        raiderState.AvatarAddress.ToHex(),
                        raiderState.PurchaseCount);
                    raiderList.Add(model);
                }
            }

            _mySqlStore.StoreRaiderList(raiderList);
        }
    }
}
