using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Industries_TenantId",
                table: "Clients",
                column: "TenantId",
                principalTable: "Industries",
                principalColumn: "IndustryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Industries_TenantId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
