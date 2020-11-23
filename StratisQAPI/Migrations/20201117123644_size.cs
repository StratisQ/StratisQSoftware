using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class size : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssetNodes");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "AssetNodes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "AssetNodes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssetNodes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
