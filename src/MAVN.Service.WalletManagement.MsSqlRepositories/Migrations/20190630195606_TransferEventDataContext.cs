using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class TransferEventDataContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transfer_event_data",
                schema: "wallet_management",
                columns: table => new
                {
                    transaction_hash = table.Column<string>(nullable: false),
                    external_operation_id = table.Column<string>(nullable: false),
                    amount = table.Column<long>(nullable: false),
                    asset_symbol = table.Column<string>(nullable: false),
                    sender_customer_id = table.Column<string>(nullable: false),
                    recipient_customer_id = table.Column<string>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_event_data", x => x.transaction_hash);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transfer_event_data",
                schema: "wallet_management");
        }
    }
}
