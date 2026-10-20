using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterFrame.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryEntryReviewDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "contains_spoilers",
                table: "library_entries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "reviewed_at_utc",
                table: "library_entries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vibe",
                table: "library_entries",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contains_spoilers",
                table: "library_entries");

            migrationBuilder.DropColumn(
                name: "reviewed_at_utc",
                table: "library_entries");

            migrationBuilder.DropColumn(
                name: "vibe",
                table: "library_entries");
        }
    }
}
