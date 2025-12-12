using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendMpact.Migrations
{
    /// <inheritdoc />
    public partial class addTaskDatabasesss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Image3",
                table: "Tasks",
                newName: "ImagePathsJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePathsJson",
                table: "Tasks",
                newName: "Image3");

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
