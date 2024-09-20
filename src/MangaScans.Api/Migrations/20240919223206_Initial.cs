using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MangaScans.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Views = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Likes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Dislikes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", unicode: false, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategorysMangas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    MangaId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorysMangas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorysMangas_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorysMangas_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdManga = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Mangas_IdManga",
                        column: x => x.IdManga,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdChapter = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImagePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Chapters_IdChapter",
                        column: x => x.IdChapter,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1218), "Action" },
                    { 2, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1220), "Adventure" },
                    { 3, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1221), "Comedy" },
                    { 4, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1222), "Drama" },
                    { 5, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1222), "Romance" },
                    { 6, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1234), "Mystery" },
                    { 7, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1235), "Suspense" },
                    { 8, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1236), "Fantasy" },
                    { 9, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1236), "Sci-Fi" },
                    { 10, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1237), "Horror" },
                    { 11, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1238), "Slice of Life" },
                    { 12, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1239), "Supernatural" },
                    { 13, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1239), "Historical" },
                    { 14, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1240), "Sports" },
                    { 15, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1241), "Harem" },
                    { 16, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1242), "Yaoi" },
                    { 17, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1242), "Yuri" },
                    { 18, new DateTime(2024, 9, 19, 22, 32, 5, 856, DateTimeKind.Utc).AddTicks(1243), "Isekai" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorysMangas_CategoryId",
                table: "CategorysMangas",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorysMangas_MangaId",
                table: "CategorysMangas",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_IdManga",
                table: "Chapters",
                column: "IdManga");

            migrationBuilder.CreateIndex(
                name: "IX_Images_IdChapter",
                table: "Images",
                column: "IdChapter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorysMangas");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Mangas");
        }
    }
}
