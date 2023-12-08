using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddSpecializationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "Id", "SpecializationNameAr", "SpecializationNameEn" },
                values: new object[,]
                {
                    { 2, "طب القلب", "Cardiology" },
                    { 3, "الأمراض الجلدية", "Dermatology" },
                    { 4, "طب العيون", "Ophthalmology" },
                    { 5, "طب الأطفال", "Pediatrics" },
                    { 6, "طب الأعصاب", "Neurology" },
                    { 7, "طب النفسيات", "Psychiatry" },
                    { 8, "طب النساء والتوليد", "Obstetrics and Gynecology" },
                    { 9, "جراحة عامة", "General Surgery" },
                    { 10, "الطب الباطني", "Internal Medicine" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Specialization",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
