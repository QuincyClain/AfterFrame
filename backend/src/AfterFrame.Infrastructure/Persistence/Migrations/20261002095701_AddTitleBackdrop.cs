using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfterFrame.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleBackdrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "backdrop_path",
                table: "titles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "backdrop_path",
                table: "titles");
        }
    }
}
