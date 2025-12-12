using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendMpact.Migrations
{
    /// <inheritdoc />
    public partial class addTaskDatabasessss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Tasks",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerComments",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ManagerComments",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "Tasks");
        }
    }
}
