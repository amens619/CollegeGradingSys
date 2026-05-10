using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.AcademicYear;
using CollegeGradingSys.ViewModels.Batch;
using CollegeGradingSys.ViewModels.CourseGrade;
using CollegeGradingSys.ViewModels.StAcademic;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class CourseGradeController : Controller
    {

        private readonly ICourseGradeService _service;
        private readonly ICourseService _courseService;

        public CourseGradeController(ICourseGradeService service, ICourseService courseService                                                  )
        {
            _service = service;
            _courseService = courseService;
        }

        // GET: StAcademicData
        //public async Task<IActionResult> Index(string sortOrder, string currentFilter, string StNameSearch, int? BatchId, int? AcademicYearId, StStatus? stStatus, Term? term, Level? level, bool IsCurrentYear, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        //{
        //    FullAllListes("-- الكل --");
        //    var model = new StAcademicDataIndexViewModel();
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";

        //    if (StNameSearch != null)
        //    {

        //    }
        //    else
        //    {
        //        StNameSearch = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = StNameSearch;


        //    var StPersonalDatas = _StPersonalDataRepository.List();

        //    if (!String.IsNullOrEmpty(StNameSearch))
        //    {
        //        StPersonalDatas = StPersonalDatas.Where(s => s.StName.Contains(StNameSearch)).ToList();

        //    }

        //    if (AcademicYearId != null)
        //    {
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {

        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.AcademicYear.Id == AcademicYearId).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //        //StPersonalDatas = StPersonalDatas.Where(x => x.StAcademicDatas.SingleOrDefault(x => x.AcademicYear.Id == AcademicYearId)).ToList();
        //        ViewBag.SelectedBatchId = BatchId;
        //        ViewData["AcademicYearId"] = new SelectList(await FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
        //    }
        //    if (BatchId != null)
        //    {
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {
        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Batch.Id == BatchId).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //        ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
        //    }

        //    if (level != null)
        //    {
        //        model.Level = level;
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {
        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StLevel == level).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //    }
        //    if (term != null)
        //    {
        //        model.Term = term;
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {
        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Term == term).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //    }
        //    if (IsCurrentYear)
        //    {
        //        model.IsCurrentYear = IsCurrentYear;
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {
        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.IsTerm == IsCurrentYear).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //    }
        //    if (stStatus != null)
        //    {
        //        model.StStatus = stStatus;
        //        IList<StPersonalData> sts = new List<StPersonalData>();
        //        foreach (var StPersonalData in StPersonalDatas)
        //        {
        //            var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StStatus == stStatus).ToList();
        //            if (StAcademicDatasTemp.Count > 0)
        //            {
        //                StPersonalData.StAcademicDatas = StAcademicDatasTemp;
        //                sts.Add(StPersonalData);
        //            }
        //        }
        //        StPersonalDatas = sts;
        //    }

        //    return View(model);
        //}



        //All student's Courses with a grade for one term Academy
        public async Task<IActionResult> AllStCourseGrade(int id, bool? courseType)
        {            
            try
            {
                var model = await _service.GetStudentGradesAsync(id, courseType ?? true);
                return View(model);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

               
        // عرض درجات دفعة كاملة (مع فلترة)
        public async Task<IActionResult> AllbatchCourseGrade(AllbatchCourseGradeViewModel filter)
        {
            // يقوم السيرفس بمعالجة الفلاتر الافتراضية وتعبئة القوائم
            var model = await _service.GetBatchGradesFilteredAsync(filter);
            return View(model);
        }

       

        // GET: StAcademicData/Details/5
        public IActionResult StSearch()
        {

            //FullAllStListes("-- أختر --");

            return PartialView("_Delete");
        }



        // عرض نموذج التعديل (Pop-up / Partial)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            try
            {
                var model = await _service.GetEditFormAsync(id.Value);
                return PartialView("_Edit", model);
            }
            catch (DomainException) { return NotFound(); }
        }

        // حفظ التعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCourseGradeViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_Edit", model);

            try
            {
                await _service.UpdateGradeAsync(model);
                // نعيد الموديل مع إشارة نجاح أو نغلق الـ Modal
                // في حالتك يبدو أنك تعيد الـ Partial View
                return PartialView("_Edit", model);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_Edit", model);
            }
        }




        [HttpGet("test")]
        public IActionResult TestMyFirstModelBinder(EditCourseGradeViewModel model)
        {
            return Json(model);
        }
        // POST: StAcademicData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, EditCourseGradeViewModel model, string Grade)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            float? newGrade = null;
        //            StStatusForCourse NewStStatus = model.StStatusForCourse;
        //            if (NewStStatus == StStatusForCourse.ناجح || NewStStatus == StStatusForCourse.راسب)
        //            {
        //                if (Grade is not null)
        //                {
        //                    var cultureInfo = new CultureInfo("en");
        //                    if (!(int.TryParse(Grade,
        //                        NumberStyles.Integer,
        //                        cultureInfo, out var modelGrade)) || !(modelGrade >= 0 && modelGrade <= model.BigGrade))
        //                    {
        //                        ModelState.AddModelError(nameof(model.Grade), " يجب إدخال  درجة المادة رقماً صحيحاً بين 0 -" + model.BigGrade);

        //                        return PartialView("_Edit", model);
        //                    }
        //                    newGrade = (float)modelGrade;
        //                    NewStStatus = newGrade >= model.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(nameof(model.Grade), " يجب إدخال  درجة المادة رقماً صحيحاً بين 0 - " + model.BigGrade);

        //                    return PartialView("_Edit", model);
        //                }
        //            }


        //            var oldcourseGrade = _CourseGradeService.Find(model.Id);

        //            oldcourseGrade.StStatusForCourse = NewStStatus;
        //            oldcourseGrade.Grade = newGrade;

        //            _CourseGradeService.Update(id, oldcourseGrade);
        //            if (model.IsSubCourse is true)
        //            {
        //                var ParentCourseGrade = _CourseGradeService.List()
        //                    .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
        //                    .SingleOrDefault(x => x.Course.Id == oldcourseGrade.Course.ParentId);
        //                var sumGradeSubCourses = oldcourseGrade.Grade ?? 0.0f;
        //                foreach (var SubCourse in ParentCourseGrade.Course.SubCourses)
        //                {
        //                    if (SubCourse.Id != oldcourseGrade.Course.Id)
        //                    {
        //                        sumGradeSubCourses += _CourseGradeService.List()
        //                                                .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
        //                                                .SingleOrDefault(x => x.Course.Id == SubCourse.Id).Grade ?? 0;
        //                    }

        //                }
        //                ParentCourseGrade.Grade = sumGradeSubCourses;
        //                ParentCourseGrade.StStatusForCourse = ParentCourseGrade.Grade >= ParentCourseGrade.Course.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
        //                //CourseGrade ParentCourseGrade = new()
        //                //{

        //                //    Id = model.Id,
        //                //    StStatusForCourse = model.StStatusForCourse,
        //                //    Grade = model.Grade

        //                //};
        //                _CourseGradeService.Update(ParentCourseGrade.Id, ParentCourseGrade);
        //            }
        //            //var specialization = _specializationRepository.Find(model.SpecializationId);

        //            //_CourseGradeRepository.Update(id ,courseGrade);
        //            return PartialView("_Edit", model);
        //            //return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return PartialView("_Edit", model);
        //        }

        //    }
        //    else
        //    {

        //        return PartialView("_Edit", model);
        //    }
        //}

        // GET: CourseController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var courseGrade = _CourseGradeService.Find(id);
        //    if (courseGrade is null)
        //    {
        //        return NotFound();
        //    }


        //    return View("Delete", courseGrade);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, CourseGrade courseGrade)
        //{

        //    try
        //    {
        //        if (id == null || id == 0)
        //        {
        //            return NotFound();
        //        }

        //        if (courseGrade.CourseType == true)
        //        {
        //            ModelState.AddModelError(nameof(courseGrade.CourseType), "");
        //            ViewBag.Message = "لا يمكن حذف درجات المادةالاساسية";
        //            return View("Delete", courseGrade);
        //        }
        //        _CourseGradeService.Delete(id);
        //        return RedirectToAction(nameof(AllbatchCourseGradeFailed));


        //    }
        //    catch
        //    {
        //        return PartialView("_Delete", courseGrade);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> BatchCourseGradeUpload(IFormFile batchGrades, AllbatchCourseGradeViewModel filter)
        {
            try
            {
                var previewModel = await _service.PreviewExcelUploadAsync(batchGrades, filter);
                return View(previewModel);
            }
            catch (DomainException ex)
            {
                ViewBag.Message = ex.Message; // أو استخدام ViewModel.ErrorMessage
                // إعادة عرض الصفحة السابقة
                var model = await _service.GetBatchGradesFilteredAsync(filter);
                return View("AllbatchCourseGrade", model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> BatchCourseGradeXlsxUpdate(BatchCourseGradeUploadVM model)
        {
            try
            {
                await _service.UpdateGradesFromPreviewAsync(model.PreviewGrades);
                return RedirectToAction(nameof(AllbatchCourseGrade));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
      
        public async Task<IActionResult> AllbatchCourseGradeFailed(string searchString, int? SearchAcademicID, Term? term, Level? level, int? CourseId, int? SpecializationId, int? AcademicYearId, bool IsSelectCurrentYear)
        {
            // استدعاء السيرفس الذي سيقوم بكل العمل
            var model = await _service.GetFailedGradesFilteredAsync(
                searchString,
                SearchAcademicID,
                term,
                level,
                CourseId,
                SpecializationId,
                AcademicYearId,
                IsSelectCurrentYear);

            return View(model);
        }


        public async Task<JsonResult> GetCoursess(int? batchId, Level level, Term term)
        {
            if (batchId == null)
            {
                return null;
            }
            var CoursessList = await _courseService.GetCoursesByBatchAsync(batchId.Value, level, term);
            return Json(new SelectList(CoursessList, "Id", "CourseName"));

        }

        public async Task<JsonResult> GetCoursessbySpecialization(int? SpecializationId, Level? level, Term? term)
        {
            if (SpecializationId == null)
            {
                return null;
            }
            var CoursessList = await _courseService.GetCoursesBySpecializationAsync(SpecializationId.Value, level, term);
            return Json(new SelectList(CoursessList, "Id", "CourseName"));
        }

       

        


        public async Task<ActionResult> ExportCourseGradeToExcel(string searchString, int? SearchAcademicID, Term? term, Level? level, int? CourseId, int? SpecializationId, int? AcademicYearId, bool IsSelectCurrentYear)
        {
            var stream = await _service.ExportFailedGradesToExcelAsync(searchString, SearchAcademicID, term, level, CourseId, SpecializationId, AcademicYearId);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "بيان درجات التكميلية.xlsx");
        }




     
    }


}
