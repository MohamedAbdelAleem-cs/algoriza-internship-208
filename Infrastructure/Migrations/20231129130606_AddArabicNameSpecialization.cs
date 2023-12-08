using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddArabicNameSpecialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecializationName",
                table: "Specialization",
                newName: "SpecializationNameEn");

            migrationBuilder.AddColumn<string>(
                name: "SpecializationNameAr",
                table: "Specialization",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecializationNameAr",
                table: "Specialization");

            migrationBuilder.RenameColumn(
                name: "SpecializationNameEn",
                table: "Specialization",
                newName: "SpecializationName");
        }
    }
}
