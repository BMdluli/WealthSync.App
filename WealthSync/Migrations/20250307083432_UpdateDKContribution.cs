using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthSync.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDKContribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavingsGoalId",
                table: "Contributions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SavingsGoalId",
                table: "Contributions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
