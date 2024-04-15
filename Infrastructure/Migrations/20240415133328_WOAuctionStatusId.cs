using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WOAuctionStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionStatusId",
                table: "AuctionStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuctionStatusId",
                table: "AuctionStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuctionStatusId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuctionStatusId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "AuctionStatusId",
                value: 0);
        }
    }
}
