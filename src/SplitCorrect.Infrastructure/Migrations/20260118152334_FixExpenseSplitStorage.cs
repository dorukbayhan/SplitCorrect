using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitCorrect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixExpenseSplitStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "ExpenseSplits",
                newName: "SplitCurrency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ExpenseSplits",
                newName: "SplitAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SplitCurrency",
                table: "ExpenseSplits",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "SplitAmount",
                table: "ExpenseSplits",
                newName: "Amount");
        }
    }
}
