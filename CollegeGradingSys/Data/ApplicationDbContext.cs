using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeGradingSys.ViewModels;

namespace CollegeGradingSys.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<College> College { get; set; }
        public DbSet<Department> Department { get; set; }

        public DbSet<Specialization> Specialization { get; set; }

        public DbSet<Nationality> Nationality { get; set; }

        public DbSet<Governorate> Governorate { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<StPersonalData>  StPersonalData { get; set; }
        public DbSet<StHighSchoolData> StHighSchoolData { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<StAcademicData> StAcademicData { get; set; }
        public DbSet<StudentBatch> StudentBatch { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseGrade> CourseGrade { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StPersonalData>()
                
                        .HasOne(m => m.Nationality)
                        .WithMany(t => t.NationalityStPersonalDatas)
                        .HasForeignKey(m => m.NationalityID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StPersonalData>()
                       .HasOne(m => m.Birthcountry)
                        .WithMany(t => t.BirthcountryStPersonalDatas)
                        .HasForeignKey(m => m.BirthcountryID)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
