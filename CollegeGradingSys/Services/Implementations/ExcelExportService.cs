using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    
    public class ExcelExportService : IExcelExportService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStPersonalDataRepository _studentRepo;
        public ExcelExportService(IWebHostEnvironment env, IStPersonalDataRepository studentRepo)
        {
            _env = env;
            _studentRepo = studentRepo;
        }
        public byte[] GenerateAcceptedStudentsExcel(IList<StPersonalData> students, AcademicYear academicYear)
        {
            // إنشاء الـ Excel Package بالذاكرة
            using (var xlPackage = new ExcelPackage())
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("المقبولين");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;

                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 8;
                var row = startRow;
                // ...
                worksheet.Column(1).Width = 4.71;
                worksheet.Column(2).Width = 38.29;
                worksheet.Column(3).Width = 6.71;
                worksheet.Column(4).Width = 9.29;
                worksheet.Column(5).Width = 17.43;
                worksheet.Column(6).Width = 17.71;
                worksheet.Column(7).Width = 23.86;
                worksheet.Column(8).Width = 21.14;
                worksheet.Column(9).Width = 11;
                worksheet.Column(10).Width = 11.43;
                worksheet.Column(11).Width = 14.57;
                worksheet.Column(12).Width = 11;
                worksheet.Column(13).Width = 11.71;
                worksheet.Column(14).Width = 12.57;
                worksheet.Column(15).Width = 14.14;
                worksheet.Column(16).Width = 24.57;
                //==========================
                worksheet.Row(1).Height = 25;
                worksheet.Row(2).Height = 25;
                worksheet.Row(3).Height = 25;
                worksheet.Row(4).Height = 25;
                worksheet.Row(5).Height = 30;
                worksheet.Row(6).Height = 30;
                worksheet.Row(7).Height = 80;



                //Create Headers and format them
                worksheet.Cells["B1"].Value = "الجمهورية اليمنية";
                worksheet.Cells["B2"].Value = "وزارة التعليم العالي والبحث العلمي";
                worksheet.Cells["B3"].Value = "قطاع الشؤون التعليمية";
                //=============================
                worksheet.Cells["A4:P7"].Style.Font.Size = 18;
                //=============================

                using (var r = worksheet.Cells["E2:H2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //============================

                worksheet.Cells["E2"].Value = "كشف الطلاب المقبولين للعام الدراسي:";
                worksheet.Cells["I2"].Value = academicYear != null ? academicYear.AcademicYearName : "";
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");
                using (var r = worksheet.Cells["I2:J2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["K2"].Value = "برنامج الدراسة";
                using (var r = worksheet.Cells["K2:L2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //===============================================
                worksheet.Cells["K3"].Value = "برنامج الدراسة";
                using (var r = worksheet.Cells["K3:L3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //===============================================
                using (var r = worksheet.Cells["M2:N3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["A4"].Value = "اسم الجامعة";
                using (var r = worksheet.Cells["A4:D4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["E4"].Value = "للعام الدراسي";
                using (var r = worksheet.Cells["E4:J4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["K4"].Value = "Name Of University";
                using (var r = worksheet.Cells["K4:p4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["A5"].Value = "جامعة الإيمان فرع حضرموت";
                using (var r = worksheet.Cells["A5:D5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
               
                     worksheet.Cells["E5"].Value = academicYear != null ? academicYear.AcademicYearName + "  " + academicYear.AcademicYearNameH :"";

                using (var r = worksheet.Cells["E5:J5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["K5"].Value = "";
                using (var r = worksheet.Cells["K5:p5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }

                //================================
                worksheet.Cells["A6"].Value = "م";
                using (var r = worksheet.Cells["A6:A7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["B6"].Value = "اسم الطالب";
                using (var r = worksheet.Cells["B6:B7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["C6"].Value = "الجنس";
                using (var r = worksheet.Cells["C6:C7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.TextRotation = 90;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["D6"].Value = "الجنسية";
                using (var r = worksheet.Cells["D6:D7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["E6"].Value = "محل للميلاد";
                using (var r = worksheet.Cells["E6:E7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["F6"].Value = "تاريخ الميلاد";
                using (var r = worksheet.Cells["F6:F7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["G6"].Value = "الرقم الوطني (الهوية)";
                using (var r = worksheet.Cells["G6:G7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["H6"].Value = "رقم القيد";
                using (var r = worksheet.Cells["H6:H7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["I6"].Value = "الكلية";
                using (var r = worksheet.Cells["I6:I7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["J6"].Value = "القسم";
                using (var r = worksheet.Cells["J6:J7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["K6"].Value = "التخصص";
                using (var r = worksheet.Cells["K6:K7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["L6"].Value = "نظام الدراسة";
                using (var r = worksheet.Cells["L6:L7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["M6"].Value = "بيانات المؤهل السابق";
                using (var r = worksheet.Cells["M6:P6"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["M7"].Value = "نوعه";

                worksheet.Cells["M7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["M7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["M7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["M7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["M7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["M7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["M7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["M7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["M7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["M7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                //================================
                worksheet.Cells["N7"].Value = "تاريخ الحصول عليه";
                worksheet.Cells["N7"].Style.WrapText = true;
                worksheet.Cells["N7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["N7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["N7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["N7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["N7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["N7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["N7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["N7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["N7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["N7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);


                //================================
                worksheet.Cells["O7"].Value = "جهة الحصول عليه";
                worksheet.Cells["O7"].Style.WrapText = true;
                worksheet.Cells["O7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["O7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["O7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["O7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["O7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["O7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["O7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["O7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["O7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                //================================
                worksheet.Cells["P7"].Value = "النسبة المئوية %";

                worksheet.Cells["P7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["P7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["P7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["P7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["P7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["P7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["P7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["P7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["P7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["P7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                              
                var no = 1;

                foreach (var StPersonalData in students)
                {
                    // 💡 التحسين: جلب السجل الأكاديمي الأول من الكائن نفسه الموجود في الذاكرة دون استدعاء قاعدة البيانات!
                    var StAcademicData = StPersonalData.StAcademicDatas?.FirstOrDefault();

                    worksheet.Cells[row, 1].Value = no;
                    worksheet.Cells[row, 2].Value = StPersonalData.StName;
                    worksheet.Cells[row, 3].Value = StPersonalData.Sex;
                    worksheet.Cells[row, 4].Value = StPersonalData.Nationality?.CountryName;
                    worksheet.Cells[row, 5].Value = StPersonalData.BirthGovernorate?.GovernorateName;
                    worksheet.Cells[row, 6].Value = StPersonalData.BirthDate.Date.ToString("d");
                    worksheet.Cells[row, 7].Value = StPersonalData.IdentificatioNO;
                    worksheet.Cells[row, 8].Value = StPersonalData.AcademicID;

                    if (StAcademicData != null && StAcademicData.Batch != null)
                    {
                        worksheet.Cells[row, 9].Value = StAcademicData.Batch.Specialization.Department.College.CollegeName;
                        worksheet.Cells[row, 10].Value = StAcademicData.Batch.Specialization?.Department?.DepartmentName;
                        worksheet.Cells[row, 11].Value = StAcademicData.Batch.Specialization?.SpecializationName;
                        worksheet.Cells[row, 12].Value = StAcademicData.StudyType;
                    }

                    if (StPersonalData.StHighSchoolData != null)
                    {

                        worksheet.Cells[row, 13].Value = StPersonalData.StHighSchoolData.CertificateType.ToString();
                        worksheet.Cells[row, 14].Value = StPersonalData.StHighSchoolData.CertificateYear;
                        worksheet.Cells[row, 15].Value = StPersonalData.StHighSchoolData.Source;
                        worksheet.Cells[row, 16].Value = StPersonalData.StHighSchoolData.Average;
                    }

                    // التنسيق لكل صف
                    string modelRange = "A" + row.ToString() + ":P" + row.ToString();
                    var modelTable = worksheet.Cells[modelRange];
                    worksheet.Row(row).Height = 25;
                    modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    modelTable.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // حدود الخلايا
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Color.SetColor(Color.Black);
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Color.SetColor(Color.Black);

                    row++;
                    no++;
                }

                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "Ameen Bashaaib";
                xlPackage.Workbook.Properties.Subject = "User List";
                // تحويل الملف إلى Byte Array مباشرة بدلاً من استخدام MemoryStream معقد
                return xlPackage.GetAsByteArray();
            }
        }

        public byte[] GenerateHighSchoolDataExcel(IList<StPersonalData> students)
        {
            using (var xlPackage = new ExcelPackage())
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("كشف الطلاب");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;

                // 1. تحديد عرض الأعمدة
                double[] colWidths = { 6, 46.57, 20, 20, 20, 20, 20, 20, 20, 20, 20, 31.71 };
                for (int i = 0; i < colWidths.Length; i++) worksheet.Column(i + 1).Width = colWidths[i];

                // ارتفاع الصفوف الأولى
                worksheet.Row(1).Height = 58.75;
                worksheet.Row(2).Height = 80;

                // 2. تنسيق العنوان الرئيسي (A1:L1)
                using (var r = worksheet.Cells["A1:L1"])
                {
                    r.Merge = true;
                    r.Value = "بيانات طلاب حضرموت دفعة 1439هـ من واقع شهادة الثانوية";
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; // حسب كودك الأصلي
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Font.Size = 36;
                    r.Style.Font.Name = "Calibri";
                }

                // 3. كتابة وتنسيق صف العناوين (A2:L2) دفعة واحدة
                string[] headers = { "م", "الاسم", "محل الميلاد", "تاريخ الميلاد", "الجنسية", "نوع الشهادة", "المعدل", "مصدرها", "رقم الجلوس", "تاريخ الالتحاق هـ", "تاريخ الالتحاق م", "ملاحظة" };
                for (int i = 0; i < headers.Length; i++) worksheet.Cells[2, i + 1].Value = headers[i];

                using (var r = worksheet.Cells["A2:L2"])
                {
                    r.Style.Font.Size = 24;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin; // خطوط داخلية
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // 4. السحر: تحضير البيانات وصبها في الإكسيل بدون Foreach
                var exportData = students.Select((st, index) => new {
                    No = index + 1,
                    Name = st.StName,
                    BirthPlace = st.Birthcountry?.CountryName ?? "",
                    BirthDate = st.BirthDate.ToString("d"),
                    Nationality = st.Nationality?.NationalityName ?? "",
                    CertType = st.StHighSchoolData?.CertificateType.ToString() ?? "",
                    Average = st.StHighSchoolData?.Average.ToString() ?? "",
                    Source = st.StHighSchoolData?.Source ?? "",
                    SeatNo = st.StHighSchoolData?.SeatNo.ToString() ?? "",
                    EnrollH = st.EnrollmentYear?.AcademicYearNameH ?? "",
                    EnrollM = st.EnrollmentYear?.AcademicYearName ?? "",
                    Note = st.StHighSchoolData?.Note ?? ""
                }).ToList();

                if (exportData.Any())
                {
                    // صب البيانات ابتداءً من الخلية A3
                    worksheet.Cells["A3"].LoadFromCollection(exportData, false);

                    // 5. التنسيق الجماعي للبيانات التي طبعناها!
                    int lastRow = 2 + exportData.Count;
                    using (var dataRange = worksheet.Cells[3, 1, lastRow, 12])
                    {
                        dataRange.Style.Font.Size = 18;
                        dataRange.Style.WrapText = true;
                        dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        dataRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // رسم حدود لجميع الخلايا في النطاق
                        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Top.Color.SetColor(Color.Black);
                        dataRange.Style.Border.Bottom.Color.SetColor(Color.Black);
                        dataRange.Style.Border.Left.Color.SetColor(Color.Black);
                        dataRange.Style.Border.Right.Color.SetColor(Color.Black);
                    }

                    // تعديل ارتفاع صفوف البيانات
                    for (int r = 3; r <= lastRow; r++) worksheet.Row(r).Height = 50;
                }

                return xlPackage.GetAsByteArray();
            }
        }

        public async Task<byte[]> GenerateStudentTranscriptExcelAsync(int AcademicID)
        {

            var student = await _studentRepo.FindFullAsync(AcademicID);          

            if (student == null) return null;
            var StAcademicData = student.StAcademicDatas?
                .Where(x => x.StStatus == StStatus.متخرج  || x.StStatus == StStatus.ناجح ).LastOrDefault();
            string templatePath = Path.Combine(_env.WebRootPath, "Templates", "AcademicTranscript.xlsx");
            FileInfo templateFile = new FileInfo(templatePath);

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(templateFile))
            {
                // 1. تغيير اسم الشيت برمجياً إلى "بيان درجات" كما طلبت
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(); // نأخذ أول شيت
                worksheet.Name = "بيان درجات";

                // 2. تعبئة بيانات الترويسة (نفس الكود السابق)
                worksheet.Cells[2, 2].Value = student.StName;
                worksheet.Cells[2, 12].Value = student.AcademicID;                
                worksheet.Cells[2, 18].Value = student.StAcademicDatas.FirstOrDefault()?.Batch?.Specialization?.Department?.College?.CollegeName;
                worksheet.Cells[2, 22].Value = student.Nationality?.NationalityName;
                worksheet.Cells[2, 27].Value = StAcademicData?.Average;               
                worksheet.Cells[2, 32].Value = StAcademicData?.GPA;
                worksheet.Cells[2, 37].Value = StAcademicData?.Valuation;
               


                // 3. 🛑 منطق طباعة الدرجات أو إغلاقها بالعلامة "#"
                // سنمر على المستويات من 1 إلى 4 (أو حسب نظام كليتكم)
                for (int level = 1; level <= 4; level++)
                {
                    int startColumn = GetStartColumnForLevelAndSemester(level);
                    int startRow = 7;
                    
                    for (int semester = 1; semester <= 2; semester++)
                    {
                        //// جلب السجل الأكاديمي الذي يطابق المستوى والفصل الحالي في الحلقة (Loop)
                        //// نستخدم (int) لتحويل الـ enum إلى رقم لمطابقته مع عداد الـ for loop
                        //var periodGrades = (student.StAcademicDatas ?? new List<StAcademicData>())
                        //    .Where(a => (int)a.StLevel == level-1 && (int)a.Term == semester-1 && a.CourseGrades != null)
                        //    .SelectMany(a => a.CourseGrades)
                        //    .ToList();



                        //if (periodGrades.Any())
                        //{
                        //    // 1. جلب كائن السنة الأكاديمية مرة واحدة فقط لزيادة الأداء
                        //    var academicYearInfo = periodGrades.FirstOrDefault()?.StAcademicData?.AcademicYear;

                        //    // 2. استخدام String Interpolation مع التحقق من الـ Null
                        //    var academicYear = academicYearInfo != null
                        //        ? $"{academicYearInfo.AcademicYearNameH}/{academicYearInfo.AcademicYearName}"
                        //        : ""; // إذا كانت البيانات غير موجودة، سيترك الخلية فارغة بدلاً من إيقاف النظام

                        //    worksheet.Cells[4, startColumn +1].Value = periodGrades.FirstOrDefault()?.StAcademicData?.StLevel;
                        //    worksheet.Cells[4, startColumn + 5].Value = academicYear;


                        //    // أ. إذا وجدت درجات: اطبعها بشكل طبيعي
                        //    foreach (var grade in periodGrades)
                        //    {
                        //     if (grade.Course.IsSubCourse == true) continue; // لتجنب  الكورس  فرعي 
                        //        worksheet.Cells[startRow, startColumn].Value = grade.Course?.CourseName;
                        //        worksheet.Cells[startRow, startColumn+5].Value = grade.Course?.Hours; // (تأكد أن اسم الخاصية في موديل Course هو Credits أو Hours)

                        //        // 💡 التعديلات الجديدة بناءً على الموديل:
                        //        worksheet.Cells[startRow, startColumn+6].Value = grade.Grade; // استخدام Grade بدلاً من FinalMark
                        //        worksheet.Cells[startRow, startColumn+7].Value = grade.GradeLetter; // استخدام التقدير الحرفي
                        //        startRow++;
                        //    }
                        //}

                        // جلب السجل الأكاديمي الذي يطابق المستوى والفصل الحالي في الحلقة (Loop)
                        // واستبعاد الكورسات الفرعية (IsSubCourse) مبكراً لتخفيف البيانات
                        var periodGrades = (student.StAcademicDatas ?? new List<StAcademicData>())
                            .Where(a => (int)a.StLevel == level - 1 && (int)a.Term == semester - 1 && a.CourseGrades != null)
                            .SelectMany(a => a.CourseGrades)
                            .Where(g => g.Course != null && g.Course.IsSubCourse != true)
                            .ToList();

                        if (periodGrades.Any())
                        {
                            // 1. جلب كائن السنة الأكاديمية
                            var academicYearInfo = periodGrades.FirstOrDefault()?.StAcademicData?.AcademicYear;
                            var academicYear = academicYearInfo != null
                                ? $"{academicYearInfo.AcademicYearNameH}/{academicYearInfo.AcademicYearName}"
                                : "";

                            worksheet.Cells[4, startColumn + 1].Value = periodGrades.FirstOrDefault()?.StAcademicData?.StLevel;
                            worksheet.Cells[4, startColumn + 5].Value = academicYear;

                            // 🛑 التعديل الجوهري: تصفية الدرجات المكررة (الدور الأول والتكميلي)
                            var finalGrades = periodGrades
                                .GroupBy(g => g.Course.Id) // نجمع الدرجات حسب معرف المادة (تأكد أن الخاصية اسمها CourseId)
                                .Select(group =>
                                {
                                    // إذا كان هناك أكثر من درجة لنفس المادة، نختار الدرجة الأكبر (التي تمثل التكميلي غالباً لأنها ستلغي الرسوب)
                                    // (ملاحظة: إذا كنت تريد جلب أحدث درجة أُدخلت بدلاً من الأكبر، استخدم OrderByDescending(x => x.Id) بدلاً من Grade)
                                    return group.OrderByDescending(g => g.Grade).FirstOrDefault();
                                })
                                .ToList();

                            // طباعة الدرجات المصفاة النهائية (درجة واحدة فقط لكل مادة)
                            foreach (var grade in finalGrades)
                            {
                                worksheet.Cells[startRow, startColumn].Value = grade.Course?.CourseName;
                                worksheet.Cells[startRow, startColumn + 5].Value = grade.Course?.Hours;
                                worksheet.Cells[startRow, startColumn + 6].Value = grade.Grade;
                                worksheet.Cells[startRow, startColumn + 7].Value = grade.GradeLetter;
                                startRow++;
                            }
                        }

                    }
                }

                package.Workbook.Calculate();
                return package.GetAsByteArray();
            }
        }

        // دالة مساعدة لتحديد أماكن الجداول في ملف الإكسل (يجب تعديل الأرقام حسب ملفك)
        private int GetStartColumnForLevelAndSemester(int level)
        {
            // مثال توضيحي:
            // المستوى 1 الفصل 1 يبدأ صف 10
            // المستوى 1 الفصل 2 يبدأ صف 20... وهكذا
            if (level == 1 ) return 3;
            if (level == 2 ) return 13;
            if (level == 3 ) return 23;
            if (level == 4) return 33;
            // استكمل بقية المستويات بناءً على تصميم ملف "بيان درجات جديد معدل.xlsx"
            return 10;
        }
    }
}
