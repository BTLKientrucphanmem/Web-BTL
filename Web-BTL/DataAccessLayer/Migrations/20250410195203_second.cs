using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_BTL.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "package",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "_ServicePackage",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "package",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_ServicePackage",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
