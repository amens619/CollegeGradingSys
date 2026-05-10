using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.CourseGrade
{
    public class CourseGradeVM
    {
        public int Id { get; set; }

        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "اسم الطالب")]
        public string StName { get; set; }

        [Display(Name = "الدرجة")]
        [DisplayFormat(NullDisplayText = "-")] // لعرض شرطة بدلاً من فراغ إذا لم توجد درجة
        public float? Grade { get; set; }

        [Display(Name = "الحالة")]
        public StStatusForCourse StStatusForCourse { get; set; }

        // يستخدم داخلياً لتحديد ما إذا تم تعديل الدرجة (مثلاً عند الرفع من إكسل)
        public bool IsGradeChange { get; set; }

        [Display(Name = "ملاحظات")]
        public string Note { get; set; }

        public bool IsCurrentYear { get; set; }
        // =========================================================
        // ملاحظة: في Clean Architecture الصارم، نفضل عدم تمرير الـ Entity (Models.Course)
        // ولكن أبقيناه هنا لأن منطق الـ Service والـ Excel Upload الذي كتبناه سابقاً
        // يعتمد عليه للوصول للـ BigGrade و SmallGrade أثناء التحقق.
        // =========================================================
        public Models.Course Course { get; set; }
    }
}