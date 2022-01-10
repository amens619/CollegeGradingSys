using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class BatchIndexData
    {
        [Display(Name = "التخصص")]
        public int? SpecializationId { get; set; }          

        public ICollection<Specialization>  Specializations { get; set; }
        public ICollection<Batch> Batches { get; set; }
    }
}
