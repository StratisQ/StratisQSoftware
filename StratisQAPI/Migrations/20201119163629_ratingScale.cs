using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class ratingScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingScale",
                table: "Surveys",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingScale",
                table: "Surveys");
        }
    }
}
