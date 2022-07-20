﻿namespace NineChronicles.DataProvider.Executable.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCraftRanking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CraftRankings",
                columns: table => new
                {
                    AvatarAddress = table.Column<string>(type: "varchar(100)", nullable: false),
                    AgentAddress = table.Column<string>(type: "varchar(100)", nullable: true),
                    BlockIndex = table.Column<long>(type: "INTEGER", nullable: false),
                    CraftCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Ranking = table.Column<int>(type: "INTEGER", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftRankings", x => x.AvatarAddress);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CraftRankings");
        }
    }
}
