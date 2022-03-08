using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace CollegeGradingSys
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                 { new CultureInfo("ar")};
                options.DefaultRequestCulture = new RequestCulture("ar");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //=======================================
            services.AddMvc();

            //services.AddSingleton<ICollegeGradingSysRepository<College>, CollegeRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<Nationality>, NationalityRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<Governorate>, GovernorateRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<District>, DistrictRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository <City>, CityRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<Department>, DepartmentRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<Specialization>, SpecializationRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<StPersonalData>, StPersonalDataRepository>();
            // services.AddSingleton<ICollegeGradingSysRepository<AcademicYear>, AcademicYearRepository>();
            //services.AddSingleton<ICollegeGradingSysRepository<Batch>, BatchRepository>();

            //============================================================================================
            services.AddTransient<ICollegeGradingSysRepository<College>, CollegeDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Nationality>, NationalityDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Governorate>, GovernorateDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<District>, DistrictDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<City>, CityDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Department>, DepartmentDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Specialization>, SpecializationDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<StPersonalData>, StPersonalDataDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<AcademicYear>, AcademicYearDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Course>, CourseDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<CourseGrade>, CourseGradeDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<Batch>, BatchDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<StHighSchoolData>, StHighSchoolDataDbRepository>();
            services.AddTransient<ICollegeGradingSysRepository<StAcademicData>, StAcademicDataDbRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SqlCon")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddCloudscribePagination();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("ar");
            cultureInfo.NumberFormat.NumberGroupSeparator = ",";
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //====================================


            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
            //====================================
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
