using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Infrastruction.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1b1741e-2ece-4508-a835-3a041c6bb228", "5d608597-be0a-43f2-8500-d5dbcf763c49", false, false, "Admin", "ADMIN" },
                    { "ee41f1ef-cafa-4185-90a1-61b7329f5fae", "4f8627e4-1182-47f9-bfab-16dcb763e8e9", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6b5a9635-aa1d-4e42-b0d9-1169289a4b95", 0, "733f6b59-cb18-4365-bc4b-8f4c43898437", "admin@survery-basket.com", true, "Survery Basket", "Admin", false, null, "ADMIN@SURVERY-BASKET.COM", "ADMIN@SURVERY-BASKET.COM", "AQAAAAIAAYagAAAAEG0nvPvQg4nHwGJeONlUTxWLaJrqv+DCyxJ+zNmP3Zu5kHwdVyVvgVFWGkPZGZv2UA==", null, false, "47577BA05F7F4E199E0CCC9C4C2602D3", false, "admin@survery-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permessions", "polls:read", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 2, "permessions", "polls:add", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 3, "permessions", "polls:update", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 4, "permessions", "polls:delete", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 5, "permessions", "questions:read", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 6, "permessions", "questions:add", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 7, "permessions", "questions:update", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 8, "permessions", "users:read", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 9, "permessions", "users:add", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 10, "permessions", "users:update", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 11, "permessions", "roles:read", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 12, "permessions", "roles:add", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 13, "permessions", "roles:update", "a1b1741e-2ece-4508-a835-3a041c6bb228" },
                    { 14, "permessions", "results:read", "a1b1741e-2ece-4508-a835-3a041c6bb228" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1b1741e-2ece-4508-a835-3a041c6bb228", "6b5a9635-aa1d-4e42-b0d9-1169289a4b95" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee41f1ef-cafa-4185-90a1-61b7329f5fae");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1b1741e-2ece-4508-a835-3a041c6bb228", "6b5a9635-aa1d-4e42-b0d9-1169289a4b95" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b1741e-2ece-4508-a835-3a041c6bb228");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b5a9635-aa1d-4e42-b0d9-1169289a4b95");
        }
    }
}
