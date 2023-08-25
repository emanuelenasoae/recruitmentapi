using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpenRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleTitle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Seniority = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RoleDescrption = table.Column<string>(type: "TEXT", nullable: true),
                    SalaryRange = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Recruiters",
                columns: table => new
                {
                    RecruiterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Seniority = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Skills = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiters", x => x.RecruiterId);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    YearsOfExperience = table.Column<byte>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_Candidates_OpenRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "OpenRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentProcesses",
                columns: table => new
                {
                    ProcessId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecruiterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Stage = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentProcesses", x => x.ProcessId);
                    table.ForeignKey(
                        name: "FK_RecruitmentProcesses_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecruitmentProcesses_Recruiters_RecruiterId",
                        column: x => x.RecruiterId,
                        principalTable: "Recruiters",
                        principalColumn: "RecruiterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RoleId",
                table: "Candidates",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentProcesses_CandidateId",
                table: "RecruitmentProcesses",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentProcesses_RecruiterId",
                table: "RecruitmentProcesses",
                column: "RecruiterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecruitmentProcesses");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Recruiters");

            migrationBuilder.DropTable(
                name: "OpenRoles");
        }
    }
}
