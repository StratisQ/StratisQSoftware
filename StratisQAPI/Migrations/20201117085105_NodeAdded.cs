using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class NodeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetNodes",
                columns: table => new
                {
                    AssetNodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentAssetNodeId = table.Column<int>(nullable: false),
                    RootAssetNodeId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    LastEditedDate = table.Column<DateTime>(nullable: false),
                    LastEditedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetNodes", x => x.AssetNodeId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentOrganizationId = table.Column<int>(nullable: false),
                    RootOrganizationId = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    LastEditedDate = table.Column<DateTime>(nullable: false),
                    LastEditedBy = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetNodes");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
