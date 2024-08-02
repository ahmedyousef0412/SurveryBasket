using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Infrastruction.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledColumnToTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b5a9635-aa1d-4e42-b0d9-1169289a4b95",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEL70IXZzyttRgFJfaQr7vwvTgnyjSkPISiIXq5TJ9SgHbv5nVpFhtwyycBzSSmx4+Q==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b5a9635-aa1d-4e42-b0d9-1169289a4b95",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG0nvPvQg4nHwGJeONlUTxWLaJrqv+DCyxJ+zNmP3Zu5kHwdVyVvgVFWGkPZGZv2UA==");
        }
    }
}
