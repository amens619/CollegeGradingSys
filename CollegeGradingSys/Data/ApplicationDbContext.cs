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
        public DbSet<CollegeGradingSys.ViewModels.DistrictCityViewModel> DistrictCityViewModel { get; set; }
      
       
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
