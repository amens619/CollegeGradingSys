using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateUserPolicy",
                   policy => policy.RequireClaim("Create User"));

                options.AddPolicy("DeleteUserPolicy",
                    policy => policy.RequireClaim("Delete User"));

                options.AddPolicy("EditUserPolicy",
                   policy => policy.RequireClaim("Edit User"));

                options.AddPolicy("ManageUserRolesPolicy",
                   policy => policy.RequireClaim("Manage User Roles"));

                options.AddPolicy("ManageUserClaimsPolicy",
                   policy => policy.RequireClaim("Manage User Claims"));

                options.AddPolicy("CreateBackupPolicy",
                  policy => policy.RequireClaim("Create Backup"));

                options.AddPolicy("DeleteBackupPolicy",
                   policy => policy.RequireClaim("Delete Backup"));

                options.AddPolicy("CreateCollegePolicy",
                  policy => policy.RequireClaim("Create College"));

                options.AddPolicy("DeleteCollegePolicy",
                    policy => policy.RequireClaim("Delete College"));

                options.AddPolicy("EditCollegePolicy",
                   policy => policy.RequireClaim("Edit College"));


                options.AddPolicy("CreateDepartmentPolicy",
                policy => policy.RequireClaim("Create Department"));

                options.AddPolicy("DeleteDepartmentPolicy",
                    policy => policy.RequireClaim("Delete Department"));

                options.AddPolicy("EditDepartmentPolicy",
                   policy => policy.RequireClaim("Edit Department"));


                options.AddPolicy("CreateSpecializationPolicy",
               policy => policy.RequireClaim("Create Specialization"));

                options.AddPolicy("DeleteSpecializationPolicy",
                    policy => policy.RequireClaim("Delete Specialization"));

                options.AddPolicy("EditSpecializationPolicy",
                   policy => policy.RequireClaim("Edit Specialization"));

                options.AddPolicy("CreateNationalityPolicy",
             policy => policy.RequireClaim("Create Nationality"));

                options.AddPolicy("DeleteNationalityPolicy",
                    policy => policy.RequireClaim("Delete Nationality"));

                options.AddPolicy("EditNationalityPolicy",
                   policy => policy.RequireClaim("Edit Nationality"));


                options.AddPolicy("CreateGovernoratePolicy",
                    policy => policy.RequireClaim("Create Governorate"));

                options.AddPolicy("DeleteGovernoratePolicy",
                    policy => policy.RequireClaim("Delete Governorate"));

                options.AddPolicy("EditGovernoratePolicy",
                   policy => policy.RequireClaim("Edit Governorate"));


                options.AddPolicy("CreateAcademicYearPolicy",
                   policy => policy.RequireClaim("Create AcademicYear"));

                options.AddPolicy("DeleteAcademicYearPolicy",
                    policy => policy.RequireClaim("Delete AcademicYear"));

                options.AddPolicy("EditAcademicYearPolicy",
                   policy => policy.RequireClaim("Edit AcademicYear"));

                options.AddPolicy("DetailsAcademicYearPolicy",
                   policy => policy.RequireClaim("Details AcademicYear"));

                options.AddPolicy("CreateBatchPolicy",
               policy => policy.RequireClaim("Create Batch"));

                options.AddPolicy("DeleteBatchPolicy",
                    policy => policy.RequireClaim("Delete Batch"));

                options.AddPolicy("EditBatchPolicy",
                   policy => policy.RequireClaim("Edit Batch"));
                
              options.AddPolicy("CreateCoursePolicy",
               policy => policy.RequireClaim("Create Course"));

                options.AddPolicy("DeleteCoursePolicy",
                    policy => policy.RequireClaim("Delete Course"));

                options.AddPolicy("EditCoursePolicy",
                   policy => policy.RequireClaim("Edit Course"));

                options.AddPolicy("DetailsCoursePolicy",
                  policy => policy.RequireClaim("Details Course"));


                options.AddPolicy("CreateStPersonalDataPolicy",
                 policy => policy.RequireClaim("Create StPersonalData"));               

                options.AddPolicy("EditStPersonalDataPolicy",
                policy => policy.RequireClaim("Edit StPersonalData"));

                options.AddPolicy("DetailsStPersonalDataPolicy",
                policy => policy.RequireClaim("Details StPersonalData"));                

                options.AddPolicy("DeleteStPersonalDataPolicy",
                policy => policy.RequireClaim("Delete StPersonalData"));

                options.AddPolicy("ExportSthighSchoolToExcelPolicy",
               policy => policy.RequireClaim("Export SthighSchoolToExcel"));
                
                options.AddPolicy("ExportAcceptedStToExcelPolicy",
               policy => policy.RequireClaim("Export AcceptedStToExcel"));



                
                options.AddPolicy("ExportStsGradesToExcelPolicy",
              policy => policy.RequireClaim("Export StsGradesToExcel"));

                options.AddPolicy("ExportStAcademicDataToExcelPolicy",
            policy => policy.RequireClaim("Export StAcademicDataToExcel"));

                options.AddPolicy("ExportGraduateStToExcelPolicy",
            policy => policy.RequireClaim("Export GraduateStToExcel"));

                options.AddPolicy("AddAcademicDataForAllStsPolicy",
             policy => policy.RequireClaim("Add AcademicDataForAllSts"));

                options.AddPolicy("AllStAcademicDatasPolicy",
               policy => policy.RequireClaim("All StAcademicData"));

              options.AddPolicy("GraduationCertificatePolicy",
               policy => policy.RequireClaim("Graduation Certificate"));

                options.AddPolicy("GraduationStatementPolicy",
               policy => policy.RequireClaim("Graduation Statement"));

                options.AddPolicy("StudyConfirmationPolicy",
               policy => policy.RequireClaim("Study Confirmation"));

            options.AddPolicy("StatementAfterComprehensivePolicy",
               policy => policy.RequireClaim("Statement AfterComprehensive"));

            options.AddPolicy("StatementThreeYearsPolicy",
             policy => policy.RequireClaim("Statement ThreeYears"));

                options.AddPolicy("EditStAcademicDataPolicy",
                         policy => policy.RequireClaim("Edit StAcademicData"));




                options.AddPolicy("AllStCourseGradePolicy",
         policy => policy.RequireClaim("All StCourseGrade"));
            });
            //==========================================
            //services.AddMvc();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                // 2. 🛑 الفلتر الجديد: حارس بوابة الترخيص (Machine ID)
                // تأكد من كتابة مسار المجلد الصحيح الذي وضعت فيه الكلاس
                options.Filters.Add<CollegeGradingSys.Filters.LicenseCheckFilter>();
            });
            //===============================
            services.AddRazorPages()
            .AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "The field is required.");
            });
            services.AddIdentity<IdentityUser, IdentityRole>(

               options => {
                   options.SignIn.RequireConfirmedEmail = false;
                    //options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 5;
                   //options.Password.RequireUppercase = false;
                   //options.Password.RequireLowercase = false;
                   //options.Password.RequireDigit = false;
                  
               })

               .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.AddMvcCore().AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(s => "The provided value is invalid.");
            });
            //============================================================================================           
            services.AddScoped<IRepository<College>, CollegeDbRepository>();
            services.AddScoped<ICollegeService, CollegeService>();
            services.AddScoped<IRepository<Nationality>, NationalityDbRepository>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IRepository<Governorate>, GovernorateDbRepository>();
            services.AddScoped<IGovernorateService, GovernorateService>();
            services.AddScoped<IRepository<District>, DistrictDbRepository>();
            //services.AddScoped<IGovernorateService, GovernorateService>();
            services.AddScoped<IRepository<City>, CityDbRepository>();
      




            services.AddScoped<IRepository<DBSettings>, DBSettingsDbRepository>();

            services.AddScoped<IRepository<Department>, DepartmentDbRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IRepository<Specialization>, SpecializationDbRepository>();
            services.AddScoped<IRepository<Course>, CourseDbRepository>();
            services.AddScoped<IRepository<Batch>, BatchDbRepository>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<ISpecializationService, SpecializationService>();

            //services.AddScoped<IRepository<StAcademicData>, StAcademicDataDbRepository>();
            //services.AddScoped<IStAcademicDataService, StAcademicDataService>();           

            services.AddScoped<IStPersonalDataRepository, StPersonalDataDbRepository>();
            services.AddScoped<IStPersonalDataService, StPersonalDataService>();


            services.AddScoped<IAcademicYearRepository, AcademicYearDbRepository>();
            services.AddScoped<IAcademicYearService, AcademicYearService>();

            services.AddScoped<ICourseRepository, CourseDbRepository>();
            services.AddScoped<ICourseService, CourseService>();

            //services.AddScoped<IRepository<CourseGrade>, CourseGradeDbRepository>();
            services.AddScoped<ICourseGradeRepository, CourseGradeDbRepository>();
            services.AddScoped<ICourseGradeService, CourseGradeService>();

           

            services.AddScoped<IStHighSchoolDataRepository, StHighSchoolDataDbRepository>();
            services.AddScoped<IStHighSchoolDataService, StHighSchoolDataService>();

            services.AddScoped<IStAcademicDataRepository, StAcademicDataDbRepository>();
            services.AddScoped<IStAcademicDataService, StAcademicDataService>();
            services.AddScoped<IExcelExportService, ExcelExportService>();




            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));


            services.AddScoped<IRepository<GeneralInfo>, GeneralInfoDbRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SqlCon")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddCloudscribePagination();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2); // مدة حفظ الفلاتر
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
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
            app.UseSession();
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
