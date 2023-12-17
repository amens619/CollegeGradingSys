using System.Collections.Generic;
using System.Security.Claims;

namespace CollegeGradingSys.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create User", "إضافة"),
            new Claim( "Edit User", "تعديل"),
            new Claim( "Delete User","حذف"),

            new Claim( "Manage User Roles","إضافة/حذف مسؤولية"),
            new Claim( "Manage User Claims","إضافة/حذف صلاحيات"),

            new Claim( "Create Backup","اجراء نسخة احتياطية جديدة"),
            new Claim( "Delete Backup","حذف نسخة احتياطية"),          

             new Claim("Create College", "إضافة"),
            new Claim( "Edit College", "تعديل"),
            new Claim( "Delete College","حذف"),

             new Claim("Create Department", "إضافة"),
            new Claim( "Edit Department", "تعديل"),
            new Claim( "Delete Department","حذف"),

             new Claim("Create Specialization", "إضافة"),
            new Claim( "Edit Specialization", "تعديل"),
            new Claim( "Delete Specialization","حذف"),

             new Claim("Create Nationality", "إضافة"),
            new Claim( "Edit Nationality", "تعديل"),
            new Claim( "Delete Nationality","حذف"),

             new Claim("Create Governorate", "إضافة"),
            new Claim( "Edit Governorate", "تعديل"),
            new Claim( "Delete Governorate","حذف"),

             new Claim("Create AcademicYear", "إضافة"),
            new Claim( "Edit AcademicYear", "تعديل"),
            new Claim( "Delete AcademicYear","حذف"),
            new Claim( "Details AcademicYear", "عرض تفاصيل"),

             new Claim("Create Batch", "إضافة"),
            new Claim( "Edit Batch", "تعديل"),
            new Claim( "Delete Batch","حذف"),


            new Claim("Create Course", "إضافة"),
            new Claim( "Edit Course", "تعديل"),
            new Claim( "Delete Course","حذف"),
            new Claim( "Details Course", "عرض تفاصيل"),


            new Claim("Create StPersonalData", "إضافة"),
            new Claim( "Edit StPersonalData", "تعديل"),
            new Claim( "Delete StPersonalData","حذف"),
            new Claim( "Details StPersonalData", "عرض تفاصيل"),

             new Claim( "All StAcademicData", "عرض جميع السجلات الاكادمية"),
            //new Claim("Create AdminDocsTemp", "إضافة ملف"),           
            //new Claim( "Edit AdminDocsTemp", "تعديل ملف"),
            //new Claim( "Download AdminDocsTemp", "تنزيل ملف"),
            //new Claim( "Delete AdminDocsTemp","حذف ملف"),             
            //new Claim( "Restore AdminDocsTemp","استعادة ملف محذوف"),
            //new Claim( "PermanentlyDelete AdminDocsTemp","حذف نهائي لملف"),

            // new Claim("Create GuidPolicies", "إضافة ملف"),
            //new Claim( "Edit GuidPolicies", "تعديل ملف"),
            //new Claim( "Download GuidPolicies", "تنزيل ملف"),
            //new Claim( "Delete GuidPolicies","حذف ملف"),            
            //new Claim( "Restore GuidPolicies","استعادة ملف محذوف"),
            //new Claim( "PermanentlyDelete GuidPolicies","حذف نهائي لملف"),

            //new Claim("Create OtherSource", "إضافة ملف"),            
            //new Claim( "Edit OtherSource", "تعديل ملف"),
            //new Claim( "Download OtherSource", "تنزيل ملف"),
            //new Claim( "Delete OtherSource","حذف ملف"),
            //new Claim( "Restore OtherSource","استعادة ملف محذوف"),
            //new Claim( "PermanentlyDelete OtherSource","حذف نهائي لملف"),

            //new Claim("Create ProjectSFiles", "إضافة ملف"),
            // new Claim( "Edit ProjectSFiles", "تعديل ملف"),
            //new Claim( "Download ProjectSFiles", "تنزيل ملف"),           
            //new Claim( "Delete ProjectSFiles","حذف ملف"),
            //new Claim( "Restore ProjectSFiles","استعادة ملف محذوف"),
            //new Claim( "PermanentlyDelete ProjectSFiles","حذف نهائي لملف "),

            //new Claim("Create StudyAndSurvey", "إضافة ملف"),
            //new Claim( "Edit StudyAndSurvey", "تعديل ملف"),
            //new Claim( "Download StudyAndSurvey", "تنزيل ملف"),           
            //new Claim( "Delete StudyAndSurvey","حذف ملف"),
            //new Claim( "Restore StudyAndSurvey","استعادة ملف محذوف"),
            //new Claim( "PermanentlyDelete StudyAndSurvey","حذف نهائي لملف"),

            //new Claim("Create PFClassification", "إضافة"),
            //new Claim( "Edit PFClassification", "تعديل"),
            //new Claim( "Delete PFClassification","حذف"),

            //new Claim("Create Department", "إضافة"),
            //new Claim( "Edit Department", "تعديل"),
            //new Claim( "Delete Department","حذف"),


        };
    }
}
