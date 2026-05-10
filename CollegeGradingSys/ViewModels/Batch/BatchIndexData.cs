using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Batch
{
    public class BatchIndexData
    {
        [Display(Name = "التخصص")]
        public int? SpecializationId { get; set; }          

        public ICollection<Models.Specialization>  Specializations { get; set; }
        public ICollection<Models.Batch> Batches { get; set; }

        public SelectList SpecializationsList { get; set; }
    }
}
