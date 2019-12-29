using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class mig14AddTripleManyToManyRelCar_Model_Prod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rlCarProducts");

            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "Cars");

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    CarModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.CarModelId);
                });

            migrationBuilder.CreateTable(
                name: "RlCarModelProducts",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false),
                    CarModelId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RlCarModelProducts", x => new { x.CarId, x.CarModelId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_RlCarModelProducts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RlCarModelProducts_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "CarModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RlCarModelProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RlCarModelProducts_CarModelId",
                table: "RlCarModelProducts",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RlCarModelProducts_ProductId",
                table: "RlCarModelProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RlCarModelProducts");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "Cars",
                nullable: false,
                defaultValue: "");

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
    }
}
