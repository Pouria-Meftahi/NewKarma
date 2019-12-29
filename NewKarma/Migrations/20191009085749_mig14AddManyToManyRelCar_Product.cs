using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class mig14AddManyToManyRelCar_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RlCarModelProducts_CarModels_CarModelId",
                table: "RlCarModelProducts");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RlCarModelProducts",
                table: "RlCarModelProducts");

            migrationBuilder.DropIndex(
                name: "IX_RlCarModelProducts_CarModelId",
                table: "RlCarModelProducts");

            migrationBuilder.DropColumn(
                name: "CarModelId",
                table: "RlCarModelProducts");

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RlCarModelProducts",
                table: "RlCarModelProducts",
                columns: new[] { "CarId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RlCarModelProducts",
                table: "RlCarModelProducts");

            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarModelId",
                table: "RlCarModelProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RlCarModelProducts",
                table: "RlCarModelProducts",
                columns: new[] { "CarId", "CarModelId", "ProductId" });

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

            migrationBuilder.CreateIndex(
                name: "IX_RlCarModelProducts_CarModelId",
                table: "RlCarModelProducts",
                column: "CarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_RlCarModelProducts_CarModels_CarModelId",
                table: "RlCarModelProducts",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "CarModelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
