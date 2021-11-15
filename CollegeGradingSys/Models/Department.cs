using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public int SpecializationId { get; set; }
        public ICollection<Specialization> Specializations { get; set; }

    }
}
