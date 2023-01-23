using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chequebatchtracker.Migrations
{
    /// <inheritdoc />
    public partial class modelupdates4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amount",
                table: "UsedBatches",
                newName: "Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UsedBatches",
                newName: "amount");
        }
    }
}
