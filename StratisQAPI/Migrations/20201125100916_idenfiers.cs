using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StratisQAPI.Migrations
{
    public partial class idenfiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurveyIdentifier",
                table: "Surveys");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveIdentifier",
                table: "Surveys",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectIdentifier",
                table: "Projects",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientIdentifier",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AssetNodeIdentifier",
                table: "AssetNodes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurveIdentifier",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "ProjectIdentifier",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientIdentifier",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AssetNodeIdentifier",
                table: "AssetNodes");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyIdentifier",
                table: "Surveys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
