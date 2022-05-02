using Microsoft.EntityFrameworkCore.Migrations;

namespace NutritionWebApplication.Migrations
{
    public partial class food_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Foods",
                newName: "Protein");

            migrationBuilder.AddColumn<string>(
                name: "Carbohydrate",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fat",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carbohydrate",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "Foods",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Foods",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
