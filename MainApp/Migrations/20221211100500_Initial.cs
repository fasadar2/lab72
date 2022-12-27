using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MainApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Indications_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 1, "Test", "Test" });

            migrationBuilder.InsertData(
                table: "Houses",
                column: "Id",
                values: new object[]
                {
                    1,
                    2
                });

            migrationBuilder.InsertData(
                table: "Indications",
                columns: new[] { "Id", "EmployeeId", "HouseId", "TimeStamp", "Title", "Value" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2022, 12, 11, 13, 5, 0, 626, DateTimeKind.Local).AddTicks(7854), "Электричество", 100.0 },
                    { 2, 1, 1, new DateTime(2022, 12, 11, 13, 5, 0, 626, DateTimeKind.Local).AddTicks(7870), "Вода", 200.0 },
                    { 3, 1, 2, new DateTime(2022, 12, 11, 13, 5, 0, 626, DateTimeKind.Local).AddTicks(7871), "Электричество", 200.0 },
                    { 4, 1, 2, new DateTime(2022, 12, 11, 13, 5, 0, 626, DateTimeKind.Local).AddTicks(7872), "Вода", 400.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Indications_EmployeeId",
                table: "Indications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Indications_HouseId",
                table: "Indications",
                column: "HouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Indications");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Houses");
        }
    }
}
