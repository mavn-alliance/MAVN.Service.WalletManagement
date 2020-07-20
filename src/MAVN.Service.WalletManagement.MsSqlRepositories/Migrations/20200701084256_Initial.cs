using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
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
                    location_id = table.Column<string>(nullable: true),
                    location_code = table.Column<string>(nullable: true),
                    campaign_id = table.Column<Guid>(nullable: false),
                    condition_id = table.Column<Guid>(nullable: false),
                    customer_id = table.Column<string>(nullable: false),
                    bonus_type = table.Column<string>(nullable: true),
                    amount = table.Column<string>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    referral_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonus_event_data", x => x.operation_id);
                });

            migrationBuilder.CreateTable(
                name: "transfer_event_data",
                schema: "wallet_management",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    operation_id = table.Column<string>(nullable: false),
                    external_operation_id = table.Column<string>(nullable: true),
                    amount = table.Column<string>(nullable: false),
                    asset_symbol = table.Column<string>(nullable: false),
                    sender_customer_id = table.Column<string>(nullable: false),
                    recipient_customer_id = table.Column<string>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_event_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_flags",
                schema: "wallet_management",
                columns: table => new
                {
                    customer_id = table.Column<string>(nullable: false),
                    is_blocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_flags", x => x.customer_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonus_event_data",
                schema: "wallet_management");

            migrationBuilder.DropTable(
                name: "transfer_event_data",
                schema: "wallet_management");

            migrationBuilder.DropTable(
                name: "wallet_flags",
                schema: "wallet_management");
        }
    }
}
