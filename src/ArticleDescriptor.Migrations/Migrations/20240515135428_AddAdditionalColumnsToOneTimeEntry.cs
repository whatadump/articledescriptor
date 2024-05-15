using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleDescriptor.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalColumnsToOneTimeEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_error",
                table: "onetime_classification_entries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "normalized_text",
                table: "onetime_classification_entries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ea71556-17d6-46f4-9e39-11c5f26fc397", "AQAAAAIAAYagAAAAEMr49d1ymV1CH4DcX+MpwWlDi7Tj8LRckkWFIpEJEeJ5/ZQZHEMRcjmPchaCnkdIJQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_error",
                table: "onetime_classification_entries");

            migrationBuilder.DropColumn(
                name: "normalized_text",
                table: "onetime_classification_entries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5c0f85b-3d01-4f65-b3de-818c37d6b9b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb1d71c5-55e3-4e6e-8cc2-4b66ae3e5af3", "AQAAAAIAAYagAAAAENN03m5zaLIKSwdeUNIeI+ilqrH1qtSsxID8OdHcHBTmPkDGiVBkVJOYo5whLWWH4Q==" });
        }
    }
}
