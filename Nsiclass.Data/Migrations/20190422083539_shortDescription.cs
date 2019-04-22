using Microsoft.EntityFrameworkCore.Migrations;

namespace Nsiclass.Data.Migrations
{
    public partial class shortDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "ClassItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "ClassItems");
        }
    }
}
