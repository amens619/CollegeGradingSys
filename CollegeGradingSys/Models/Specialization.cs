using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }

        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم التخصص  من 3 - 30 حرفًا.")]
        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Course> Courses  { get; set; }
        public virtual ICollection<Batch>  Batches { get; set; }
    }
}
