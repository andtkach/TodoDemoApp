using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasks.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Audio",
                table: "TodoItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audio",
                table: "TodoItems");
        }
    }
}
