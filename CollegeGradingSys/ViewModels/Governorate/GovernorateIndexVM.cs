using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Governorate
{
    public class GovernorateIndexVM
    {
        public int Id { get; set; }

        [Display(Name = "المحافظة/المنطقة")]
        public string GovernorateName { get; set; }

        [Display(Name = "الدولة")]
        public string NationalityName { get; set; } // لعرض اسم الدولة التابعة لها
    }
}
