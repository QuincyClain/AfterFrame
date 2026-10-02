using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterFrame.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "titles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tags = table.Column<int>(type: "integer", nullable: false),
                    release_year = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    origin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    publication_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    external_source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    external_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titles", x => x.id);
                    table.CheckConstraint("ck_titles_origin", "(\n    \"origin\" = 'External'\n    AND \"publication_status\" = 'Published'\n    AND \"external_source\" IS NOT NULL\n    AND \"external_id\" IS NOT NULL\n    AND \"created_by_user_id\" IS NULL\n)\nOR\n(\n    \"origin\" = 'UserCreated'\n    AND \"external_source\" IS NULL\n    AND \"external_id\" IS NULL\n    AND \"created_by_user_id\" IS NOT NULL\n)");
                    table.CheckConstraint("ck_titles_release_year", "\"release_year\" > 0");
                    table.ForeignKey(
                        name: "FK_titles_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "library_entries",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rating = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true),
                    review = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    last_watched_season = table.Column<int>(type: "integer", nullable: true),
                    last_watched_episode = table.Column<int>(type: "integer", nullable: true),
                    created_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_library_entries", x => new { x.user_id, x.title_id });
                    table.CheckConstraint("ck_library_entries_progress", "(\n    \"last_watched_season\" IS NULL\n    AND \"last_watched_episode\" IS NULL\n)\nOR\n(\n    \"last_watched_season\" IS NOT NULL\n    AND \"last_watched_season\" > 0\n    AND \"last_watched_episode\" IS NOT NULL\n    AND \"last_watched_episode\" > 0\n)");
                    table.CheckConstraint("ck_library_entries_rating", "\"rating\" IS NULL\nOR\n(\n    \"rating\" >= 0.5\n    AND \"rating\" <= 10\n    AND MOD(\"rating\" * 2, 1) = 0\n)");
                    table.ForeignKey(
                        name: "FK_library_entries_titles_title_id",
                        column: x => x.title_id,
                        principalTable: "titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_library_entries_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at_utc", "display_name" },
                values: new object[] { new Guid("0199f3f4-7c00-7000-8000-000000000001"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Demo User" });

            migrationBuilder.CreateIndex(
                name: "ix_library_entries_title_id",
                table: "library_entries",
                column: "title_id");

            migrationBuilder.CreateIndex(
                name: "ix_library_entries_user_id_status",
                table: "library_entries",
                columns: new[] { "user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_titles_created_by_user_id",
                table: "titles",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ux_titles_external_identity",
                table: "titles",
                columns: new[] { "external_source", "external_id" },
                unique: true,
                filter: "\"external_source\" IS NOT NULL AND \"external_id\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "library_entries");

            migrationBuilder.DropTable(
                name: "titles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
