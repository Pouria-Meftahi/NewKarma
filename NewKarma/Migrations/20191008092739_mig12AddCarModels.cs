using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class mig12AddCarModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUser_UserIDFK",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserIDFK",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "UserIDFK",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarTitle = table.Column<string>(nullable: false),
                    CarModel = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "rlCarProducts",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rlCarProducts", x => new { x.CarId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_rlCarProducts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rlCarProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rlCarProducts_ProductId",
                table: "rlCarProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rlCarProducts");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "UserIDFK",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserIDFK",
                table: "Categories",
                column: "UserIDFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUser_UserIDFK",
                table: "Categories",
                column: "UserIDFK",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
