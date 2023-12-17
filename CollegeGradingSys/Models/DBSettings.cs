using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class DBSettings
    {
        public int Id { get; set; }

        [Display(Name = "سجل النسخة الاحتياطية")]
        public string BackupName { get; set; }
    }
}
