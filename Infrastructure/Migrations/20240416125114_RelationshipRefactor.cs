using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "LotId",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "LotId",
                table: "Bids",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id");
        }
    }
}
