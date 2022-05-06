using Microsoft.EntityFrameworkCore.Migrations;

namespace CarFactoryDatabaseImplement.Migrations
{
    public partial class AddMailsReplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyText",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Viewed",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyText",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Viewed",
                table: "Messages");
        }
    }
}
