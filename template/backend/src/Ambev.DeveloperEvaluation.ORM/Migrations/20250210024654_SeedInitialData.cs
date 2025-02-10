using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Username", "Password", "Email", "Phone", "Status", "Role", "CreatedAt" },
                values: new object[] { 
                    Guid.NewGuid(), 
                    "admin", 
                    "hashed_password_here", 
                    "admin@example.com",
                    "+5511999999999",
                    "Active",
                    "Admin",
                    DateTime.UtcNow            
                }
            );

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Name", "Description", "Price", "Stock", "Category", "Image", "CreatedAt", "CreatedBy"},
                values: new object[] {
                    Guid.NewGuid(),
                    "Brahma",
                    "Brahma Cerveja Puro Malte 350ml",
                    99.99,
                    30,
                    "Cerveja",
                    "image_url_1",
                    DateTime.UtcNow,
                    "admin",
                }
            );

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Name", "Description", "Price", "Stock", "Category", "Image", "CreatedAt", "CreatedBy" },
                values: new object[] {
                    Guid.NewGuid(),
                    "Guarana",
                    "Guarana Antartica 2L",
                    6,
                    18,
                    "Refrigerante",
                    "image_url_1",
                    DateTime.UtcNow,
                    "admin",
                }
            );
        }

            /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DeleteData(
               table: "users",
               keyColumn: "Email",
               keyValue: "admin@example.com"
           );

           migrationBuilder.DeleteData(
               table: "products",
               keyColumn: "Name",
               keyValue: "Brahma"
           );

           migrationBuilder.DeleteData(
               table: "products",
               keyColumn: "Name",
               keyValue: "Guarana"
           );
        }
    }
}
