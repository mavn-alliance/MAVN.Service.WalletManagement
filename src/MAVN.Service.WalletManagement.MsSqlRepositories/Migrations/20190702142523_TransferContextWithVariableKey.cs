using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class TransferContextWithVariableKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transaction_hash",
                schema: "wallet_management",
                table: "transfer_event_data",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "operation_id",
                schema: "wallet_management",
                table: "transfer_event_data",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operation_id",
                schema: "wallet_management",
                table: "transfer_event_data");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "wallet_management",
                table: "transfer_event_data",
                newName: "transaction_hash");
        }
    }
}
