using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class migNullableBrandProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandIDFK",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "BrandIDFK",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandIDFK",
                table: "Products",
                column: "BrandIDFK",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandIDFK",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "BrandIDFK",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandIDFK",
                table: "Products",
                column: "BrandIDFK",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
