using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddBookingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID('Bookings', 'U') IS NOT NULL DROP TABLE Bookings;");

            migrationBuilder.CreateTable(
            name: "Bookings",
             columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        DoctorId = table.Column<int>(nullable: false),
        PatientId = table.Column<string>(maxLength: 450, nullable: false), // Match the length here
        Day = table.Column<int>(nullable: false),
        Status = table.Column<int>(nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Bookings", x => x.Id);
        table.ForeignKey(
            name: "FK_Bookings_Doctors_DoctorId",
            column: x => x.DoctorId,
            principalTable: "Doctors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_Bookings_AspNetUsers_PatientId",
            column: x => x.PatientId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Bookings_DoctorId",
            //    table: "Bookings",
            //    column: "DoctorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Bookings_PatientId",
            //    table: "Bookings",
            //    column: "PatientId");

            


            migrationBuilder.CreateTable(
                name: "Times",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Times", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Times_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_patientId",
                table: "Bookings",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Times_BookingId",
                table: "Times",
                column: "BookingId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Bookings_AspNetUsers_patientId",
            //    table: "Bookings",
            //    column: "patientId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Times");

            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
