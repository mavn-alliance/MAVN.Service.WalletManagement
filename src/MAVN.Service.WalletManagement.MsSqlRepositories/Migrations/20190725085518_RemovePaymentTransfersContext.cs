using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class RemovePaymentTransfersContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_transfers_data",
                schema: "wallet_management");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_transfers_data",
                schema: "wallet_management",
                columns: table => new
                {
                    transfer_id = table.Column<string>(nullable: false),
                    amount = table.Column<long>(nullable: false),
                    campaign_id = table.Column<string>(nullable: false),
                    customer_id = table.Column<string>(nullable: false),
                    invoice_id = table.Column<string>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_transfers_data", x => x.transfer_id);
                });
        }
    }
}
