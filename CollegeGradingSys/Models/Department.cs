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

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }

        public int CollegeId { get; set; }
        public College College { get; set; }

        public ICollection<Specialization> Specializations { get; set; }

    }
}
