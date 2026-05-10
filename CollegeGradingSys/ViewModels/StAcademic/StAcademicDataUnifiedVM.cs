using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class StAcademicDataUnifiedVM
    {
        // 1. رسائل النظام
        public string Message { get; set; }

        // 2. الفلترة والبحث
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string StNameSearch { get; set; }

        public int? BatchId { get; set; }
        public int? AcademicYearId { get; set; }

        public StStatus? StStatus { get; set; }
        public Term? Term { get; set; }
        public Level? Level { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType? StudyType { get; set; }

        // 3. القوائم المنسدلة (البديل السحري لـ ViewBag)
        public SelectList AcademicYearsList { get; set; }
        public SelectList BatchesList { get; set; }

        // 4. ضوابط النظام وحالته
        public bool IsSelectCurrentYear { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }
        public bool IsStEnrollmentClosed { get; set; }
        public bool IsCurrentYearClosed { get; set; }

        // 5. التقسيم والبيانات
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public PagedResult<StAcademicDataVM> pagedResult { get; set; }

        // القائمة الفعلية للعرض (نستخدم IList لكي تعمل حلقة for بنجاح عند ترحيل الطلاب)
        public IList<StAcademicDataVM> StAcademicDataVMs { get; set; } = new List<StAcademicDataVM>();
    }
}