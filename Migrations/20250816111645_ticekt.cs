using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_ticket.Migrations
{
    /// <inheritdoc />
    public partial class ticekt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_applicationUserId",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "IdapplicationUser",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "tickets",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_applicationUserId",
                table: "tickets",
                newName: "IX_tickets_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_ApplicationUserId",
                table: "tickets",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AspNetUsers_ApplicationUserId",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "tickets",
                newName: "applicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_ApplicationUserId",
                table: "tickets",
                newName: "IX_tickets_applicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "IdapplicationUser",
                table: "tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AspNetUsers_applicationUserId",
                table: "tickets",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
