using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.GeneralInfo
{
    public class GeneralInfoVM
    {       
          
            public int Id { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم المدينة أطول من 100 حرفًا.")]
            [Display(Name = "رئيس قسم شئون الطلاب")]
            public string StDepartmentHead { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم الرئيس أطول من 100 حرفًا.")]
            [Display(Name = "رئيس فرع الجامعة بحضرموت")]
            public string BranchHeadName { get; set; }

            
            [StringLength(100, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم الرئيس أطول من 100 حرفًا.")]
            [Display(Name = "مسار النسخ الاحتياطي")]
            public string BackupPath { get; set; }

            public string SystemLicenseKey { get; set; } = "";

    }
}
