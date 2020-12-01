using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class employeeBiographicEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EmployeeBiographics");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "EmployeeBiographics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "EmployeeBiographics");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "EmployeeBiographics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
