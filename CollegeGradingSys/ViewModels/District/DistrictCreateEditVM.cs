using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.District
{
    public class DistrictCreateEditVM

    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المديرية مطلوب")]
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المحافظة")]
        public int GovernorateId { get; set; }

        public IList<SelectItemVM> Governorates { get; set; }
    }
}
