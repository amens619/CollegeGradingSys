using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StPersonalDataFilteringIndexData
    {
        [Display(Name = "رقم القيد")]
        public int? AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus? StStatus { get; set; }
        [Display(Name = "الدفعة")]
        public int StudentBatchId { get; set; }

        
        public ICollection<StudentBatch> StudentBatches { get; set; }

        //public ICollection<StPersonalData>  StPersonalDatas { get; set; }
        [DisplayFormat(NullDisplayText = "لا توجد بيانات")]
        public virtual StHighSchoolData StHighSchoolData { get; set; }
        public PagedResult<StPersonalData> pagedResult { get; set; }
    }
}
