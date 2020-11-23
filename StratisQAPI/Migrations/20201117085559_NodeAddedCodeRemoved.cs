using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class NodeAddedCodeRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "AssetNodes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AssetNodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AssetNodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "AssetNodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
