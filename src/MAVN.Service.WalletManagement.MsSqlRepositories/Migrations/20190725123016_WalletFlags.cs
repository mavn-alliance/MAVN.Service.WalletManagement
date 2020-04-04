using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Migrations
{
    public partial class WalletFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "wallet_flags",
                schema: "wallet_management");
        }
    }
}
