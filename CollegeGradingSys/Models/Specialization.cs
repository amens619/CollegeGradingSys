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
        public string SpecializationName { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
