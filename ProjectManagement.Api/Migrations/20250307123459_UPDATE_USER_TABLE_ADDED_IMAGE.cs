using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_USER_TABLE_ADDED_IMAGE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countrys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countrys", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 12, 34, 59, 131, DateTimeKind.Utc).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 12, 34, 59, 131, DateTimeKind.Utc).AddTicks(8275));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 12, 34, 59, 131, DateTimeKind.Utc).AddTicks(8276));

            migrationBuilder.InsertData(
                table: "Countrys",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Afghanistan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 2, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Åland Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 3, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Albania", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 4, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Algeria", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 5, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "American Samoa", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 6, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Andorra", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 7, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Angola", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 8, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Anguilla", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 9, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Antarctica", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 10, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Antigua and Barbuda", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 11, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Argentina", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 12, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Armenia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 13, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Aruba", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 14, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Australia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 15, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Austria", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 16, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Azerbaijan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 17, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bahamas", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 18, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bahrain", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 19, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bangladesh", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 20, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Barbados", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 21, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Belarus", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 22, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Belgium", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 23, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Belize", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 24, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Benin", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 25, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bermuda", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 26, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bhutan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 27, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bolivia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 28, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bonaire, Sint Eustatius and Saba", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 29, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bosnia and Herzegovina", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 30, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Botswana", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 31, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bouvet Island", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 32, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Brazil", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 33, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "British Indian Ocean Territory", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 34, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Brunei Darussalam", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 35, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Bulgaria", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 36, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Burkina Faso", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 37, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Burundi", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 38, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cambodia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 39, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cameroon", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 40, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Canada", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 41, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cape Verde", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 42, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cayman Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 43, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Central African Republic", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 44, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Chad", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 45, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Chile", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 46, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "China", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 47, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Christmas Island", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 48, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cocos (Keeling) Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 49, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Colombia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 50, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Comoros", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 51, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Congo, Republic of the (Brazzaville)", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 52, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Congo, the Democratic Republic of the (Kinshasa)", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 53, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cook Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 54, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Costa Rica", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 55, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Côte d'Ivoire, Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 56, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Croatia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 57, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cuba", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 58, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Curaçao", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 59, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Cyprus", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 60, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Czech Republic", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 61, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Denmark", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 62, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Djibouti", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 63, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Dominica", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 64, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Dominican Republic", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 65, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Ecuador", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 66, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Egypt", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 67, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "El Salvador", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 68, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Equatorial Guinea", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 69, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Eritrea", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 70, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Estonia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 71, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Ethiopia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 72, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Falkland Islands (Islas Malvinas)", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 73, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Faroe Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 74, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Fiji", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 75, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Finland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 76, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "France", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 77, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "French Guiana", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 78, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "French Polynesia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 79, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "French Southern and Antarctic Lands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 80, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Gabon", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 81, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Gambia, The", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 82, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Georgia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 83, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Germany", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 84, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Ghana", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 85, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Gibraltar", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 86, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Greece", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 87, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Greenland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 88, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Grenada", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 89, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guadeloupe", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 90, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guam", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 91, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guatemala", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 92, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guernsey", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 93, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guinea", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 94, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guinea-Bissau", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 95, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Guyana", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 96, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Haiti", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 97, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Heard Island and McDonald Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 98, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Holy See (Vatican City)", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 99, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Honduras", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 100, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Hong Kong", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 101, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Hungary", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 102, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Iceland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 103, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "India", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 104, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Indonesia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 105, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Iran, Islamic Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 106, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Iraq", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 107, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Ireland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 108, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Isle of Man", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 109, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Israel", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 110, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Italy", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 111, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Jamaica", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 112, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Japan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 113, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Jersey", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 114, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Jordan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 115, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kazakhstan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 116, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kenya", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 117, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kiribati", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 118, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Korea, Democratic People's Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 119, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Korea, Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 120, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kosovo", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 121, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kuwait", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 122, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Kyrgyzstan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 123, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Laos", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 124, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Latvia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 125, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Lebanon", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 126, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Lesotho", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 127, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Liberia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 128, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Libya", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 129, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Liechtenstein", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 130, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Lithuania", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 131, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Luxembourg", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 132, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Macao", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 133, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Macedonia, Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 134, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Madagascar", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 135, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Malawi", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 136, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Malaysia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 137, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Maldives", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 138, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mali", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 139, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Malta", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 140, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Marshall Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 141, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Martinique", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 142, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mauritania", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 143, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mauritius", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 144, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mayotte", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 145, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mexico", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 146, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Micronesia, Federated States of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 147, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Moldova", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 148, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Monaco", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 149, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mongolia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 150, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Montenegro", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 151, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Montserrat", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 152, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Morocco", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 153, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Mozambique", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 154, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Myanmar", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 155, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Namibia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 156, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Nauru", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 157, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Nepal", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 158, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Netherlands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 159, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "New Caledonia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 160, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "New Zealand", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 161, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Nicaragua", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 162, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Niger", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 163, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Nigeria", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 164, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Niue", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 165, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Norfolk Island", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 166, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Northern Mariana Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 167, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Norway", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 168, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Oman", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 169, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Pakistan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 170, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Palau", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 171, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Palestine, State of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 172, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Panama", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 173, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Papua New Guinea", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 174, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Paraguay", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 175, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Peru", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 176, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Philippines", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 177, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Pitcairn", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 178, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Poland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 179, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Portugal", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 180, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Puerto Rico", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 181, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Qatar", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 182, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Réunion", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 183, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Romania", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 184, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Russian Federation", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 185, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Rwanda", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 186, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Barthélemy", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 187, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Helena, Ascension and Tristan da Cunha", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 188, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Kitts and Nevis", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 189, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Lucia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 190, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Martin", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 191, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Pierre and Miquelon", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 192, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saint Vincent and the Grenadines", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 193, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Samoa", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 194, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "San Marino", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 195, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sao Tome and Principe", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 196, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Saudi Arabia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 197, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Senegal", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 198, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Serbia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 199, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Seychelles", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 200, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sierra Leone", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 201, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Singapore", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 202, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sint Maarten (Dutch part)", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 203, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Slovakia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 204, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Slovenia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 205, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Solomon Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 206, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Somalia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 207, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "South Africa", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 208, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "South Georgia and South Sandwich Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 209, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "South Sudan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 210, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Spain", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 211, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sri Lanka", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 212, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sudan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 213, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Suriname", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 214, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Eswatini", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 215, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Sweden", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 216, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Switzerland", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 217, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Syrian Arab Republic", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 218, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Taiwan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 219, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tajikistan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 220, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tanzania, United Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 221, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Thailand", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 222, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Timor-Leste", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 223, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Togo", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 224, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tokelau", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 225, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tonga", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 226, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Trinidad and Tobago", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 227, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tunisia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 228, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Turkey", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 229, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Turkmenistan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 230, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Turks and Caicos Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 231, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Tuvalu", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 232, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Uganda", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 233, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Ukraine", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 234, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "United Arab Emirates", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 235, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "United Kingdom", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 236, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "United States", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 237, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "United States Minor Outlying Islands", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 238, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Uruguay", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 239, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Uzbekistan", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 240, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Vanuatu", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 241, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Venezuela, Bolivarian Republic of", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 242, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Vietnam", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 243, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Virgin Islands, British", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 244, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Virgin Islands, U.S.", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 245, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Wallis and Futuna", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 246, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Western Sahara", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 247, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Yemen", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 248, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Zambia", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) },
                    { 249, new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), 0, "Zimbabwe", new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CountryId", "CreatedAt", "DateOfBirth", "ImageId" },
                values: new object[] { 1, new DateTime(2025, 3, 7, 12, 34, 59, 131, DateTimeKind.Utc).AddTicks(8386), new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ImageId",
                table: "Users",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Attachments_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Attachments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Countrys_CountryId",
                table: "Users",
                column: "CountryId",
                principalTable: "Countrys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Attachments_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countrys_CountryId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Countrys");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountryId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ImageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Users");

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
                column: "CreatedAt",
                value: new DateTime(2025, 3, 7, 7, 17, 46, 508, DateTimeKind.Utc).AddTicks(557));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
