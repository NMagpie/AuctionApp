using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedVerboseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_Users_UserId",
                table: "AuctionReviews");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReviews_AuctionId",
                table: "AuctionReviews");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReviews_UserId",
                table: "AuctionReviews");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReviews_AuctionId",
                table: "AuctionReviews",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReviews_UserId",
                table: "AuctionReviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_Users_UserId",
                table: "AuctionReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_Users_UserId",
                table: "AuctionReviews");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReviews_AuctionId",
                table: "AuctionReviews");

            migrationBuilder.DropIndex(
                name: "IX_AuctionReviews_UserId",
                table: "AuctionReviews");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReviews_AuctionId",
                table: "AuctionReviews",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionReviews_UserId",
                table: "AuctionReviews",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_Users_UserId",
                table: "AuctionReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
