using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewKarma.Migrations
{
    public partial class migNullableCreateDateTimeProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                nullable: true,
                defaultValueSql: "CONVERT(datetime,GetDate())",
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "CONVERT(datetime,GetDate())");
        }
    }
}
