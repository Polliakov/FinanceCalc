using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceCalc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRelevance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Relevance",
                table: "Bonds",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Relevance",
                table: "Bonds");
        }
    }
}
