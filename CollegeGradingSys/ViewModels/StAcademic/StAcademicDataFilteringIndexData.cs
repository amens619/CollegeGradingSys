using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class StAcademicDataFilteringIndexData
    {
        [Display(Name = "رقم القيد")]
        public int? AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus? StStatus { get; set; }
        [Display(Name = "الدفعة")]
        public int BatchId { get; set; }

        
        public ICollection<Models.Batch> Batches { get; set; }

        //public ICollection<StPersonalData>  StPersonalDatas { get; set; }
        [DisplayFormat(NullDisplayText = "لا توجد بيانات")]
        public virtual Models.StHighSchoolData StHighSchoolData { get; set; }
        public PagedResult<StAcademicData> pagedResult { get; set; }
    }
}
