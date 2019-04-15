using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nsiclass.Data.Migrations
{
    public partial class classification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    NameEng = table.Column<string>(maxLength: 250, nullable: true),
                    IsHierachy = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 5000, nullable: true),
                    isDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassRelationsTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    SrcClassifId = table.Column<string>(nullable: false),
                    DestClassifId = table.Column<string>(nullable: false),
                    Valid_From = table.Column<DateTime>(nullable: false),
                    Valid_To = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRelationsTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRelationsTypes_Classifications_DestClassifId",
                        column: x => x.DestClassifId,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRelationsTypes_Classifications_SrcClassifId",
                        column: x => x.SrcClassifId,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassVersions",
                columns: table => new
                {
                    Classif = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Valid_From = table.Column<DateTime>(nullable: false),
                    Valid_To = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(maxLength: 5000, nullable: true),
                    isDeleted = table.Column<bool>(nullable: false),
                    Parent = table.Column<string>(maxLength: 50, nullable: true),
                    ByLow = table.Column<string>(maxLength: 500, nullable: true),
                    Publications = table.Column<string>(maxLength: 500, nullable: true),
                    UseAreas = table.Column<string>(maxLength: 1000, nullable: true),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassVersions", x => new { x.Classif, x.Version });
                    table.ForeignKey(
                        name: "FK_ClassVersions_Classifications_Classif",
                        column: x => x.Classif,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassItems",
                columns: table => new
                {
                    Classif = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    ItemCode = table.Column<string>(nullable: false),
                    OtherCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    DescriptionEng = table.Column<string>(maxLength: 1000, nullable: true),
                    Includes = table.Column<string>(maxLength: 5000, nullable: true),
                    IncludesMore = table.Column<string>(maxLength: 1000, nullable: true),
                    IncludesNo = table.Column<string>(maxLength: 1000, nullable: true),
                    OrderNo = table.Column<int>(nullable: true),
                    ParentItemCode = table.Column<string>(nullable: true),
                    ItemLevel = table.Column<int>(nullable: false),
                    IsLeaf = table.Column<bool>(nullable: false),
                    EnteredByUserId = table.Column<string>(nullable: true),
                    EntryTime = table.Column<DateTime>(nullable: false),
                    ModifiedByUserId = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassItems", x => new { x.Classif, x.Version, x.ItemCode });
                    table.ForeignKey(
                        name: "FK_ClassItems_AspNetUsers_EnteredByUserId",
                        column: x => x.EnteredByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassItems_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassItems_ClassVersions_Classif_Version",
                        columns: x => new { x.Classif, x.Version },
                        principalTable: "ClassVersions",
                        principalColumns: new[] { "Classif", "Version" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassItems_ClassItems_Classif_Version_ParentItemCode",
                        columns: x => new { x.Classif, x.Version, x.ParentItemCode },
                        principalTable: "ClassItems",
                        principalColumns: new[] { "Classif", "Version", "ItemCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassRelations",
                columns: table => new
                {
                    SrcClassif = table.Column<string>(nullable: false),
                    SrcVer = table.Column<string>(nullable: false),
                    SrcItemId = table.Column<string>(nullable: false),
                    DestClassif = table.Column<string>(nullable: false),
                    DestVer = table.Column<string>(nullable: false),
                    DestItemId = table.Column<string>(nullable: false),
                    RelationTypeId = table.Column<string>(nullable: false),
                    EnteredByUserId = table.Column<string>(nullable: true),
                    EntryTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRelations", x => new { x.SrcClassif, x.SrcVer, x.SrcItemId, x.DestClassif, x.DestVer, x.DestItemId, x.RelationTypeId });
                    table.ForeignKey(
                        name: "FK_ClassRelations_AspNetUsers_EnteredByUserId",
                        column: x => x.EnteredByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRelations_ClassRelationsTypes_RelationTypeId",
                        column: x => x.RelationTypeId,
                        principalTable: "ClassRelationsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRelations_ClassItems_DestClassif_DestVer_DestItemId",
                        columns: x => new { x.DestClassif, x.DestVer, x.DestItemId },
                        principalTable: "ClassItems",
                        principalColumns: new[] { "Classif", "Version", "ItemCode" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassRelations_ClassItems_SrcClassif_SrcVer_SrcItemId",
                        columns: x => new { x.SrcClassif, x.SrcVer, x.SrcItemId },
                        principalTable: "ClassItems",
                        principalColumns: new[] { "Classif", "Version", "ItemCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassItems_EnteredByUserId",
                table: "ClassItems",
                column: "EnteredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassItems_ModifiedByUserId",
                table: "ClassItems",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassItems_Classif_Version_ParentItemCode",
                table: "ClassItems",
                columns: new[] { "Classif", "Version", "ParentItemCode" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelations_EnteredByUserId",
                table: "ClassRelations",
                column: "EnteredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelations_RelationTypeId",
                table: "ClassRelations",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelations_DestClassif_DestVer_DestItemId",
                table: "ClassRelations",
                columns: new[] { "DestClassif", "DestVer", "DestItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_DestClassifId",
                table: "ClassRelationsTypes",
                column: "DestClassifId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationsTypes_SrcClassifId",
                table: "ClassRelationsTypes",
                column: "SrcClassifId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassRelations");

            migrationBuilder.DropTable(
                name: "ClassRelationsTypes");

            migrationBuilder.DropTable(
                name: "ClassItems");

            migrationBuilder.DropTable(
                name: "ClassVersions");

            migrationBuilder.DropTable(
                name: "Classifications");
        }
    }
}
