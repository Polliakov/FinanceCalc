using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceCalc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBondMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BondMetrics",
                columns: table => new
                {
                    CalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MeanCostDiffPercent = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorCostDiffPercent = table.Column<double>(type: "double precision", nullable: false),
                    CostDiffDistribution = table.Column<string>(type: "text", nullable: false),
                    MeanCouponYieldYear = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorCouponYieldYear = table.Column<double>(type: "double precision", nullable: false),
                    CouponYieldYearDistribution = table.Column<string>(type: "text", nullable: false),
                    MeanTotalYieldYear = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorTotalYieldYear = table.Column<double>(type: "double precision", nullable: false),
                    TotalYieldYearDistribution = table.Column<string>(type: "text", nullable: false),
                    MeanDurationYears = table.Column<double>(type: "double precision", nullable: false),
                    StdErrorDurationYears = table.Column<double>(type: "double precision", nullable: false),
                    DurationYearsDistribution = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondMetrics", x => x.CalculatedAt);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BondMetrics");
        }
    }
}
