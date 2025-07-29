using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class userdeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDeletedId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserDeletedId",
                table: "Students",
                column: "UserDeletedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_MyAppUsers_UserDeletedId",
                table: "Students",
                column: "UserDeletedId",
                principalTable: "MyAppUsers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_MyAppUsers_UserDeletedId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserDeletedId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserDeletedId",
                table: "Students");
        }
    }
}
