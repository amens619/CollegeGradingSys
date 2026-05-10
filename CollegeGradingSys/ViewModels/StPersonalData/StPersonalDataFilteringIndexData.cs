using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.StPersonalData
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
        public int BatchId { get; set; }
        // خصائص البحث
        public int? AcademicYearId { get; set; }
        public int? SearchAcademicID { get; set; }      
        //===
        public string CurrentSort { get; set; }
        public string NameSortParm { get; set; }
        public string SexSortParm { get; set; }
        public string CurrentFilter { get; set; }
        public bool IsSelectCurrentYear { get; set; }
        public int? SelectedAcademicId { get; set; }

        [DisplayFormat(NullDisplayText = "لا توجد بيانات")]
        public Models.StHighSchoolData StHighSchoolData { get; set; }
        //===

        public ICollection<Models.Batch> Batches { get; set; }

        //public ICollection<StPersonalData>  StPersonalDatas { get; set; }     
       
        public PagedResult<Models.StPersonalData> pagedResult { get; set; }

        // القوائم المنسدلة التي كانت في ViewData
        public SelectList AcademicYearsList { get; set; }
        public SelectList BatchesList { get; set; }
    }
}
