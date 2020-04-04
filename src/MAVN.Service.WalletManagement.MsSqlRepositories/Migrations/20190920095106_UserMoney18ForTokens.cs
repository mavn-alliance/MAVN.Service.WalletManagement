using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class UserMoney18ForTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "amount",
                schema: "wallet_management",
                table: "transfer_event_data",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "amount",
                schema: "wallet_management",
                table: "bonus_event_data",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "amount",
                schema: "wallet_management",
                table: "transfer_event_data",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<long>(
                name: "amount",
                schema: "wallet_management",
                table: "bonus_event_data",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
