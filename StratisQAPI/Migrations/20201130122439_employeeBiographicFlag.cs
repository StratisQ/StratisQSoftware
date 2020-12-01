using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class employeeBiographicFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BiographicName",
                table: "EmployeeBiographics",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBiographicDetail",
                table: "EmployeeBiographics",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiographicName",
                table: "EmployeeBiographics");

            migrationBuilder.DropColumn(
                name: "IsBiographicDetail",
                table: "EmployeeBiographics");
        }
    }
}
