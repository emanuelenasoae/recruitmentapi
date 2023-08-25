using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.Migrations
{
    /// <inheritdoc />
    public partial class addIsActiveCandidateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Candidates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Candidates");
        }
    }
}
