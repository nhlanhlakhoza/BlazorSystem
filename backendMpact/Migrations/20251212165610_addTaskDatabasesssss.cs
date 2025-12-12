using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendMpact.Migrations
{
    /// <inheritdoc />
    public partial class addTaskDatabasesssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalRecipients",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRecipients",
                table: "Tasks");
        }
    }
}
