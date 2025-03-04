using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthSync.Migrations
{
    /// <inheritdoc />
    public partial class addIconToSaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "Contributions");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Savings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Savings");

            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "Contributions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
