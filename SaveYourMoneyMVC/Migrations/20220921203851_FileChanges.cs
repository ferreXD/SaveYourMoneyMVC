using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveYourMoneyMVC.Migrations
{
    public partial class FileChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Files",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Files",
                newName: "Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Files",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "Content");
        }
    }
}
