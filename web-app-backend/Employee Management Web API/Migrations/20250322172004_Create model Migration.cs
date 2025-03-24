using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee_Management_Web_API.Migrations
{
    /// <inheritdoc />
    public partial class CreatemodelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Employees",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Employees",
                newName: "isActive");
        }
    }
}
