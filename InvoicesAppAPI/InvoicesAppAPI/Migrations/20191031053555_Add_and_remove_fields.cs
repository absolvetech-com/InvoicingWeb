using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoicesAppAPI.Migrations
{
    public partial class Add_and_remove_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "BussinessDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "BussinessDetails");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
