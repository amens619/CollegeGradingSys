namespace CollegeGradingSys.ViewModels.StPersonalData
{
    public class StPersonalDataDto
    {
        public int AcademicID { get; set; }
        public string StName { get; set; }
        public string IdentificatioNO { get; set; }
        public string SexName { get; set; } // تحويل الـ Enum لنص
        public string NationalityName { get; set; }
        public string EnrollmentYearName { get; set; }
    }
}