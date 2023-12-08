using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<Specialization> specializations = new List<Specialization>
            {
                new Specialization { Id=2, SpecializationNameEn = "Cardiology", SpecializationNameAr = "طب القلب" },
                new Specialization { Id=3, SpecializationNameEn = "Dermatology", SpecializationNameAr = "الأمراض الجلدية" },
                new Specialization { Id=4, SpecializationNameEn = "Ophthalmology", SpecializationNameAr = "طب العيون" },
                new Specialization { Id=5, SpecializationNameEn = "Pediatrics", SpecializationNameAr = "طب الأطفال" },
                new Specialization { Id=6, SpecializationNameEn = "Neurology", SpecializationNameAr = "طب الأعصاب" },
                new Specialization { Id=7, SpecializationNameEn = "Psychiatry", SpecializationNameAr = "طب النفسيات" },
                new Specialization { Id=8, SpecializationNameEn = "Obstetrics and Gynecology", SpecializationNameAr = "طب النساء والتوليد" },
                new Specialization { Id=9, SpecializationNameEn = "General Surgery", SpecializationNameAr = "جراحة عامة" },
                new Specialization { Id=10, SpecializationNameEn = "Internal Medicine", SpecializationNameAr = "الطب الباطني" }
            };


            modelBuilder.Entity<Specialization>().HasData(specializations);

            //Identity Id for bookings
            modelBuilder.Entity<Bookings>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //Doctor,Specialization -> one to one
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationID);

            //Doctor,User -> One to one
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Doctor,Bookings -> one to many
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasMany(d => d.Bookings) 
                    .WithOne(b => b.doctor)     
                    .HasForeignKey(b => b.DoctorId) 
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);              
            });

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .IsRequired();


            modelBuilder.Entity<Times>()
                .HasOne(t => t.Appointment)
                .WithMany(a => a.Times)
                .HasForeignKey(t => t.AppointmentId)
                .IsRequired();

            modelBuilder.Entity<Discount>()
                .HasIndex(D => D.DiscountCode)
                .IsUnique();

        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Times> Times { get; set; }
        public DbSet<Discount> Discounts { get; set; }
    }
}
