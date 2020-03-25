using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wallet_management");

            migrationBuilder.CreateTable(
                name: "bonus_event_data",
                schema: "wallet_management",
                columns: table => new
                {
                    operation_id = table.Column<Guid>(nullable: false),
                    partner_id = table.Column<string>(nullable: true),
                    campaign_id = table.Column<Guid>(nullable: false),
                    customer_id = table.Column<string>(nullable: false),
                    bonus_type = table.Column<string>(nullable: true),
                    amount = table.Column<long>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonus_event_data", x => x.operation_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonus_event_data",
                schema: "wallet_management");
        }
    }
}
