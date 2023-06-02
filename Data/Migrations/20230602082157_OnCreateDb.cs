using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class OnCreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Description",
                columns: new[] { "Id", "Opis" },
                values: new object[] { 1, "Opis" });
            migrationBuilder.InsertData(
                table: "Klient",
                columns: new[] { "Id", "Nazwa", "Adres", "NIP", "Telefon", "Email" },
                values: new object[] { 1, "Radio", "Adres", "NIP", "Telefon", "Email" });
            
            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Komentarz", "UserId" },
                values: new object[] { 1, "Comment", "Database" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Description",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "Klient",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
