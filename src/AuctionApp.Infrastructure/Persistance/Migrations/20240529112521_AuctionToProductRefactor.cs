using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuctionToProductRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_Auctions_AuctionId",
                table: "AuctionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropTable(
                name: "CategoryLot");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.RenameColumn(
                name: "AuctionId",
                table: "UserWatchlists",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists",
                newName: "IX_UserWatchlists_UserId_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlists_AuctionId",
                table: "UserWatchlists",
                newName: "IX_UserWatchlists_ProductId");

            migrationBuilder.RenameColumn(
                name: "LotId",
                table: "Bids",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_LotId",
                table: "Bids",
                newName: "IX_Bids_ProductId");

            migrationBuilder.RenameColumn(
                name: "AuctionId",
                table: "AuctionReviews",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReviews_AuctionId",
                table: "AuctionReviews",
                newName: "IX_AuctionReviews_ProductId");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InitialPrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    LotsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoriesId, x.LotsId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Products_LotsId",
                        column: x => x.LotsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_LotsId",
                table: "CategoryProduct",
                column: "LotsId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorId",
                table: "Products",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_Products_ProductId",
                table: "AuctionReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Products_ProductId",
                table: "Bids",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlists_Products_ProductId",
                table: "UserWatchlists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionReviews_Products_ProductId",
                table: "AuctionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Products_ProductId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Products_ProductId",
                table: "UserWatchlists");

            migrationBuilder.DropTable(
                name: "CategoryProduct");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "UserWatchlists",
                newName: "AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlists_UserId_ProductId",
                table: "UserWatchlists",
                newName: "IX_UserWatchlists_UserId_AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchlists_ProductId",
                table: "UserWatchlists",
                newName: "IX_UserWatchlists_AuctionId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Bids",
                newName: "LotId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_ProductId",
                table: "Bids",
                newName: "IX_Bids_LotId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AuctionReviews",
                newName: "AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionReviews_ProductId",
                table: "AuctionReviews",
                newName: "IX_AuctionReviews_AuctionId");

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    EndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    InitialPrice = table.Column<decimal>(type: "money", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryLot",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    LotsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLot", x => new { x.CategoriesId, x.LotsId });
                    table.ForeignKey(
                        name: "FK_CategoryLot_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryLot_Lots_LotsId",
                        column: x => x.LotsId,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLot_LotsId",
                table: "CategoryLot",
                column: "LotsId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AuctionId",
                table: "Lots",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionReviews_Auctions_AuctionId",
                table: "AuctionReviews",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
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
    }
}
