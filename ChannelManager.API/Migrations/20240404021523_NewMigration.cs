using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelManager.API.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BotToken",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BotToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "State",
                table: "User");
        }
    }
}
