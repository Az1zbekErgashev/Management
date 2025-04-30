using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Some_fileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "InquirySource",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProjectBudget",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 30, 10, 16, 20, 184, DateTimeKind.Utc).AddTicks(5958));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InquirySource",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectBudget",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 17, 13, 27, 649, DateTimeKind.Utc).AddTicks(50));
        }
    }
}
