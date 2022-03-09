using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public partial class College
    {
        [Key]
        public int Id { get; set; }

        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم الكلية  من 3 - 30 حرفًا.")]
        [Display(Name = "الكلية")]
        public string CollegeName { get; set; }

        public virtual ICollection<Department>  Departments { get; set; }
    }
}
