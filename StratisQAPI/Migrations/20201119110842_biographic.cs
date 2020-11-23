using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class biographic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiographicDetails",
                columns: table => new
                {
                    BiographicDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiographicDetails", x => x.BiographicDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Biographics",
                columns: table => new
                {
                    BiographicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biographics", x => x.BiographicId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiographicDetails");

            migrationBuilder.DropTable(
                name: "Biographics");
        }
    }
}
