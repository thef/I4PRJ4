using Microsoft.EntityFrameworkCore.Migrations;

namespace myWebApp.Migrations
{
    public partial class OnlineWebLaunch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150);
        }
    }
}
