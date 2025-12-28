using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAcademicYearService _academicYearService;
        private readonly IStPersonalDataService _stPersonalDataService;

        public HomeController(
            IAcademicYearService academicYearService,
            IStPersonalDataService stPersonalDataService)
        {
            _academicYearService = academicYearService;
            _stPersonalDataService = stPersonalDataService;
        }

        public async Task<IActionResult> Index()
        {
            var academicYear = await _academicYearService.GetCurrentYearAsync();
            var homeIndexData = new HomeIndexData();
            
            if (academicYear != null)
            {
                var stPersonalData = await _stPersonalDataService.GetByEnrollmentYearAsync(academicYear.Id);
                homeIndexData.StudentsNO = stPersonalData?.Count() ?? 0;
                homeIndexData.AcademicYearName = academicYear.AcademicYearName;                 
            }
            
            return View(homeIndexData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
