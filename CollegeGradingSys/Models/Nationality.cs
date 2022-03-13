using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Nationality
    {
        [Key]
        public int Id { get; set; }

       
        [StringLength(60, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم الكلية  من 3 - 60 حرفًا.")]
        [Display(Name = "الدولة")]
        public string CountryName { get; set; }

        
        [StringLength(60, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم الكلية  من 3 - 60 حرفًا.")]
        [Display(Name = "الجنسية")]
        public string NationalityName { get; set; }

        public virtual ICollection<Governorate>  Governorates { get; set; }
        public virtual ICollection<StPersonalData> NationalityStPersonalDatas { get; set; }
        public virtual ICollection<StPersonalData> BirthcountryStPersonalDatas { get; set; }

    }
}
