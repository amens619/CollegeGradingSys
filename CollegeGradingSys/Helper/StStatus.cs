using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public enum StStatus
    {
        مقيد,
        ناجح,
        راسب,
        منحسب,
        [Display(Name = "موقف قيد")]
        موقف_قيد,
        متخرج
    }
}
