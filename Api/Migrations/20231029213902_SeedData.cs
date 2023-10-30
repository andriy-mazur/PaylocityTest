using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "Salary" },
                values: new object[,]
                {
                    { 1, new DateTime(1984, 12, 30), "LeBron", "James", 75420.99m },
                    { 2, new DateTime(1999, 8, 10), "Ja", "Morant", 92365.22m },
                    { 3, new DateTime(1963, 2, 17), "Michael", "Jordan", 143211.12m }
                });

            migrationBuilder.InsertData(
                table: "Dependents",
                columns: new[] { "Id", "DateOfBirth", "EmployeeId", "FirstName", "LastName", "Relationship" },
                values: new object[,]
                {
                    { 1, new DateTime(1998, 3, 3), 2, "Spouse", "Morant", 1 },
                    { 2, new DateTime(2020, 6, 23), 2, "Child1", "Morant", 3 },
                    { 3, new DateTime(2021, 5, 18), 2, "Child2", "Morant", 3 },
                    { 4, new DateTime(1974, 1, 2), 3, "DP", "Jordan", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dependents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Dependents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Dependents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Dependents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
