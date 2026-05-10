using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.AcademicYear;
using CollegeGradingSys.ViewModels.StPersonalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class StPersonalDataController : Controller
    {
        private readonly IStPersonalDataService _studentService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IGovernorateService _governorateService;
        private readonly INationalityService _nationalityService;
        private readonly IBatchService _batchService;
        private readonly IStHighSchoolDataService _highSchoolService;
        private readonly IExcelExportService _excelExportService;

        public StPersonalDataController(
              IStPersonalDataService studentService,
              IAcademicYearService academicYearService,
              IGovernorateService governorateService,
              INationalityService nationalityService,
              IBatchService batchService,
              IStHighSchoolDataService highSchoolService,
              IExcelExportService excelExportService)
        {
            _studentService = studentService;
            _academicYearService = academicYearService;
            _governorateService = governorateService;
            _nationalityService = nationalityService;
            _batchService = batchService;
            _highSchoolService = highSchoolService;
            _excelExportService = excelExportService;
        }
        // GET: StPersonalDataController
        //public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, string BatchName, int? id,  int? AcademicYearId, StStatus? StStatus, int? SearchAcademicID, int pageNumber = 1, int pageSize = 10,bool IsSelectCurrentYear = false)
        //{
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? AcademicYearId, int? SearchAcademicID, int? id, int pageNumber = 1, int pageSize = 10, bool IsSelectCurrentYear = false)
        {     
            // 1. تحديد الفلترة والبحث
            if (searchString != null) pageNumber = 1;
            else searchString = currentFilter;

            var allStudents = await _studentService.GetAllFullAsync();
            var studentsQuery = allStudents.AsQueryable();

            if (IsSelectCurrentYear)
            {
                var currentYear = await _academicYearService.GetCurrentYearAsync();
                if (currentYear != null) AcademicYearId = currentYear.Id;
            }
          
            if (AcademicYearId != null)
                studentsQuery = studentsQuery.Where(x => x.EnrollmentYear.Id == AcademicYearId);

            if (!string.IsNullOrEmpty(searchString))
                studentsQuery = studentsQuery.Where(s => s.StName.Contains(searchString));

            if (SearchAcademicID != null)
                studentsQuery = studentsQuery.Where(s => s.AcademicID == SearchAcademicID);

           



            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";





            //ViewBag.CurrentFilter = searchString;

            // 2. تطبيق الترتيب
            switch (sortOrder)
            {
                case "name_desc": studentsQuery = studentsQuery.OrderByDescending(s => s.StName); break;
                case "SexSortParm": studentsQuery = studentsQuery.OrderBy(s => s.Sex); break;
                case "SexSortParm_desc": studentsQuery = studentsQuery.OrderByDescending(s => s.Sex); break;
                default: studentsQuery = studentsQuery.OrderBy(s => s.StName); break;
            }

            // 4. تجميع كل البيانات في الـ ViewModel وإرسالها
            var model = new StPersonalDataFilteringIndexData
            {               
                CurrentSort = sortOrder,
                NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm",
                CurrentFilter = searchString,
                IsSelectCurrentYear = IsSelectCurrentYear,
                AcademicYearId = AcademicYearId,
                SearchAcademicID = SearchAcademicID,
                BatchId = -1,
                SelectedAcademicId = id,

                // تعبئة القوائم المنسدلة مباشرة في الـ VM
                AcademicYearsList = new SelectList(await FillSelectAcademicYearesList("-- الكل --"), "Id", "Name", AcademicYearId ?? -1),
                BatchesList = new SelectList(await FillSelectBatchsList("-- الكل --"), "Id", "BatchName", -1)
            };
          
            if (id != null && id > 0)
            {
                model.StHighSchoolData = await _highSchoolService.GetByIdAsync(id.Value);
            }
            model.pagedResult = new PagedResult<StPersonalData>
            {
                Data = studentsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                TotalItems = studentsQuery.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(model);
         
        }

      
        // GET: StPersonalDataController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var stPersonalData = await _studentService.GetFullAsync(id);
            if (stPersonalData == null) return NotFound();
            return View(stPersonalData);
        }
       
        // GET: StPersonalDataController/Create
        [Authorize(Policy = "CreateStPersonalDataPolicy")]
        public async Task<ActionResult> Create()
        {
            
            var academicCurrentYear = await _academicYearService.GetCurrentYearAsync();
            if (academicCurrentYear == null)
            {
                // استخدام TempData للرسائل العابرة بدلاً من ViewBag
                TempData["ErrorMessage"] = "الرجاء ادخال السنة الاكاديمية أولاً";
                return RedirectToAction(nameof(Index));
            }

            var model = new StPersonalDataViewModel
            {
                EnrollmentYearId = academicCurrentYear.Id,
                EnrollmentYearM = academicCurrentYear.AcademicYearName,
                EnrollmentYearH = academicCurrentYear.AcademicYearNameH,
                BirthDate = new DateTime(2000, 1, 1),
            };

            await PopulateViewModelDropdowns(model);
            return PartialView("_Create", model);
        }


        // POST: StPersonalDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateStPersonalDataPolicy")]
        public async Task<ActionResult> Create(StPersonalDataViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Create", await PopulateViewModelDropdowns(model));
           

            int parsedAcademicId = int.Parse(model.AcademicID);
            if (!await _studentService.IsAcademicIdAvailableAsync(parsedAcademicId))
            {
                ModelState.AddModelError(nameof(model.AcademicID), "لقد تم إيجاد رقم قيد سابق بنفس الرقم .. الرجاء كتابة رقم آخر");
                return View(await PopulateViewModelDropdowns(model));
            }

            try
            {
                var governorate = await _governorateService.GetByIdAsync(model.GovernorateId);
                var nationality = await _nationalityService.GetByIdAsync(model.NationalityId);
                var birthPlace = await _nationalityService.GetByIdAsync(model.BirthPlaceId);
                var enrollmentYear = await _academicYearService.GetByIdAsync(model.EnrollmentYearId);

                var stPersonalData = new StPersonalData
                {                   
                    AcademicID = parsedAcademicId,
                    StName = model.StName,
                    IdentificatioNO = model.IdentificatioNO,
                    Sex = model.Sex,
                    BirthDate = model.BirthDate,
                    Birthcountry = birthPlace,
                    EnrollmentYear = enrollmentYear,
                    Nationality = nationality,
                    BirthGovernorate = governorate,
                    StHighSchoolData = model.StHighSchoolData
                };

                await _studentService.RegisterNewStudentAsync(stPersonalData, model.BatchId);

                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "حدث خطأ غير متوقع: " + ex.Message);
                return View(await PopulateViewModelDropdowns(model));
            }
        }       

        // GET: StPersonalDataController/Edit/5
        [Authorize(Policy = "EditStPersonalDataPolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            var stPersonalData = await _studentService.GetFullAsync(id);
            if (stPersonalData == null) return NotFound();

            var model = new StPersonalDataViewModel
            {
                AcademicID = stPersonalData.AcademicID.ToString(),
                StName = stPersonalData.StName,
                IdentificatioNO = stPersonalData.IdentificatioNO,
                Sex = stPersonalData.Sex,
                BirthDate = stPersonalData.BirthDate,
                BirthPlaceId = stPersonalData.Birthcountry?.Id ?? 0,
                EnrollmentYearH = stPersonalData.EnrollmentYear?.AcademicYearNameH,
                EnrollmentYearM = stPersonalData.EnrollmentYear?.AcademicYearName,
                NationalityId = stPersonalData.Nationality?.Id ?? 0,
                GovernorateId = stPersonalData.BirthGovernorate?.Id ?? 0,
            };

            await PopulateViewModelDropdowns(model);
            return View(model);
        }

       
        // POST: StPersonalDataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditStPersonalDataPolicy")]
        public async Task<ActionResult> Edit(int id, StPersonalDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await PopulateViewModelDropdowns(model));
            }

            try
            {
                var stPersonalData = await _studentService.GetFullAsync(id);
                if (stPersonalData == null) return NotFound();

                stPersonalData.StName = model.StName;
                stPersonalData.IdentificatioNO = model.IdentificatioNO;
                stPersonalData.Sex = model.Sex;
                stPersonalData.BirthDate = model.BirthDate;
                stPersonalData.Birthcountry = await _nationalityService.GetByIdAsync(model.BirthPlaceId);
                stPersonalData.Nationality = await _nationalityService.GetByIdAsync(model.NationalityId);
                stPersonalData.BirthGovernorate = await _governorateService.GetByIdAsync(model.GovernorateId);

                await _studentService.UpdateAsync(stPersonalData);

                return RedirectToAction(nameof(Details), new { IsSelectCurrentYear = true, id = stPersonalData.AcademicID });
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: StPersonalDataController/Delete/5       
        [Authorize(Policy = "DeleteStPersonalDataPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            var stPersonalData = await _studentService.GetFullAsync(id);
            if (stPersonalData == null) return NotFound();

            // تحقق واحد فقط، واستخدام TempData لإرسال التحذير للشاشة
            if (stPersonalData.StAcademicDatas != null && stPersonalData.StAcademicDatas.Any())
            {
                TempData["WarningMessage"] = "يوجد سجلات أكاديمية تابعة للطالب.. سيتم حذف الطالب وجميع بياناته ودرجاته نهائياً.";
            }
            
            return PartialView("_Delete", stPersonalData);
        }

        // POST: StPersonalDataController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteStPersonalDataPolicy")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _studentService.DeleteAsync(id);
                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            }
            catch
            {
                return View();
            }
        }

       



        [Authorize(Policy = "ExportAcceptedStToExcelPolicy")]
        public async Task<ActionResult> ExportAcceptedStToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        {
            AcademicYear selectedYear = null;

            // 1. تحديد العام الدراسي المطلوب
            if (IsSelectCurrentYear)
            {
                selectedYear = await _academicYearService.GetCurrentYearAsync();
                if (selectedYear != null)
                {
                    AcademicYearId = selectedYear.Id;
                }
            }
            else if (AcademicYearId.HasValue)
            {
                selectedYear = await _academicYearService.GetByIdAsync(AcademicYearId.Value);
            }

            // 2. جلب قائمة الطلاب مجهزة بالكامل بالبيانات المرتبطة
            var allStudents = await _studentService.GetAllFullAsync();

            // فلترة الطلاب حسب العام الدراسي إذا تم تحديده
            if (AcademicYearId.HasValue)
            {
                allStudents = allStudents.Where(x => x.EnrollmentYear?.Id == AcademicYearId.Value).ToList();
            }

            // 3. إرسال البيانات للـ Service ليقوم بتوليد الإكسيل المعقد
            byte[] fileContents = _excelExportService.GenerateAcceptedStudentsExcel(allStudents, selectedYear);

            // 4. إرجاع الملف النهائي للمستخدم
            return File(
                fileContents,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "المقبولين.xlsx"
            );
        }
        
        //==============================================
        [Authorize(Policy = "ExportSthighSchoolToExcelPolicy")]
        public async Task<ActionResult> ExportSthighSchoolToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        {
            // 1. تحديد السنة الأكاديمية المطلوبة
            if (IsSelectCurrentYear)
            {
                var currentYear = await _academicYearService.GetCurrentYearAsync();
                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;
                }
            }

            // 2. جلب كل الطلاب مع بياناتهم المرتبطة لتجنب N+1 Queries
            var allStudents = await _studentService.GetAllFullAsync();

            // 3. فلترة البيانات
            if (AcademicYearId.HasValue)
            {
                allStudents = allStudents
                    .Where(x => x.EnrollmentYear?.Id == AcademicYearId.Value)
                    .ToList();
            }

            // 4. توليد التقرير من الخدمة مباشرة
            byte[] fileContents = _excelExportService.GenerateHighSchoolDataExcel(allStudents);

            // 5. إرجاع الملف
            return File(
                fileContents,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "بيان درجات من واقع شهادة الثانوية.xlsx"
            );
        }
        //[Authorize(Policy = "ExportSthighSchoolToExcelPolicy")]
        //public async Task<ActionResult> ExportSthighSchoolToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        //{
        //    using (var xlPackage = new ExcelPackage(stream))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add(" كشف الطلاب");
        //        worksheet.View.RightToLeft = true;
        //        worksheet.Cells.Style.Font.Bold = true;
        //        var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
        //        namedStyle.Style.Font.UnderLine = true;
        //        namedStyle.Style.Font.Color.SetColor(Color.Blue);
        //        const int startRow = 2;                
        //        var row = startRow;

        //        worksheet.Column(1).Width = 6;
        //        worksheet.Column(2).Width = 46.57;
        //        worksheet.Column(3).Width = 20;
        //        worksheet.Column(4).Width = 20;
        //        worksheet.Column(5).Width = 20;
        //        worksheet.Column(6).Width = 20;
        //        worksheet.Column(7).Width = 20;
        //        worksheet.Column(8).Width = 20;
        //        worksheet.Column(9).Width = 20;
        //        worksheet.Column(10).Width = 20;
        //        worksheet.Column(11).Width = 20;
        //        worksheet.Column(12).Width = 31.71;



        //        //==========================
        //        worksheet.Row(1).Height = 58.75;
        //        worksheet.Row(2).Height = 80;





        //        //Create Headers and format them
        //        worksheet.Cells["A1:L1"].Value = "بيانات طلاب حضرموت دفعة 1439هـ من واقع شهادة الثانوية";
        //        using (var r = worksheet.Cells["A1:L1"])
        //        {
        //            r.Merge = true;
        //            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
        //            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
        //            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
        //            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //            r.Style.Font.Size = 36;
        //            r.Style.Font.Name = "Calibri";

        //        }

        //        //=============================

        //        worksheet.Cells["A2:L2"].Style.Font.Size = 24;

        //        //=============================


        //        //=====================================
        //        //////var academicYear = GetCurrentYear();
        //        //////worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
        //        Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
        //        Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
        //        Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");

        //        //================================

        //        int rowIndex = 1;
        //        int colIndex = 7;
        //        int PixelTop = 17;
        //        int PixelLeft = 1721;
        //        int Height = 153;
        //        int Width = 100;

        //        using (var r = worksheet.Cells["A2:L2"])
        //        {
        //            r.Style.WrapText = true;
        //            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
        //            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(Color.White);

        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Top.Color.SetColor(Color.Black);

        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Left.Color.SetColor(Color.Black);

        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Right.Color.SetColor(Color.Black);

        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //        }
        //        row = startRow;
        //        //================================
        //        worksheet.Cells["A2"].Value = "م";
        //        //================================
        //        worksheet.Cells["B2"].Value = "الاسم";
        //        //================================
        //        worksheet.Cells["C2"].Value = "محل الميلاد";
        //        //================================
        //        worksheet.Cells["D2"].Value = "تاريخ الميلاد";
        //        //================================
        //        worksheet.Cells["E2"].Value = "الجنسية";
        //        //================================
        //        worksheet.Cells["F2"].Value = "نوع الشهادة";
        //        //================================
        //        worksheet.Cells["G2"].Value = "المعدل";
        //        //================================
        //        worksheet.Cells["H2"].Value = "مصدرها";
        //        //================================
        //        worksheet.Cells["I2"].Value = "رقم الجلوس";
        //        //================================
        //        worksheet.Cells["J2"].Value = "تاريخ الالتحاق هـ";
        //        //================================
        //        worksheet.Cells["K2"].Value = "تاريخ الالتحاق م";
        //        //================================
        //        worksheet.Cells["L2"].Value = "ملاحظة";
        //        //================================              


        //        var no = 1;
        //        foreach (var OneSt in StPersonalDatasR)
        //        {
        //            row++;
        //            worksheet.Row(row).Height = 50;
        //            worksheet.Cells[row, 1].Value = no;

        //            //=================================================
        //            worksheet.Cells[row, 2].Value = OneSt.StName;

        //            //================================================
        //            worksheet.Cells[row, 3].Value = OneSt.Birthcountry.CountryName;

        //            //================================================
        //            worksheet.Cells[row, 4].Value = OneSt.BirthDate.ToShortDateString();

        //            //================================================

        //            worksheet.Cells[row, 5].Value = OneSt.Nationality.NationalityName;
        //            if (OneSt.StHighSchoolData is not null)
        //            {
        //                //================================================
        //                worksheet.Cells[row, 6].Value= OneSt.StHighSchoolData.CertificateType.ToString() ;

        //                //================================================
        //                worksheet.Cells[row, 7].Value =  OneSt.StHighSchoolData.Average.ToString() ;

        //                //================================================
        //                worksheet.Cells[row, 8].Value = (OneSt.StHighSchoolData.Source is not null) ? OneSt.StHighSchoolData.Source : "";

        //                //================================================
        //                worksheet.Cells[row, 9].Value =  OneSt.StHighSchoolData.SeatNo.ToString() ;

        //                //================================================
        //                worksheet.Cells[row, 12].Value = (OneSt.StHighSchoolData.Note is not null) ? OneSt.StHighSchoolData.Note:"";

        //            }
        //            //================================================
        //            worksheet.Cells[row, 10].Value = OneSt.EnrollmentYear.AcademicYearNameH;

        //            //================================================
        //            worksheet.Cells[row, 11].Value = OneSt.EnrollmentYear.AcademicYearName;



        //            //================================================


        //            using (var r = worksheet.Cells[row,1, row, 12])
        //            {
        //                r.Style.WrapText = true;
        //                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
        //                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                r.Style.Fill.BackgroundColor.SetColor(Color.White);

        //                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                r.Style.Border.Top.Color.SetColor(Color.Black);

        //                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                r.Style.Border.Left.Color.SetColor(Color.Black);

        //                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                r.Style.Border.Right.Color.SetColor(Color.Black);

        //                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //                r.Style.Font.Size = 18;
        //            }


        //            no++;
        //        }

        //            //================================================


        //        // set some core property values
        //        xlPackage.Workbook.Properties.Title = "User List";
        //        xlPackage.Workbook.Properties.Author = "";
        //        xlPackage.Workbook.Properties.Subject = "User List";
        //        // save the new spreadsheet
        //        xlPackage.Save();
        //        // Response.Clear();
        //    }
        //    stream.Position = 0;
        //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "بيان درجات من واقع شهادة الثانوية.xlsx");

        //}

        //List<Governorate> FillSelectGovernoratesList()
        //{
        //    var Governorates = GovernorateRepository.List().ToList();
        //    Governorates.Insert(0, new Governorate { Id = -1, GovernorateName = "-- أختر --" });

        //    return Governorates;
        //}



        //List<Batch> FillSelectBatchsList(string studentBatchName)
        //{
        //    var Batchs = BatchRepository.List().OrderByDescending(x => x.Id).ToList();
        //    Batchs.Insert(0, new Batch { Id = -1, BatchName = studentBatchName });

        //    return Batchs;
        //}




        //List<Nationality> FillSelectNationalitiesList()
        //{
        //    var Nationalities = NationalityRepository.List().ToList();
        //    Nationalities.Insert(0, new Nationality { Id = -1, NationalityName = "-- أختر --" , CountryName = "-- أختر --" });

        //    return Nationalities;
        //}
        //StPersonalDataViewModel GetAllStPersonalDatas(StPersonalDataViewModel  Model)
        //{
        //    Model.Governorates = FillSelectGovernoratesList();
        //    Model.Nationalities = FillSelectNationalitiesList();            
        //    return Model;
        //}
        //public JsonResult GetGovernorate(int Id)
        //{
        //    var GovernoratesList = GovernorateRepository.List().Where(a => a.Nationality.Id == Id);
        //    return Json(new SelectList(GovernoratesList, "Id", "GovernorateName"));
        //}

        //public async Task<List<AcademicYearSelectItemVM>> FillSelectAcademicYearesList(string placeholder)
        //{
        //    var years = (await AcademicYearRepository.ListAsync())
        //        .Select(x => new AcademicYearSelectItemVM
        //        {
        //            Id = x.Id,
        //            Name = x.AcademicYearName
        //        })
        //        .ToList();

        //    // Insert placeholder option
        //    years.Insert(0, new AcademicYearSelectItemVM
        //    {
        //        Id = -1,
        //        Name = placeholder
        //    });

        //    return years;
        //}


        //private bool IsAcademicIDExists(int academicID)
        //{
        //    return StPersonalDataRepository.List().Any(e => e.AcademicID == academicID);
        //}

        //private void AddCourseGradeTostAcademicData(StAcademicData stAcademicData)
        // {
        //     var stAcCourses = CourseRepository.List().Where(x => x.Level == stAcademicData.StLevel).Where(x => x.Term == stAcademicData.Term).Where(x => x.Specialization == stAcademicData.Batch.Specialization).ToList();
        //     foreach (var course in stAcCourses)
        //     {

        //         var courseGrade = new CourseGrade()
        //         {
        //             Course = course,
        //             CourseType = true,
        //             StAcademicData = stAcademicData,
        //             StStatusForCourse = StStatusForCourse.غير_محدد,
        //         };
        //         CourseGradeRepository.Add(courseGrade);
        //     }
        // }

        //private async Task<AcademicYear> GetCurrentYear()
        //{
        //    var currentYear =await AcademicYearRepository.GetCurrentYearAsync();
        //    return (currentYear);
        //}

        private async Task<StPersonalDataViewModel> PopulateViewModelDropdowns(StPersonalDataViewModel model)
        {
            model.Governorates = await FillSelectGovernoratesList();
            model.Nationalities = await FillSelectNationalitiesList();

            // تعبئة قائمة الدفعات داخل الـ VM
            model.BatchesList = new SelectList(await FillSelectBatchsList("-- أختر --"), "Id", "BatchName");
            return model;
        }

        private async Task<List<Governorate>> FillSelectGovernoratesList()
        {
            var list = (await _governorateService.GetAllAsync()).ToList();
            list.Insert(0, new Governorate { Id = -1, GovernorateName = "-- أختر --" });
            return list;
        }

        private async Task<List<Batch>> FillSelectBatchsList(string placeholder)
        {
            var list = (await _batchService.GetAllAsync()).OrderByDescending(x => x.Id).ToList();
            list.Insert(0, new Batch { Id = -1, BatchName = placeholder });
            return list;
        }

        private async Task<List<Nationality>> FillSelectNationalitiesList()
        {
            var list = (await _nationalityService.GetAllAsync()).ToList();
            list.Insert(0, new Nationality { Id = -1, NationalityName = "-- أختر --", CountryName = "-- أختر --" });
            return list;
        }

        public async Task<List<AcademicYearSelectItemVM>> FillSelectAcademicYearesList(string placeholder)
        {
            var years = (await _academicYearService.GetAllAsync())
                .Select(x => new AcademicYearSelectItemVM { Id = x.Id, Name = x.AcademicYearName })
                .ToList();
            years.Insert(0, new AcademicYearSelectItemVM { Id = -1, Name = placeholder });
            return years;
        }

        [HttpGet]
        public async Task<JsonResult> GetGovernorate(int Id)
        {
            var list = (await _governorateService.GetAllAsync()).Where(a => a.Nationality?.Id == Id);
            return Json(new SelectList(list, "Id", "GovernorateName"));
        }
    }
}
