using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class TenantsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Tenants_TenantId",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Tenants_TenantId",
                table: "Clients",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
