using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyReviewWeb.Data.Migrations
{
    public partial class LikesCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Reviews",
                type: "int",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Reviews");
        }
    }
}
