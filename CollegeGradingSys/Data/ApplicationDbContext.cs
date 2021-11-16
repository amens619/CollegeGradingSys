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

        public DbSet<Nationality> Nationality { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<CollegeGradingSys.ViewModels.CollegeDepartmentViewModel> CollegeDepartmentViewModel { get; set; }

        public DbSet<CollegeGradingSys.Models.Specialization> Specialization { get; set; }

        public DbSet<CollegeGradingSys.ViewModels.DepartmentSpecializationViewModel> DepartmentSpecializationViewModel { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
