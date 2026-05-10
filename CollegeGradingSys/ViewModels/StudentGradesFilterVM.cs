using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels
{
    public class StudentGradesFilterVM
    {
        // فلاتر البحث
        public int AcademicID { get; set; }
        public string StudentName { get; set; }
        public int? SelectedLevel { get; set; }
        public int? SelectedTerm { get; set; }
        public int? SelectedYearId { get; set; }

        // قوائم التصفية (للـ Dropdown)
        public List<SelectListItem> Levels { get; set; }
        public List<SelectListItem> Terms { get; set; }
        public List<SelectListItem> AcademicYears { get; set; }

        // النتائج
        public List<Models.CourseGrade> Grades { get; set; }
    }
}
