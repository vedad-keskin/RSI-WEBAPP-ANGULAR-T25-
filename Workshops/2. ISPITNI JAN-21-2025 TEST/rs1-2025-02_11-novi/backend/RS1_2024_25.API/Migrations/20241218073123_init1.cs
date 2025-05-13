using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations;

/// <inheritdoc />
public partial class init1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AcademicYears",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AcademicYears", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsoCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "Tenants",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DatabaseConnection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ServerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenants", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "Regions",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CountryId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Regions", x => x.ID);
                table.ForeignKey(
                    name: "FK_Regions_Countries_CountryId",
                    column: x => x.CountryId,
                    principalTable: "Countries",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Faculties",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Faculties", x => x.ID);
                table.ForeignKey(
                    name: "FK_Faculties_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "MyAppUsers",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                IsDean = table.Column<bool>(type: "bit", nullable: false),
                FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                LockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MyAppUsers", x => x.ID);
                table.ForeignKey(
                    name: "FK_MyAppUsers_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Cities",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RegionId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cities", x => x.ID);
                table.ForeignKey(
                    name: "FK_Cities_Regions_RegionId",
                    column: x => x.RegionId,
                    principalTable: "Regions",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FacultyId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Departments", x => x.ID);
                table.ForeignKey(
                    name: "FK_Departments_Faculties_FacultyId",
                    column: x => x.FacultyId,
                    principalTable: "Faculties",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Departments_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "MyAuthenticationTokens",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                MyAppUserId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MyAuthenticationTokens", x => x.ID);
                table.ForeignKey(
                    name: "FK_MyAuthenticationTokens_MyAppUsers_MyAppUserId",
                    column: x => x.MyAppUserId,
                    principalTable: "MyAppUsers",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_MyAuthenticationTokens_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Professors",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Biography = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Professors", x => x.ID);
                table.ForeignKey(
                    name: "FK_Professors_MyAppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "MyAppUsers",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Professors_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Municipalities",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CityId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Municipalities", x => x.ID);
                table.ForeignKey(
                    name: "FK_Municipalities_Cities_CityId",
                    column: x => x.CityId,
                    principalTable: "Cities",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                ParentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                Gender = table.Column<int>(type: "int", nullable: false),
                CitizenshipId = table.Column<int>(type: "int", nullable: true),
                BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                BirthMunicipalityId = table.Column<int>(type: "int", nullable: true),
                BirthCountryID = table.Column<int>(type: "int", nullable: true),
                PermanentAddressStreet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PermanentMunicipalityId = table.Column<int>(type: "int", nullable: true),
                StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ContactMobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ContactPrivateEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                TenantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.ID);
                table.ForeignKey(
                    name: "FK_Students_Countries_BirthCountryID",
                    column: x => x.BirthCountryID,
                    principalTable: "Countries",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Students_Countries_CitizenshipId",
                    column: x => x.CitizenshipId,
                    principalTable: "Countries",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Students_Municipalities_BirthMunicipalityId",
                    column: x => x.BirthMunicipalityId,
                    principalTable: "Municipalities",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Students_Municipalities_PermanentMunicipalityId",
                    column: x => x.PermanentMunicipalityId,
                    principalTable: "Municipalities",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Students_MyAppUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "MyAppUsers",
                    principalColumn: "ID");
                table.ForeignKey(
                    name: "FK_Students_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalTable: "Tenants",
                    principalColumn: "ID");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Cities_RegionId",
            table: "Cities",
            column: "RegionId");

        migrationBuilder.CreateIndex(
            name: "IX_Departments_FacultyId",
            table: "Departments",
            column: "FacultyId");

        migrationBuilder.CreateIndex(
            name: "IX_Departments_TenantId",
            table: "Departments",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Faculties_TenantId",
            table: "Faculties",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Municipalities_CityId",
            table: "Municipalities",
            column: "CityId");

        migrationBuilder.CreateIndex(
            name: "IX_MyAppUsers_TenantId",
            table: "MyAppUsers",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_MyAuthenticationTokens_MyAppUserId",
            table: "MyAuthenticationTokens",
            column: "MyAppUserId");

        migrationBuilder.CreateIndex(
            name: "IX_MyAuthenticationTokens_TenantId",
            table: "MyAuthenticationTokens",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Professors_TenantId",
            table: "Professors",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Professors_UserId",
            table: "Professors",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Regions_CountryId",
            table: "Regions",
            column: "CountryId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_BirthCountryID",
            table: "Students",
            column: "BirthCountryID");

        migrationBuilder.CreateIndex(
            name: "IX_Students_BirthMunicipalityId",
            table: "Students",
            column: "BirthMunicipalityId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_CitizenshipId",
            table: "Students",
            column: "CitizenshipId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_PermanentMunicipalityId",
            table: "Students",
            column: "PermanentMunicipalityId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_TenantId",
            table: "Students",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_UserId",
            table: "Students",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AcademicYears");

        migrationBuilder.DropTable(
            name: "Departments");

        migrationBuilder.DropTable(
            name: "MyAuthenticationTokens");

        migrationBuilder.DropTable(
            name: "Professors");

        migrationBuilder.DropTable(
            name: "Students");

        migrationBuilder.DropTable(
            name: "Faculties");

        migrationBuilder.DropTable(
            name: "Municipalities");

        migrationBuilder.DropTable(
            name: "MyAppUsers");

        migrationBuilder.DropTable(
            name: "Cities");

        migrationBuilder.DropTable(
            name: "Tenants");

        migrationBuilder.DropTable(
            name: "Regions");

        migrationBuilder.DropTable(
            name: "Countries");
    }
}
