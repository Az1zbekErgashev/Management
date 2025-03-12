using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_LOG_TABlE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_UserId1",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId1",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CompanyLocation",
                table: "Partners",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Partners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeesCount",
                table: "Partners",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Partners",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "Logs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeesCount",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CountryId", "CreatedAt", "Description", "EmployeesCount", "Site" },
                values: new object[] { 1, new DateTime(2025, 3, 12, 12, 16, 32, 751, DateTimeKind.Utc).AddTicks(1547), null, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CountryId", "CreatedAt", "Description", "EmployeesCount", "Site" },
                values: new object[] { 67, new DateTime(2025, 3, 12, 12, 16, 32, 751, DateTimeKind.Utc).AddTicks(1549), null, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CountryId", "CreatedAt", "Description", "EmployeesCount", "Site" },
                values: new object[] { 45, new DateTime(2025, 3, 12, 12, 16, 32, 751, DateTimeKind.Utc).AddTicks(1551), null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 12, 12, 16, 32, 751, DateTimeKind.Utc).AddTicks(1694));

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CountryId",
                table: "Partners",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Countrys_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Countrys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Countrys_CountryId",
                table: "Partners",
                column: "CountryId",
                principalTable: "Countrys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Countrys_CountryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Countrys_CountryId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CountryId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CountryId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "EmployeesCount",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EmployeesCount",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Partners",
                newName: "CompanyLocation");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TeamMembers",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Location" },
                values: new object[] { new DateTime(2025, 3, 12, 6, 27, 0, 344, DateTimeKind.Utc).AddTicks(7), "South Korea" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Location" },
                values: new object[] { new DateTime(2025, 3, 12, 6, 27, 0, 344, DateTimeKind.Utc).AddTicks(9), "Uzbekistan" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Location" },
                values: new object[] { new DateTime(2025, 3, 12, 6, 27, 0, 344, DateTimeKind.Utc).AddTicks(11), "South Korea" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 12, 6, 27, 0, 344, DateTimeKind.Utc).AddTicks(158));

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId1",
                table: "TeamMembers",
                column: "UserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_UserId1",
                table: "TeamMembers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
