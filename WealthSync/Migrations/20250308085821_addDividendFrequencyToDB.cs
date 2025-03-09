using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthSync.Migrations
{
    /// <inheritdoc />
    public partial class addDividendFrequencyToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DividendFrequency",
                table: "Stocks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DividendFrequency",
                table: "Stocks");
        }
    }
}
