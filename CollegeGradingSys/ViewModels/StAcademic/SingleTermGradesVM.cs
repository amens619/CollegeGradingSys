using System.Collections.Generic;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;

namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class SingleTermGradesVM
    {
        public int StAcademicDataId { get; set; }
        public int AcademicID { get; set; }
        public string StudentName { get; set; }

        // بيانات الفصل الحالي
        public Level Level { get; set; }
        public Term Term { get; set; }
        public string AcademicYearName { get; set; }

        // 💡 هل السنة الدراسية الحالية مفتوحة للتعديل؟
        public bool IsYearOpen { get; set; }

        public List<Models.CourseGrade> Grades { get; set; }
    }
}