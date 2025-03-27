using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_REQUEST_TAbLE_ADDED_CHAtID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 8, 2, 4, 305, DateTimeKind.Utc).AddTicks(373));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 5, 19, 41, 968, DateTimeKind.Utc).AddTicks(7207));
        }
    }
}
