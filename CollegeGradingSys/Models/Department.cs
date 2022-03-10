using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        
        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم القسم  من 3 - 30 حرفًا.")]
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }

        
        public virtual College College { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; }

    }
}
