using CollegeGradingSys.Models;
using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels.AcademicYear
{
    public class AcademicYearIndexVM
    {
        /// <summary>
        /// هل تم إغلاق العام الحالي أم لا
        /// (يُستخدم لتحديد السماح بإنشاء عام جديد)
        /// </summary>
        public bool IsCurrentYearClosed { get; set; }

        /// <summary>
        /// قائمة الأعوام الدراسية للعرض فقط
        /// </summary>
        public IReadOnlyList<Models.AcademicYear> AcademicYears { get; set; }
            = new List<Models.AcademicYear>();
    }
}
