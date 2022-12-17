using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vinTEAge.Data.Migrations
{
    public partial class stersUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewUserId",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewUserId",
                table: "Reviews",
                type: "int",
                nullable: true);
        }
    }
}
