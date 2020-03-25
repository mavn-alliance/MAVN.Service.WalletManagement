using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class AddReferralId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "referral_id",
                schema: "wallet_management",
                table: "bonus_event_data",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "referral_id",
                schema: "wallet_management",
                table: "bonus_event_data");
        }
    }
}
