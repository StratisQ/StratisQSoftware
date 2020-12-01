using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class employeeBiographicDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBiographicResponses");

            migrationBuilder.CreateTable(
                name: "EmployeeBiographicDetails",
                columns: table => new
                {
                    EmployeeBiographicDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    BiographicId = table.Column<int>(nullable: false),
                    BiographicDetailId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    DateStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBiographicDetails", x => x.EmployeeBiographicDetailId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBiographics",
                columns: table => new
                {
                    EmployeeBiographicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    BiographicId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    DateStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBiographics", x => x.EmployeeBiographicId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBiographicDetails");

            migrationBuilder.DropTable(
                name: "EmployeeBiographics");

            migrationBuilder.CreateTable(
                name: "EmployeeBiographicResponses",
                columns: table => new
                {
                    EmployeeBiographicResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiographicDetailId = table.Column<int>(type: "int", nullable: false),
                    BiographicDetailName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiographicId = table.Column<int>(type: "int", nullable: false),
                    BiographicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBiographicResponses", x => x.EmployeeBiographicResponseId);
                });
        }
    }
}
