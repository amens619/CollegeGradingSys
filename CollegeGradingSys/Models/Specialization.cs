using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public Department Department { get; set; }
    }
}
