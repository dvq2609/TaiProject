using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add Stock column first
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 2. Copy the existing stock counts from InStock to Stock
            migrationBuilder.Sql("UPDATE [Product] SET [Stock] = [InStock]");

            // 3. Alter InStock column to bit
            migrationBuilder.AlterColumn<bool>(
                name: "InStock",
                table: "Product",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 4. Initialize InStock boolean flag based on Stock count
            migrationBuilder.Sql("UPDATE [Product] SET [InStock] = CASE WHEN [Stock] > 0 THEN 1 ELSE 0 END");

            migrationBuilder.AddColumn<string>(
                name: "CareLevel",
                table: "Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Product",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "Product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareLevel",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "InStock",
                table: "Product",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
