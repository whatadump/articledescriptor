using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleDescriptor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddIsErrorToFeedEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_error",
                table: "classification_entries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb1d71c5-55e3-4e6e-8cc2-4b66ae3e5af3", "AQAAAAIAAYagAAAAENN03m5zaLIKSwdeUNIeI+ilqrH1qtSsxID8OdHcHBTmPkDGiVBkVJOYo5whLWWH4Q==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_error",
                table: "classification_entries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "06d7c731-e5cb-49e3-8d4c-f6316b46d361", "AQAAAAIAAYagAAAAEKU0bjwL2Virpe+wk2FWE3Kx9izp05byVS2E2Wr4oDg0M7tDI+M3Ka6oNRn7yquD0g==" });
        }
    }
}
