using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceCalc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAciNextCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccumulatedCouponIncome",
                table: "Bonds",
                type: "numeric(16,6)",
                precision: 16,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextCouponDate",
                table: "Bonds",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedCouponIncome",
                table: "Bonds");

            migrationBuilder.DropColumn(
                name: "NextCouponDate",
                table: "Bonds");
        }
    }
}
