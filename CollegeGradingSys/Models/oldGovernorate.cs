using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public enum oldGovernorate
    {
      حضرموت,
        شبوة,
        عمران,
        مأرب,
        عدن,
       البيضاء,
        الحديدة,
        الجوف,
        المحويت,
        [Display(Name = "أمانة العاصمة")]
        أمانة_العاصمة,
        ذمار,
        حجة,
        إب,
        ريمة,
        صعدة,
        صنعاء,
        تعز,
        أبين,
        الضالع,
        المهرة,
        [Display(Name = "أرخيب سقطرى")]
        أرخيب_سقطرى,
        لحج          

    }
}
