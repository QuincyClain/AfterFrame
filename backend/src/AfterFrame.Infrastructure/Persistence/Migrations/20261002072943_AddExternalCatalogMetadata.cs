using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterFrame.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalCatalogMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "external_rating",
                table: "titles",
                type: "numeric(5,3)",
                precision: 5,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "external_vote_count",
                table: "titles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "poster_path",
                table: "titles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "title_genres",
                columns: table => new
                {
                    title_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_title_genres", x => new { x.title_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_title_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_title_genres_titles_title_id",
                        column: x => x.title_id,
                        principalTable: "titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "ck_titles_external_rating",
                table: "titles",
                sql: "(\n    \"origin\" = 'External'\n    AND\n    (\n        (\n            \"external_rating\" IS NULL\n            AND \"external_vote_count\" = 0\n        )\n        OR\n        (\n            \"external_rating\" IS NOT NULL\n            AND \"external_rating\" >= 0\n            AND \"external_rating\" <= 10\n            AND \"external_vote_count\" > 0\n        )\n    )\n)\nOR\n(\n    \"origin\" = 'UserCreated'\n    AND \"external_rating\" IS NULL\n    AND \"external_vote_count\" = 0\n)");

            migrationBuilder.CreateIndex(
                name: "ux_genres_name",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_title_genres_genre_id",
                table: "title_genres",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "title_genres");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropCheckConstraint(
                name: "ck_titles_external_rating",
                table: "titles");

            migrationBuilder.DropColumn(
                name: "external_rating",
                table: "titles");

            migrationBuilder.DropColumn(
                name: "external_vote_count",
                table: "titles");

            migrationBuilder.DropColumn(
                name: "poster_path",
                table: "titles");
        }
    }
}
