using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_USER_TABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Companies_AssignedCompanyId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers");

            migrationBuilder.DeleteData(
                table: "TeamMembers",
                keyColumns: new[] { "TeamId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "TeamMemberId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeamMembers");

            migrationBuilder.AddColumn<int>(
                name: "IndividualRole",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompaniesId",
                table: "Teams",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 7, 17, 46, 508, DateTimeKind.Utc).AddTicks(465));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 7, 17, 46, 508, DateTimeKind.Utc).AddTicks(467));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 7, 17, 46, 508, DateTimeKind.Utc).AddTicks(468));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IndividualRole" },
                values: new object[] { new DateTime(2025, 3, 7, 7, 17, 46, 508, DateTimeKind.Utc).AddTicks(557), 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CompaniesId",
                table: "Teams",
                column: "CompaniesId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Companies_AssignedCompanyId",
                table: "Teams",
                column: "AssignedCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Companies_CompaniesId",
                table: "Teams",
                column: "CompaniesId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Companies_AssignedCompanyId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Companies_CompaniesId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CompaniesId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IndividualRole",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompaniesId",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "TeamMemberId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "TeamMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "TeamMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(7823));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(7851));

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "AssignedCompanyId", "CreatedAt", "IsDeleted", "Location", "UpdatedAt" },
                values: new object[] { 1, null, new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(7967), 0, "UZB", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "TeamMemberId" },
                values: new object[] { new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(7964), 1 });

            migrationBuilder.InsertData(
                table: "TeamMembers",
                columns: new[] { "TeamId", "UserId", "CreatedAt", "Id", "IsCurrent", "IsDeleted", "Role", "UpdatedAt" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 6, 13, 4, 42, 698, DateTimeKind.Utc).AddTicks(8015), 1, true, 0, 4, null });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Companies_AssignedCompanyId",
                table: "Teams",
                column: "AssignedCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
