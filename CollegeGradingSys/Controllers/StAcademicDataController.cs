using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.CourseGrade;
using CollegeGradingSys.ViewModels.StAcademic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class StAcademicDataController : Controller
    {
        private readonly IStAcademicDataService _service;
        private readonly IStPersonalDataService _stPersonalDataService;
        private readonly IExcelExportService _excelExportService;

        public StAcademicDataController(IStAcademicDataService service,
            IStPersonalDataService stPersonalDataService,
            IExcelExportService excelExportService)
        {
            _service = service;
            _stPersonalDataService = stPersonalDataService;
            _excelExportService = excelExportService;
        }

        public async Task<IActionResult> Index(StAcademicDataUnifiedVM filter, int pageNumber = 1, int pageSize = 10)
        {

            var model = await _service.GetIndexDataAsync(filter, pageNumber, pageSize);
           

            return View(model);
        }

        public async Task<IActionResult> StudentAcademicHistory(int academicID)
        {
            try
            {
                var model = await _service.GetStudentAcademicHistoryAsync(academicID);
                return View(model);
            }
            catch (DomainException ex)
            {
                return RedirectToAction(nameof(Index), new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var model = await _service.GetCreateFormAsync(id);
                return View(model);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStAcademicDataDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _service.FillListsForModelAsync(model);
                return View(model);
            }

            try
            {
                await _service.CreateAsync(model);
                return RedirectToAction(nameof(StudentAcademicHistory), new { academicID = model.AcademicID });
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await _service.FillListsForModelAsync(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _service.GetEditFormAsync(id);
                return View(model);
            }
            catch (DomainException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateStAcademicDataDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _service.FillListsForModelAsync(model);
                return View(model);
            }

            try
            {
                await _service.EditAsync(model);
                return RedirectToAction(nameof(StudentAcademicHistory), new { academicID = model.AcademicID });
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await _service.FillListsForModelAsync(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // قد تحتاج لتعديل بسيط هنا لتمكين إعادة التوجيه الصحيحة
                // عن طريق إرجاع الـ academicID من دالة الحذف
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<IActionResult> AddAcademicDataForAllSts()
        {
            //try
            //{
            //    await _service.PromoteAllStudentsToNextYearAsync();
            //    return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            //}
            //catch (DomainException ex)
            //{
            //    return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true, Message = ex.Message });
            //}

            try
            {
                // محاولة تنفيذ دالة الترحيل
                await _service.PromoteAllStudentsToNextYearAsync();

                // إذا نجحت الدالة مستقبلاً، يمكنك إضافة رسالة نجاح هنا
                TempData["SuccessMessage"] = "تم ترحيل الطلاب بنجاح!";
                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            }
            catch (NotImplementedException ex)
            {
                // 👈 هنا نصطاد الخطأ ونأخذ الرسالة ونضعها في TempData
                TempData["WarningMessage"] = ex.Message; // ستكون قيمتها: "يجب تطبيق منطق الترحيل هنا"
                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            }
            catch (Exception ex)
            {
                // 👈 اصطياد أي أخطاء أخرى غير متوقعة
                TempData["WarningMessage"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });
            }
        }


        //public async Task<IActionResult> AllStAcademicDatas(int id)
        //{



        //    var stPersonalData = await _stPersonalDataService.GetByIdAsync(id);

        //    IList<StAcademicData> stAcademicDatas = new List<StAcademicData>();
        //    if (id != null)
        //    {
        //        stAcademicDatas = _StAcademicDataRepository.List().Where(s => s.StPersonalData.AcademicID.Equals(id))
        //            .OrderBy(x => x.AcademicYear.AcademicYearStart.Year)
        //            .ThenBy(x => x.StLevel)
        //            .ThenBy(x => x.Term)
        //            .ToList();

        //    }
        //    var model = new StAcademicDataDataViewModel()
        //    {
        //        Id = id,
        //        AcademicID = stPersonalData.AcademicID,
        //        StName = stPersonalData.StName,
        //        IsCanRegisterInCurrentYear = false,
        //        StAcademicDatas = stAcademicDatas
        //    };
        //    var currentYear = await _AcademicYearRepository.GetCurrentYearAsync();
        //    var stACourseGrades = stAcademicDatas.Where(x => x.AcademicYear.Id == currentYear.Id).ToList();
        //    if (stACourseGrades == null || stACourseGrades.Count <= 0)
        //    {

        //        var isSTwithdrew = stAcademicDatas.Any(x => x.StStatus == StStatus.منحسب);
        //        model.IsCanRegisterInCurrentYear = !isSTwithdrew;

        //    }




        //    return View(model);
        //}


        // ================= Printing & Export =================
        public async Task<IActionResult> PrintConfEnroll(int id)
        {
            var model = await _service.GetPrintConfEnrollAsync(id);
            return View(model);
        }

        public async Task<IActionResult> PrintGradeReport(int id)
        {
            var model = await _service.GetPrintGradeReportAsync(id);
            return View(model);
        }

        //[Authorize(Policy = "ExportSthighSchoolToExcelPolicy")]
        //public async Task<ActionResult> ExportSthighSchoolToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        //{
        //    // 1. تحديد السنة الأكاديمية المطلوبة
        //    if (IsSelectCurrentYear)
        //    {
        //        var currentYear = await _academicYearService.GetCurrentYearAsync();
        //        if (currentYear != null)
        //        {
        //            AcademicYearId = currentYear.Id;
        //        }
        //    }

        //    // 2. جلب كل الطلاب مع بياناتهم المرتبطة لتجنب N+1 Queries
        //    var allStudents = await _stPersonalDataService.FindFullAsync();



        //    // 4. توليد التقرير من الخدمة مباشرة
        //    byte[] fileContents = await _excelExportService.GenerateStudentTranscriptExcelAsync(allStudents);

        //    // 5. إرجاع الملف
        //    return File(
        //        fileContents,
        //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        "بيان درجات من واقع شهادة الثانوية.xlsx"
        //    );
        //}
        // 2. الدالة المسؤولة عن التحميل
        [HttpGet]
        public async Task<IActionResult> DownloadTranscript(int id)
        {
            try
            {               
                // استدعاء الخدمة التي قمنا ببرمجتها مسبقاً لتوليد ملف الإكسل
                var excelData = await _excelExportService.GenerateStudentTranscriptExcelAsync(id);

                // التحقق من أن الخدمة أعادت بيانات ولم تفشل
                if (excelData == null || excelData.Length == 0)
                {
                    // في حال عدم وجود طالب أو خطأ ما، نعيده للصفحة مع رسالة خطأ
                    TempData["ErrorMessage"] = "عذراً، لم يتم العثور على بيانات أو درجات لهذا الطالب.";

                    // يمكنك تغيير "Index" إلى اسم الشاشة التي يعود إليها
                    return RedirectToAction(nameof(Index));
                }

                // تحديد اسم الملف عند التحميل (مثال: بيان_درجات_الطالب_12345.xlsx)
                string fileName = $"بيان_درجات_الطالب_{id}.xlsx";

                // الـ MIME Type الخاص بملفات إكسل الحديثة
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // إرجاع الملف للمتصفح ليبدأ التحميل تلقائياً
                return File(excelData, contentType, fileName);
            }
            catch (Exception ex)
            {
                // يفضل هنا تسجيل الخطأ في السيرفر (Logging) إن وجد
                TempData["ErrorMessage"] = "حدث خطأ غير متوقع أثناء توليد بيان الدرجات.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ExportStAcademicDataToExcel(StAcademicDataFilterVM filter)
        {
            var stream = await _service.ExportStAcademicDataToExcelAsync(filter);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "كشف المقيدين.xlsx");
        }

        public async Task<IActionResult> ExportGraduateStToExcel(StAcademicDataFilterVM filter)
        {
            var stream = await _service.ExportGraduateStToExcelAsync(filter);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "كشف الخريجين.xlsx");
        }

        public async Task<IActionResult> StudentGradesReport(int id, int? level, int? term, int? yearId)
        {
            // استدعاء الخدمة فقط لتجهيز كل شيء
            var viewModel = await _service.GetStudentGradesReportAsync(id, level, term, yearId);

            // إذا رجع null، يعني الطالب غير موجود
            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "عذراً، لم يتم العثور على بيانات هذا الطالب.";
                return RedirectToAction(nameof(Index)); // أو أي شاشة أخرى
            }

            return View(viewModel);
        }
    }
}