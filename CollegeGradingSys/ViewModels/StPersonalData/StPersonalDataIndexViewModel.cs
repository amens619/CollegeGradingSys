using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels.StPersonalData
{   
    public class StPersonalDataIndexViewModel
    {
        public IEnumerable<StPersonalDataDto> Students { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public int? AcademicYearId { get; set; }
        public bool IsSelectCurrentYear { get; set; }
        public IEnumerable<SelectListItem> AcademicYearsList { get; set; }
        public IEnumerable<SelectListItem> BatchesList { get; set; }
    }
}
