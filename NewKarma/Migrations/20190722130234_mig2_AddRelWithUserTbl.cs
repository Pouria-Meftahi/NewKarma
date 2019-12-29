using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class mig2_AddRelWithUserTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Products",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "UserIDFK",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIDFK",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIDFK",
                table: "Brands",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "BrandId");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarName = table.Column<string>(nullable: false),
                    UserIDFK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                    table.ForeignKey(
                        name: "FK_Cars_AppUser_UserIDFK",
                        column: x => x.UserIDFK,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserIDFK",
                table: "Products",
                column: "UserIDFK");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserIDFK",
                table: "Categories",
                column: "UserIDFK");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_UserIDFK",
                table: "Brands",
                column: "UserIDFK");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UserIDFK",
                table: "Cars",
                column: "UserIDFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AppUser_UserIDFK",
                table: "Brands",
                column: "UserIDFK",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AppUser_UserIDFK",
                table: "Categories",
                column: "UserIDFK",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppUser_UserIDFK",
                table: "Products",
                column: "UserIDFK",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AppUser_UserIDFK",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AppUser_UserIDFK",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppUser_UserIDFK",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserIDFK",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserIDFK",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_UserIDFK",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "UserIDFK",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserIDFK",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserIDFK",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "Desc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
