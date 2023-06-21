using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class TryLastMigrationAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EstimatedTime",
                table: "Tours",
                type: "interval",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "TourId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedTime",
                table: "Tours",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "TourId");
        }
    }
}
