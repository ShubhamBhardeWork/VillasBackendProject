using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Villas.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "CreatedAt", "Details", "ImageUrl", "LastUpdatedAt", "Name", "Occupancy", "Rate", "Sqft" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxurious villa with stunning ocean views and private beach access.", "https://static.vecteezy.com/system/resources/thumbnails/035/974/293/small_2x/ai-generated-luxury-home-exterior-and-pool-on-sunny-day-with-blue-sky-free-photo.jpg", null, "Royal Villa", 6, 500m, 2500 },
                    { 2, new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant villa with marble interiors and panoramic mountain views.", "https://tse4.mm.bing.net/th/id/OIP.RpZHEyv0mnqtn6TdVjQNlwHaE7?pid=Api&P=0&h=180", new DateTime(2020, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diamond Villa", 8, 750m, 3200 },
                    { 3, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern villa featuring an infinity pool and outdoor entertainment area.", "https://tse4.mm.bing.net/th/id/OIP.kct6TCUyMIKQzWa2uXBNBQHaE8?pid=Api&P=0&h=180", null, "Pool Villa", 4, 350m, 1800 },
                    { 4, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium villa with spa facilities and concierge services.", "https://tse1.mm.bing.net/th/id/OIP.Ebeiw1CrSAS7JNkOrthplAHaEu?pid=Api&P=0&h=180", new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxury Villa", 10, 900m, 4000 },
                    { 5, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charming villa surrounded by tropical gardens and nature.", "https://tse3.mm.bing.net/th/id/OIP.FkHSoEx_TOwMkABTf2IrlQHaE8?rs=1&pid=ImgDetMain&o=7&rm=3", new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Garden Villa", 3, 275m, 1500 }
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
