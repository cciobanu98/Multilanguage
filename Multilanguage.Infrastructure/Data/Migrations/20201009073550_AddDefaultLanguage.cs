using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Multilanguage.Infrastructure.Data.Migrations
{
    public partial class AddDefaultLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("a02f7209-8e72-4e45-8d2a-29d892e1f66e"), "en-GB", "British English" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: new Guid("a02f7209-8e72-4e45-8d2a-29d892e1f66e"));
        }
    }
}
