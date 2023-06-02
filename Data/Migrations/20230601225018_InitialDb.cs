using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Opis = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Description", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Adres = table.Column<string>(type: "TEXT", nullable: true),
                    NIP = table.Column<string>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task_",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Piority = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    KlientId = table.Column<int>(type: "INTEGER", nullable: true),
                    Pracownik = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Task_Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task_", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task__Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Task__Klient_KlientId",
                        column: x => x.KlientId,
                        principalTable: "Klient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Task__Task__Task_Id",
                        column: x => x.Task_Id,
                        principalTable: "Task_",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Komentarz = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Task_Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Task__Task_Id",
                        column: x => x.Task_Id,
                        principalTable: "Task_",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Task_Id",
                table: "Comments",
                column: "Task_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Task__DescriptionId",
                table: "Task_",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Task__KlientId",
                table: "Task_",
                column: "KlientId");

            migrationBuilder.CreateIndex(
                name: "IX_Task__Task_Id",
                table: "Task_",
                column: "Task_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Task_");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "Klient");
        }
    }
}
