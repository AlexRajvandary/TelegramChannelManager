using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelManager.API.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "User",
                newName: "MainChatId");

            migrationBuilder.AddColumn<Guid>(
                name: "LastEditedPostId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PersonalChatId",
                table: "User",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditedPostId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PersonalChatId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "MainChatId",
                table: "User",
                newName: "ChatId");
        }
    }
}
