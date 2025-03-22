using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Learning.Migrations
{
    /// <inheritdoc />
    public partial class AddTutortoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, "Tutor with Manage courses privilages", "Tutur" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
