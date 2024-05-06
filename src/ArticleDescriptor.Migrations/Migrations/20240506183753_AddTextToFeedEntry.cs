using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleDescriptor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTextToFeedEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "classification_entries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "06d7c731-e5cb-49e3-8d4c-f6316b46d361", "AQAAAAIAAYagAAAAEKU0bjwL2Virpe+wk2FWE3Kx9izp05byVS2E2Wr4oDg0M7tDI+M3Ka6oNRn7yquD0g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "text",
                table: "classification_entries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "30765b1d-4dea-4392-bb2b-75a0938cf9bb", "AQAAAAIAAYagAAAAECt+xVaIvWDES1uEMf+zvleumfhE421iiVeRXnM2J6pa/M5Rl5x2wlb06LBxxWQ6zQ==" });
        }
    }
}
