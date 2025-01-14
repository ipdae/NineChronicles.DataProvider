﻿namespace NineChronicles.DataProvider.GraphQL.Types
{
    using global::GraphQL.Types;
    using NineChronicles.DataProvider.Store.Models;

    public class BattleArenaRankingType : ObjectGraphType<BattleArenaRankingModel>
    {
        public BattleArenaRankingType()
        {
            Field(x => x.BlockIndex);
            Field(x => x.AgentAddress);
            Field(x => x.AvatarAddress);
            Field(x => x.Name);
            Field(x => x.TitleId, nullable: true);
            Field(x => x.ArmorId, nullable: true);
            Field(x => x.AvatarLevel, nullable: true);
            Field(x => x.Cp, nullable: true);
            Field(x => x.ChampionshipId);
            Field(x => x.Round);
            Field(x => x.ArenaType);
            Field(x => x.Score);
            Field(x => x.WinCount);
            Field(x => x.MedalCount);
            Field(x => x.LossCount);
            Field(x => x.Ticket);
            Field(x => x.PurchasedTicketCount);
            Field(x => x.TicketResetCount);
            Field(x => x.EntranceFee);
            Field(x => x.TicketPrice);
            Field(x => x.AdditionalTicketPrice);
            Field(x => x.RequiredMedalCount);
            Field(x => x.StartBlockIndex);
            Field(x => x.EndBlockIndex);
            Field(x => x.Ranking);
            Field(x => x.TimeStamp);

            Name = "BattleArenaRanking";
        }
    }
}
