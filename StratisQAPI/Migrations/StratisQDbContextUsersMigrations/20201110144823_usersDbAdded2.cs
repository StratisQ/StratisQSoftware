using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations.StratisQDbContextUsersMigrations
{
    public partial class usersDbAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorAuthentication",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoFactorAuthentication",
                table: "AspNetUsers");
        }
    }
}
