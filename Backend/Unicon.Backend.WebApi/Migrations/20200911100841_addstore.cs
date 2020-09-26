using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Unicon.Backend.WebApi.Migrations
{
    public partial class addstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDateTime",
                table: "DeviceDefinitions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CommandsStoreEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SequenceNumber = table.Column<int>(nullable: false),
                    RelatedCommandId = table.Column<int>(nullable: false),
                    CommandType = table.Column<string>(nullable: true),
                    CommandDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandsStoreEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateDeviceDefinitionCommands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefinitionString = table.Column<string>(nullable: true),
                    TagsOneLine = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateDeviceDefinitionCommands", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandsStoreEntries");

            migrationBuilder.DropTable(
                name: "UpdateDeviceDefinitionCommands");

            migrationBuilder.DropColumn(
                name: "LastUpdateDateTime",
                table: "DeviceDefinitions");
        }
    }
}
