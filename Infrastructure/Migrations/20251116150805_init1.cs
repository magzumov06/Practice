using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Specialties_SpecialtyId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Students_StudentId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_StudentId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_SpecialtyId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Enrollments");

            migrationBuilder.AddColumn<string>(
                name: "SpecialityLanguage",
                table: "Specialties",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialityLanguage",
                table: "Specialties");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Specialties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Enrollments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_StudentId",
                table: "Specialties",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SpecialtyId",
                table: "Enrollments",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Specialties_SpecialtyId",
                table: "Enrollments",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Students_StudentId",
                table: "Specialties",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
