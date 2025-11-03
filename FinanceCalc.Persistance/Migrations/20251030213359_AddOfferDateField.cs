using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceCalc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OfferDate",
                table: "Bonds",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferDate",
                table: "Bonds");
        }
    }
}
