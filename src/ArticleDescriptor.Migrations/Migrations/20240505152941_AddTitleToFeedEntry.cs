using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleDescriptor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToFeedEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "classification_entries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "30765b1d-4dea-4392-bb2b-75a0938cf9bb", "AQAAAAIAAYagAAAAECt+xVaIvWDES1uEMf+zvleumfhE421iiVeRXnM2J6pa/M5Rl5x2wlb06LBxxWQ6zQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "classification_entries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0107145e-2e3e-4d9c-803c-19c8fadf08cb", "AQAAAAIAAYagAAAAEErHcfEBf9lzNx9FzsQdBHPrIUZYnF/sYg34dFyGqNMzOr0/UhXAXOm4ZcslrDgOoQ==" });
        }
    }
}
