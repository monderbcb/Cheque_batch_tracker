using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chequebatchtracker.Migrations
{
    /// <inheritdoc />
    public partial class modelupdates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UsedBatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Batch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Batch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalUsedFunds",
                table: "Batch",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UsedBatches");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Batch");

            migrationBuilder.DropColumn(
                name: "TotalUsedFunds",
                table: "Batch");
        }
    }
}
