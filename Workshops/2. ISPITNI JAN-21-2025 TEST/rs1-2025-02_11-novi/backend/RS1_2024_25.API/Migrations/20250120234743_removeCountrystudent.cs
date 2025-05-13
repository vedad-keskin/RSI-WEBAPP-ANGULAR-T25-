using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class removeCountrystudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Countries_BirthCountryID",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_BirthCountryID",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BirthCountryID",
                table: "Students");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "BirthCountryID",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_BirthCountryID",
                table: "Students",
                column: "BirthCountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Countries_BirthCountryID",
                table: "Students",
                column: "BirthCountryID",
                principalTable: "Countries",
                principalColumn: "ID");
        }
    }
}
