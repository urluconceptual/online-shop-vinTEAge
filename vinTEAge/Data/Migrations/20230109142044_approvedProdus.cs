using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vinTEAge.Data.Migrations
{
    public partial class approvedProdus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Products");
        }
    }
}
