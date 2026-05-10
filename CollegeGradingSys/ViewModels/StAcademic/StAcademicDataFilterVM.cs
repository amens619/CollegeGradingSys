using CollegeGradingSys.Models.Enums;
using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class StAcademicDataFilterVM
    {
        public string Message { get; set; }
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }

        public bool IsSelectCurrentYear { get; set; }
        public bool IsCurrentYear { get; set; }

        public string StNameSearch { get; set; }
        public int? BatchId { get; set; }
        public int? AcademicYearId { get; set; }

        public StStatus? StStatus { get; set; }
        public Term? Term { get; set; }
        public StudyType? StudyType { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public List<StAcademicDataVM> Data { get; set; } = new();
    }


}
