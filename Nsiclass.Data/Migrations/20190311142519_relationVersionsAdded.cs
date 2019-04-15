using Microsoft.EntityFrameworkCore.Migrations;

namespace Nsiclass.Data.Migrations
{
    public partial class relationVersionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassRelationsTypes_DestClassifId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationsTypes_SrcClassifId",
                table: "ClassRelationsTypes");

            migrationBuilder.AddColumn<string>(
                name: "DestVersionId",
                table: "ClassRelationsTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SrcVersionId",
                table: "ClassRelationsTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_DestClassifId_DestVersionId",
                table: "ClassRelationsTypes",
                columns: new[] { "DestClassifId", "DestVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_SrcClassifId_SrcVersionId",
                table: "ClassRelationsTypes",
                columns: new[] { "SrcClassifId", "SrcVersionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRelationsTypes_ClassVersions_DestClassifId_DestVersionId",
                table: "ClassRelationsTypes",
                columns: new[] { "DestClassifId", "DestVersionId" },
                principalTable: "ClassVersions",
                principalColumns: new[] { "Classif", "Version" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRelationsTypes_ClassVersions_SrcClassifId_SrcVersionId",
                table: "ClassRelationsTypes",
                columns: new[] { "SrcClassifId", "SrcVersionId" },
                principalTable: "ClassVersions",
                principalColumns: new[] { "Classif", "Version" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRelationsTypes_ClassVersions_DestClassifId_DestVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassRelationsTypes_ClassVersions_SrcClassifId_SrcVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationsTypes_DestClassifId_DestVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationsTypes_SrcClassifId_SrcVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropColumn(
                name: "DestVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.DropColumn(
                name: "SrcVersionId",
                table: "ClassRelationsTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_DestClassifId",
                table: "ClassRelationsTypes",
                column: "DestClassifId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_SrcClassifId",
                table: "ClassRelationsTypes",
                column: "SrcClassifId");
        }
    }
}
