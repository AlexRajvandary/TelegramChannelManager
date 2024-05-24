using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyUpdateIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdateId",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateId",
                table: "User");
        }
    }
}
