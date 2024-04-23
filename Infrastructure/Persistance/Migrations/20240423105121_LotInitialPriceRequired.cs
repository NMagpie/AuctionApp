using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LotInitialPriceRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "UserWatchlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "Lots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialPrice",
                table: "Lots",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists",
                columns: new[] { "UserId", "AuctionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "UserWatchlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionId",
                table: "Lots",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialPrice",
                table: "Lots",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists",
                columns: new[] { "UserId", "AuctionId" },
                unique: true,
                filter: "[AuctionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }
    }
}
