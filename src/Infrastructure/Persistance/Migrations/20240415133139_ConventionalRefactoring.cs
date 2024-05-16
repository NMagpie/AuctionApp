using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConventionalRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AuctionStatus_StatusId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropTable(
                name: "LotCategory");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_UserId",
                table: "UserWatchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionStatus",
                table: "AuctionStatus");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_StatusId",
                table: "Auctions");

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "AuctionStatusId",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "AuctionStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "AuctionStatusId",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AuctionStatus",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionStatus",
                table: "AuctionStatus",
                column: "Id");

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
                        name: "FK_CategoryLot_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryLot_Lots_LotsId",
                        column: x => x.LotsId,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuctionStatus",
                columns: new[] { "Id", "AuctionStatusId", "Status" },
                values: new object[,]
                {
                    { 1, 0, "Created" },
                    { 2, 0, "Active" },
                    { 3, 0, "Finished" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_AuctionId",
                table: "UserWatchlists",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists",
                columns: new[] { "UserId", "AuctionId" },
                unique: true,
                filter: "[AuctionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_StatusId",
                table: "Auctions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLot_LotsId",
                table: "CategoryLot",
                column: "LotsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AuctionStatus_StatusId",
                table: "Auctions",
                column: "StatusId",
                principalTable: "AuctionStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AuctionStatus_StatusId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropTable(
                name: "CategoryLot");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropIndex(
                name: "IX_UserWatchlists_UserId_AuctionId",
                table: "UserWatchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionStatus",
                table: "AuctionStatus");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_StatusId",
                table: "Auctions");

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AuctionStatus",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AuctionStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionStatus",
                table: "AuctionStatus",
                column: "AuctionStatusId");

            migrationBuilder.CreateTable(
                name: "LotCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotCategory", x => new { x.CategoryId, x.LotId });
                    table.ForeignKey(
                        name: "FK_LotCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotCategory_Lots_LotId",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuctionStatus",
                columns: new[] { "AuctionStatusId", "Status" },
                values: new object[,]
                {
                    { 0, "Created" },
                    { 1, "Active" },
                    { 2, "Finished" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_AuctionId",
                table: "UserWatchlists",
                column: "AuctionId",
                unique: true,
                filter: "[AuctionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlists_UserId",
                table: "UserWatchlists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                unique: true,
                filter: "[CreatorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_StatusId",
                table: "Auctions",
                column: "StatusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LotCategory_LotId",
                table: "LotCategory",
                column: "LotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AuctionStatus_StatusId",
                table: "Auctions",
                column: "StatusId",
                principalTable: "AuctionStatus",
                principalColumn: "AuctionStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Auctions_AuctionId",
                table: "Lots",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchlists_Auctions_AuctionId",
                table: "UserWatchlists",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
