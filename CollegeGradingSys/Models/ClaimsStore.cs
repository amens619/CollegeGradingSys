using System.Collections.Generic;
using System.Security.Claims;

namespace CollegeGradingSys.Models // تأكد أن الـ namespace يطابق مشروعك
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            // --- 1. حسابات المستخدمين (الفهارس 0 إلى 4) ---
            new Claim("Create User", "إضافة"),
            new Claim("Edit User", "تعديل"),
            new Claim("Delete User", "حذف"),
            new Claim("Manage User Roles", "إضافة/حذف مسؤولية"),
            new Claim("Manage User Claims", "إضافة/حذف صلاحيات"),

            // --- 2. النسخ الاحتياطي (الفهارس 5 إلى 6) ---
            new Claim("Create Backup", "اجراء نسخة احتياطية جديدة"),
            new Claim("Delete Backup", "حذف نسخة احتياطية"),          

            // --- 3. الكليات (الفهارس 7 إلى 9) ---
            new Claim("Create College", "إضافة"),
            new Claim("Edit College", "تعديل"),
            new Claim("Delete College", "حذف"),

            // --- 4. الأقسام (الفهارس 10 إلى 12) ---
            new Claim("Create Department", "إضافة"),
            new Claim("Edit Department", "تعديل"),
            new Claim("Delete Department", "حذف"),

            // --- 5. التخصصات (الفهارس 13 إلى 15) ---
            new Claim("Create Specialization", "إضافة"),
            new Claim("Edit Specialization", "تعديل"),
            new Claim("Delete Specialization", "حذف"),

            // --- 6. الدول والجنسيات (الفهارس 16 إلى 18) ---
            new Claim("Create Nationality", "إضافة"),
            new Claim("Edit Nationality", "تعديل"),
            new Claim("Delete Nationality", "حذف"),

            // --- 7. المحافظات (الفهارس 19 إلى 21) ---
            new Claim("Create Governorate", "إضافة"),
            new Claim("Edit Governorate", "تعديل"),
            new Claim("Delete Governorate", "حذف"),

            // --- 8. العام الجامعي (الفهارس 22 إلى 25) ---
            new Claim("Create AcademicYear", "إضافة"),
            new Claim("Edit AcademicYear", "تعديل"),
            new Claim("Delete AcademicYear", "حذف"),
            new Claim("Details AcademicYear", "عرض تفاصيل"),

            // --- 9. الدفعات (الفهارس 26 إلى 28) ---
            new Claim("Create Batch", "إضافة"),
            new Claim("Edit Batch", "تعديل"),
            new Claim("Delete Batch", "حذف"),

            // --- 10. المواد (الفهارس 29 إلى 32) ---
            new Claim("Create Course", "إضافة"),
            new Claim("Edit Course", "تعديل"),
            new Claim("Delete Course", "حذف"),
            new Claim("Details Course", "عرض تفاصيل"),

            // ================= بداية الصلاحيات المضافة الجديدة =================

            // --- 11. إدارة بيانات الطلاب (الفهارس 33 إلى 36) ---
            new Claim("Create StPersonalData", "إضافة"),
            new Claim("Edit StPersonalData", "تعديل"),
            new Claim("Delete StPersonalData", "حذف"),
            new Claim("Details StPersonalData", "عرض تفاصيل"),

            // --- 12. تصدير التقارير والكشوفات (الفهارس 37 إلى 41) ---
            new Claim("Export SthighSchoolToExcel", "تصدير من واقع الثانوية"),
            new Claim("Export AcceptedStToExcel", "تصدير المقبولين"),
            new Claim("Export StsGradesToExcel", "تصدير بيان درجات"),
            new Claim("Export StAcademicDataToExcel", "تصدير كشف المقيدين"),
            new Claim("Export GraduateStToExcel", "تصدير كشف الخريجين"),

            // --- 13. السجل الأكاديمي والوثائق (الفهارس 42 إلى 49) ---
            new Claim("Add AcademicDataForAllSts", "تقييد جميع الطلاب"),
            new Claim("All StAcademicData", "عرض السجل بالكامل"),
            new Claim("Graduation Certificate", "شهادة التخرج"),
            new Claim("Graduation Statement", "افادة تخرج"),
            new Claim("Study Confirmation", "تأكيد الدراسة"),
            new Claim("Statement AfterComprehensive", "افادة بعد الشامل"),
            new Claim("Statement ThreeYears", "افادة ثلاثة سنوات"),
            new Claim("Edit StAcademicData", "تعديل السجل الاكاديمي"),

            // --- 14. درجات الطلاب (الفهرس 50) ---
            new Claim("All StCourseGrade", "إدارة وعرض الدرجات")
        };
    }
}