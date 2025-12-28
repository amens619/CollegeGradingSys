using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.Models
{
    /// <summary>
    /// Represents the academic year settings including start/end dates,
    /// titles in Gregorian and Hijri formats, and additional metadata.
    /// </summary>
    public class AcademicYear
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// تاريخ بداية العام الدراسي.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ بداية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearStart { get; set; }

        /// <summary>
        /// تاريخ نهاية العام الدراسي.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ نهاية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearEnd { get; set; }

        /// <summary>
        /// اسم العام الدراسي (ميلادي).
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "يجب أن يكون طول العام الدراسي من 3 - 30 حرفًا.")]
        [Display(Name = "العام الدراسي")]
        public required string AcademicYearName { get; set; }


        /// <summary>
        /// اسم العام الدراسي بالهجري.
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "يجب أن يكون طول العام الدراسي بالهجري من 3 - 30 حرفًا.")]
        [Display(Name = "العام الدراسي بالهجري")]
        public required string AcademicYearNameH { get; set; }

        /// <summary>
        /// هل هذا العام الدراسي هو الحالي؟
        /// </summary>
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }
        public bool IsClosed { get; private set; }
        /// <summary>
        /// ملاحظات إضافية عن العام الدراسي.
        /// </summary>
        [MaxLength(200)]
        [Display(Name = "ملاحظة")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string? Note { get; set; }

        /// <summary>
        /// بيانات الطلاب المرتبطة بهذا العام الدراسي.
        /// </summary>
        public virtual ICollection<StAcademicData>? StAcademicDatas { get; set; } = new List<StAcademicData>();

        // ===== Domain Behavior =====
        public void Close()
        {
            if (IsClosed)
                throw new InvalidOperationException("العام الدراسي مغلق بالفعل.");

            IsClosed = true;
            IsCurrentYear = false;
        }

        public void ReOpen()
        {
            if (!IsClosed)
                throw new InvalidOperationException("العام الجامعي ليس مغلق");

            IsClosed = false;
        }
    }
}

