using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuctionReviewToProductReviewRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_AspNetUsers_UserId",
                table: "AuctionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_Products_ProductId",
                table: "AuctionReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionReviews",
                table: "AuctionReviews");

            migrationBuilder.RenameTable(
                name: "AuctionReviews",
                newName: "ProductReview");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReviews_UserId",
                table: "ProductReview",
                newName: "IX_ProductReview_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReviews_ProductId",
                table: "ProductReview",
                newName: "IX_ProductReview_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AspNetUsers_UserId",
                table: "ProductReview",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AspNetUsers_UserId",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview");

            migrationBuilder.RenameTable(
                name: "ProductReview",
                newName: "AuctionReviews");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_UserId",
                table: "AuctionReviews",
                newName: "IX_AuctionReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_ProductId",
                table: "AuctionReviews",
                newName: "IX_AuctionReviews_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionReviews",
                table: "AuctionReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_AspNetUsers_UserId",
                table: "AuctionReviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_Products_ProductId",
                table: "AuctionReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
