using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendMpact.Migrations
{
    /// <inheritdoc />
    public partial class addTaskDatabases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedBy",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Tasks");
        }
    }
}
