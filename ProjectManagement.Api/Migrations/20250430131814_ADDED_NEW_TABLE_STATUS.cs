using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_NEW_TABLE_STATUS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessingStatus",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "ProcessingStatusId",
                table: "Requests",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProcessingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStatus", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 30, 13, 18, 13, 569, DateTimeKind.Utc).AddTicks(3205));

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProcessingStatusId",
                table: "Requests",
                column: "ProcessingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ProcessingStatus_ProcessingStatusId",
                table: "Requests",
                column: "ProcessingStatusId",
                principalTable: "ProcessingStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ProcessingStatus_ProcessingStatusId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "ProcessingStatus");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ProcessingStatusId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProcessingStatusId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "ProcessingStatus",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 30, 10, 16, 20, 184, DateTimeKind.Utc).AddTicks(5958));
        }
    }
}
