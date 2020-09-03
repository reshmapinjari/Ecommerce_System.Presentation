using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce_WebApi.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductColor = table.Column<string>(nullable: true),
                    ProductSize = table.Column<string>(nullable: true),
                    ProductMaterial = table.Column<string>(nullable: true),
                    category = table.Column<int>(nullable: false),
                    ProductPrice = table.Column<double>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
