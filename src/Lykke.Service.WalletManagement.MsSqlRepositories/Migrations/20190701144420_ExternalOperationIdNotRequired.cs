using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class ExternalOperationIdNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "external_operation_id",
                schema: "wallet_management",
                table: "transfer_event_data",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "external_operation_id",
                schema: "wallet_management",
                table: "transfer_event_data",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
