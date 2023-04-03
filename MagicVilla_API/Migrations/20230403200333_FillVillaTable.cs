using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class FillVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreationDate", "Detail", "Fee", "ImageUrl", "Name", "Occupants", "SquareMeters", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Amenity 1", new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2506), "Villa detail 1", 1f, "", "Villa 1", 10, 80.5f, new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2524) },
                    { 2, "Amenity 2", new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2535), "Villa detail 2", 2f, "", "Villa 2", 20, 200f, new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2536) },
                    { 3, "Amenity 3", new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2540), "Villa detail 3", 3f, "", "Villa 3", 30, 300f, new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2541) },
                    { 4, "Amenity 4", new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2545), "Villa detail 4", 4f, "", "Villa 4", 40, 400f, new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2546) },
                    { 5, "Amenity 5", new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2549), "Villa detail 5", 5f, "", "Villa 5", 50, 500f, new DateTime(2023, 4, 3, 15, 3, 33, 576, DateTimeKind.Local).AddTicks(2550) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
