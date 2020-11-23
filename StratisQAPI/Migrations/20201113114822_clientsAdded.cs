using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class clientsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndustryId = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    ClientName = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    TelephoneNumber = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    VatNumber = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    IndustryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndustryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.IndustryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Industries");
        }
    }
}
