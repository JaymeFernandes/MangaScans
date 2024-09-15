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
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", unicode: false, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mangas_categories_IdCategory",
                        column: x => x.IdCategory,
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
                    { 1, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1747), "Action" },
                    { 2, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1749), "Adventure" },
                    { 3, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1749), "Comedy" },
                    { 4, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1750), "Drama" },
                    { 5, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1750), "Romance" },
                    { 6, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1751), "Mystery" },
                    { 7, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1752), "Suspense" },
                    { 8, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1752), "Fantasy" },
                    { 9, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1753), "Sci-Fi" },
                    { 10, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1754), "Horror" },
                    { 11, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1755), "Slice of Life" },
                    { 12, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1755), "Supernatural" },
                    { 13, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1756), "Historical" },
                    { 14, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1757), "Sports" },
                    { 15, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1757), "Harem" },
                    { 16, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1758), "Yaoi" },
                    { 17, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1758), "Yuri" },
                    { 18, new DateTime(2024, 9, 15, 1, 26, 56, 521, DateTimeKind.Utc).AddTicks(1759), "Isekai" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_IdManga",
                table: "Chapters",
                column: "IdManga");

            migrationBuilder.CreateIndex(
                name: "IX_Images_IdChapter",
                table: "Images",
                column: "IdChapter");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_IdCategory",
                table: "Mangas",
                column: "IdCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Mangas");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
