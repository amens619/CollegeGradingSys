using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IAcademicYearRepository _AcademicYearRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;

        public HomeController(IAcademicYearRepository AcademicYearRepository,
            ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository)
        {
            
            _AcademicYearRepository = AcademicYearRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
        }

        public async Task<IActionResult> Index()
        {
            
            var academicYear =await _AcademicYearRepository.GetCurrentYearAsync();
            var stPersonalData = _StPersonalDataRepository.List().Where(x => x.EnrollmentYear.Id == academicYear.Id).ToList();
            var homeIndexData = new HomeIndexData();
            if (academicYear != null)
            {
                homeIndexData.StudentsNO = stPersonalData.Count();
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
