using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanner.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixTourLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLog_Tour_TourId",
                table: "TourLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourLog",
                table: "TourLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tour",
                table: "Tour");

            migrationBuilder.RenameTable(
                name: "TourLog",
                newName: "TourLogs");

            migrationBuilder.RenameTable(
                name: "Tour",
                newName: "Tours");

            migrationBuilder.RenameIndex(
                name: "IX_TourLog_TourId",
                table: "TourLogs",
                newName: "IX_TourLogs_TourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourLogs",
                table: "TourLogs",
                column: "TourLogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tours",
                table: "Tours",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "TourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tours",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourLogs",
                table: "TourLogs");

            migrationBuilder.RenameTable(
                name: "Tours",
                newName: "Tour");

            migrationBuilder.RenameTable(
                name: "TourLogs",
                newName: "TourLog");

            migrationBuilder.RenameIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLog",
                newName: "IX_TourLog_TourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tour",
                table: "Tour",
                column: "TourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourLog",
                table: "TourLog",
                column: "TourLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLog_Tour_TourId",
                table: "TourLog",
                column: "TourId",
                principalTable: "Tour",
                principalColumn: "TourId");
        }
    }
}
