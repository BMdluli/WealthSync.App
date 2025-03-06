using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthSync.Migrations
{
    /// <inheritdoc />
    public partial class addStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Savings");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Savings",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Savings",
                newName: "TargetAmount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Contributions",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Savings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentAmount",
                table: "Savings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetDate",
                table: "Savings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Contributions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SavingsGoalId",
                table: "Contributions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Shares = table.Column<double>(type: "REAL", nullable: false),
                    PurchasePrice = table.Column<double>(type: "REAL", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dividends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dividends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dividends_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPrices_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dividends_StockId",
                table: "Dividends",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPrices_StockId",
                table: "StockPrices",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_AppUserId",
                table: "Stocks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings");

            migrationBuilder.DropTable(
                name: "Dividends");

            migrationBuilder.DropTable(
                name: "StockPrices");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "TargetDate",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "SavingsGoalId",
                table: "Contributions");

            migrationBuilder.RenameColumn(
                name: "TargetAmount",
                table: "Savings",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Savings",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Contributions",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Savings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Savings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
