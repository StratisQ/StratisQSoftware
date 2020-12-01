using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class employeeBiographicResponseFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBiographics");

            migrationBuilder.CreateTable(
                name: "EmployeeBiographicResponses",
                columns: table => new
                {
                    EmployeeBiographicResponseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiographicId = table.Column<int>(nullable: false),
                    BiographicName = table.Column<string>(nullable: true),
                    BiographicDetailId = table.Column<int>(nullable: false),
                    BiographicDetailName = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBiographicResponses", x => x.EmployeeBiographicResponseId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBiographicResponses");

            migrationBuilder.CreateTable(
                name: "EmployeeBiographics",
                columns: table => new
                {
                    EmployeeBiographicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiographicDetailId = table.Column<int>(type: "int", nullable: false),
                    BiographicId = table.Column<int>(type: "int", nullable: false),
                    DateStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBiographics", x => x.EmployeeBiographicId);
                });
        }
    }
}
