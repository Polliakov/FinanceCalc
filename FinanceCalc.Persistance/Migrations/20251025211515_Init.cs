using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceCalc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonds",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Nominal = table.Column<decimal>(type: "numeric(16,6)", precision: 16, scale: 6, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(16,6)", precision: 16, scale: 6, nullable: false),
                    Coupon = table.Column<decimal>(type: "numeric(16,6)", precision: 16, scale: 6, nullable: true),
                    CouponsPerYear = table.Column<int>(type: "integer", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationYears = table.Column<double>(type: "double precision", nullable: false),
                    CouponsPeriodMonths = table.Column<double>(type: "double precision", nullable: true),
                    CouponProfitability = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: true),
                    CouponProfitabilityYear = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: true),
                    CapitalProfitability = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: false),
                    CapitalProfitabilityYear = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: false),
                    ProfitabilityYear = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonds", x => new { x.Ticker, x.DateStart });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonds");
        }
    }
}
