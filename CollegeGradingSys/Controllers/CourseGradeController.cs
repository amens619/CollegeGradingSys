using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using cloudscribe.Pagination.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
//using GemBox.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Http;
using CollegeGradingSys.Helper;
using System.Drawing;
using System.IO;
using OfficeOpenXml.Drawing;

namespace CollegeGradingSys.Controllers
{
    public class CourseGradeController : Controller
    {

        private readonly ICollegeGradingSysRepository<CourseGrade> _CourseGradeRepository;
        private readonly ICollegeGradingSysRepository<StAcademicData> _StAcademicDataRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<Batch> _BatchRepository;
        private readonly ICollegeGradingSysRepository<AcademicYear> _AcademicYearRepository;
        private readonly ICollegeGradingSysRepository<Specialization> _SpecializationRepository;
        private readonly ICollegeGradingSysRepository<Course> _CourseRepository;
        private IFormatProvider cultureInfo;

        public CourseGradeController(ICollegeGradingSysRepository<CourseGrade> CourseGradeRepository, ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository
            , ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository
            , ICollegeGradingSysRepository<Batch> BatchRepository
            , ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository
            ,ICollegeGradingSysRepository<Specialization> SpecializationRepository
            , ICollegeGradingSysRepository<Course> CourseRepository)
        {
            _CourseGradeRepository = CourseGradeRepository;
            _StAcademicDataRepository = StAcademicDataRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
            _BatchRepository = BatchRepository;
            _AcademicYearRepository = AcademicYearRepository;
            _SpecializationRepository = SpecializationRepository;
            _CourseRepository = CourseRepository;
        }

        // GET: StAcademicData
        public IActionResult Index( string sortOrder, string currentFilter, string StNameSearch, int? BatchId, int? AcademicYearId, StStatus? stStatus, Term? term,Level? level,bool IsCurrentYear, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        {
            FullAllListes("-- الكل --");
            var model = new StAcademicDataIndexViewModel();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";

            



            if (StNameSearch != null)
            {
               
            }
            else
            {
                StNameSearch = currentFilter;
            }

            ViewBag.CurrentFilter = StNameSearch;


            var StPersonalDatas = _StPersonalDataRepository.List();

            if (!String.IsNullOrEmpty(StNameSearch))
            {
                StPersonalDatas = StPersonalDatas.Where(s => s.StName.Contains(StNameSearch)).ToList();

            }

            if (AcademicYearId != null)
            {
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {

                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.AcademicYear.Id == AcademicYearId).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;
                //StPersonalDatas = StPersonalDatas.Where(x => x.StAcademicDatas.SingleOrDefault(x => x.AcademicYear.Id == AcademicYearId)).ToList();
                ViewBag.SelectedBatchId = BatchId;
                ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            }
            if (BatchId != null)
            {
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Batch.Id == BatchId).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;             
                ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
            }
            
            if (level != null)
            {
                model.Level = level;
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StLevel == level).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;
            }
            if (term != null)
            {
                model.Term = term;
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Term == term).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;
            }
            if (IsCurrentYear)
            {
                model.IsCurrentYear = IsCurrentYear;
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.IsTerm == IsCurrentYear).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;
            }
            if (stStatus != null)
            {
                model.StStatus = stStatus;
                IList<StPersonalData> sts = new List<StPersonalData>();
                foreach (var StPersonalData in StPersonalDatas)
                {
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StStatus == stStatus).ToList();
                    if (StAcademicDatasTemp.Count > 0)
                    {
                        StPersonalData.StAcademicDatas = StAcademicDatasTemp;
                        sts.Add(StPersonalData);
                    }
                }
                StPersonalDatas = sts;           
            }
            

            //var model = new StPersonalDataFilteringIndexData
            //{
            //    BatchId = -1,
            //    Batches = FillSelectBatchsList("-- الكل --"),
            //    //StPersonalDatas = StPersonalDatas.ToList()
            //    pagedResult = result
            //};

            //if (id != null)
            //{
            //    model.StHighSchoolData = StHighSchoolDataRepository.Find(id ?? 0);
            //    ViewData["AcademicID"] = id;
            //}
            ////BatchNamelist
            ////int pageSize = 3;
            ////int pageNumber = (page ?? 1);



            
            //IList<StPersonalDataVM>  stPersonalDataVMs = new List<StPersonalDataVM>();
            //model.StAcademicDataVMs = new List<StAcademicDataVM>();
            //foreach (var stPersonalData in StPersonalDatas)
            //{

            //    model.StAcademicDataVMs.Add( new StAcademicDataVM()
            //    {
            //        AcademicID = stPersonalData.AcademicID,
            //        StName = stPersonalData.StName,
            //         IsSelected= false,
            //        StAcademicData = stPersonalData.StAcademicDatas
            //        .OrderBy(x => x.AcademicYear.AcademicYearStart.Year)
            //        .ThenBy(x => x.StLevel)
            //        .ThenBy(x => x.Term).LastOrDefault(),

            //    });
               
            //}
            
             

            return View(model);
        }

       
        
       //All student's Courses with a grade for one term Academy
        public IActionResult AllStCourseGrade(int id, bool? courseType)
        {
            var stAcademicData = _StAcademicDataRepository.Find(id);
            
            bool _courseType  = courseType ??= true;
            ICollection<CourseGrade> courseGrades = new List<CourseGrade>();
            if (stAcademicData != null)
            {
                 courseGrades = _CourseGradeRepository
                    .List()
                    .Where(s => s.StAcademicData.Id.Equals(id))
                    .Where(x => x.CourseType == courseType)
                    .OrderBy(x => x.Id)
                    .ToList();                                   
            }

            var model = new CourseGradeIndexViewModel()
            {
                Id = id,
                AcademicID = stAcademicData.StPersonalData.AcademicID,
                StName = stAcademicData.StPersonalData.StName,
                AcademicYear = stAcademicData.AcademicYear.AcademicYearName,
                Batch = stAcademicData.Batch.BatchName,
                CourseType = _courseType ? "عام" : "تكميلي",
                IsCurrentYear = stAcademicData.AcademicYear.IsCurrentYear,
                Level = stAcademicData.StLevel.ToString(),
                Specialization = stAcademicData.Batch.Specialization.SpecializationName,
                StStatus = stAcademicData.StStatus.ToString(),
                Term = stAcademicData.Term.ToString(),
                CourseGrades = courseGrades
            };
            return View(model);
        }



        //All student's Courses with a grade for one term Academy
        public IActionResult AllbatchCourseGrade(int? BatchId, int? AcademicYearId, StStatusForCourse? stStatusForCourse, Term? term, Level? level, bool? CourseType , int? CourseId)
        {

            //term ??= Term.الأول;
            //int? batchId= BatchId;
            //int courseId = CourseId ?? 0;

            //var model = new AllbatchCourseGradeViewModel();

            //var AcademicYearsList = _AcademicYearRepository.List().OrderByDescending(x => x.AcademicYearStart).ToList();

            //if (AcademicYearsList != null)
            //{

            //    var academicYearId = AcademicYearId ??= GetCurrentYear().Id;
            //    model.IsCurrentYear = AcademicYearsList.SingleOrDefault(x => x.Id == academicYearId).IsCurrentYear;
            //    ViewData["AcademicYearId"] = new SelectList(AcademicYearsList, "Id", "AcademicYearName", academicYearId);

            //    var Batchs = getBatchsOfOneAcademicYear(academicYearId);
            //    if (Batchs != null && Batchs.Count > 0)
            //    {
            //         batchId = BatchId ??= Batchs[0].Id;
            //        ViewData["BatchId"] = new SelectList(Batchs, "Id", "BatchName", batchId);

            //         level ??= Batchs[0].StAcademicDatas.LastOrDefault().StLevel;
            //         var courses = getCoursessOfOneBatch(batchId, level, term);
            //        if (courses !=null)
            //        {
            //            courseId = CourseId ??= courses[0].Id;
            //            ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName", CourseId ?? 0);
            //            model.courseName = courses.SingleOrDefault(x => x.Id == courseId).CourseName;
            //        }

            //    }

            //}




            //CourseType ??= true;

            //var courseGrades = _CourseGradeRepository.List()
            //    .Where(x => x.StAcademicData.AcademicYear.Id == AcademicYearId)
            //    .Where(x => x.StAcademicData.Batch.Id == batchId)
            //    .Where(x => x.StAcademicData.StLevel == level)
            //    .Where(x => x.StAcademicData.Term == term)
            //    .Where(x => x.Course.Id == courseId)
            //    .Where(x => x.CourseType == CourseType);

            //if (stStatusForCourse != null)
            //{
            //    courseGrades = courseGrades.Where(x => x.StStatusForCourse == stStatusForCourse);
            //    model.StStatusForCourse = stStatusForCourse ?? StStatusForCourse.غير_محدد;
            //}

            //model.Level = level;
            //model.Term = term;           
            //model.CourseType = CourseType ?? true;
            //model.CourseGrades = courseGrades.ToList();           
            //return View(model);
            var model = getAllbatchCourseGradeViewModel(BatchId, AcademicYearId, stStatusForCourse, term, level, CourseType, CourseId);

            return View(model);

        }


        public IActionResult AllbatchCourseGradeFailed(string searchString, int? SearchAcademicID, Term? term, Level? level,int? CourseId,int? SpecializationId ,int? AcademicYearId,bool IsSelectCurrentYear)
        {
            var model = getAllCourseGradeFailedViewModel(searchString, SearchAcademicID, term, level, CourseId, SpecializationId, AcademicYearId, IsSelectCurrentYear);

            return View(model);
        }

        // GET: StAcademicData/Details/5
        public IActionResult StSearch()
        {

            FullAllStListes("-- أختر --");

            return PartialView("_Delete");
        }

        // GET: StAcademicData/Create
        public IActionResult Create(int id)
        {
            var stPersonalData = _StPersonalDataRepository.Find(id);
            var model = new CreateStAcademicDataDataViewModel()
            {
                AcademicID = stPersonalData.AcademicID
            };

            FullAllListes("-- أختر --");
            return View(model);
        }

      
            // POST: StAcademicData/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear,AcademicID,AcademicYearId,BatchId")] CreateStAcademicDataDataViewModel model)
        {
           
            return View(model);
        }

       

        // GET: StAcademicData/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseGrade = _CourseGradeRepository.Find(id ?? 0);
            if (courseGrade == null)
            {
                return NotFound();
            }
            var stringGrade = "";
            if (courseGrade.Grade != null)
            {
                stringGrade = courseGrade.Grade.Value.ToString("N2", new CultureInfo("en"));
            }
             
            var model = new EditCourseGradeViewModel()
            {
                Id = courseGrade.Id,
                CourseName = courseGrade.Course.CourseName,
                CourseType = courseGrade.CourseType ? "عام" : "تكميلي",
                BigGrade = courseGrade.Course.BigGrade,
                SmallGrade = courseGrade.Course.SmallGrade,
                StStatusForCourse = courseGrade.StStatusForCourse,
                ParentId = courseGrade.Course.ParentId,
                IsSubCourse = courseGrade.Course.IsSubCourse,
                Grade = stringGrade
            };            
           
          
            return PartialView("_Edit", model);
        }


        [HttpGet("test")]
        public IActionResult TestMyFirstModelBinder(EditCourseGradeViewModel model)
        {
            return Json(model);
        }
        // POST: StAcademicData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditCourseGradeViewModel model,string Grade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    float? newGrade =null;
                    StStatusForCourse NewStStatus = model.StStatusForCourse;
                    if(NewStStatus== StStatusForCourse.ناجح || NewStStatus == StStatusForCourse.راسب)
                    {
                        if (Grade is not null)
                        {
                            var cultureInfo = new CultureInfo("en");
                            if (!(decimal.TryParse(Grade,
                                NumberStyles.AllowDecimalPoint,
                                cultureInfo, out var modelGrade)) || !(modelGrade >= 0 && modelGrade <= 100))
                            {
                                ModelState.AddModelError(nameof(model.Grade), " يجب إدخال  درجة المادة رقماً بين 0 - 100.");

                                return PartialView("_Edit", model);
                            }
                            newGrade = (float)modelGrade;
                            NewStStatus = newGrade >= model.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(model.Grade), " يجب إدخال  درجة المادة رقماً بين 0 - 100.");

                            return PartialView("_Edit", model);
                        }
                    }
                   
                    
                    var oldcourseGrade = _CourseGradeRepository.Find(model.Id);
                   
                    oldcourseGrade.StStatusForCourse = NewStStatus;
                    oldcourseGrade.Grade = newGrade; 
                   
                    _CourseGradeRepository.Update(id, oldcourseGrade);
                    if (model.IsSubCourse is true )
                    {

                        var ParentCourseGrade = _CourseGradeRepository.List()
                            .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
                            .SingleOrDefault(x => x.Course.Id == oldcourseGrade.Course.ParentId);
                        var sumGradeSubCourses = oldcourseGrade.Grade ?? 0.0f;
                        foreach (var SubCourse in ParentCourseGrade.Course.SubCourses)
                        {
                            if(SubCourse.Id != oldcourseGrade.Course.Id)
                            {
                                sumGradeSubCourses += _CourseGradeRepository.List()
                                                        .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
                                                        .SingleOrDefault(x => x.Course.Id == SubCourse.Id).Grade ?? 0;                              
                            }

                        }
                        ParentCourseGrade.Grade = sumGradeSubCourses / ParentCourseGrade.Course.SubCourses.Count;
                        ParentCourseGrade.StStatusForCourse = ParentCourseGrade.Grade >= ParentCourseGrade.Course.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
                        //CourseGrade ParentCourseGrade = new()
                        //{

                        //    Id = model.Id,
                        //    StStatusForCourse = model.StStatusForCourse,
                        //    Grade = model.Grade

                        //};
                        _CourseGradeRepository.Update(ParentCourseGrade.Id, ParentCourseGrade);
                    }
                    //var specialization = _specializationRepository.Find(model.SpecializationId);
                   
                    //_CourseGradeRepository.Update(id ,courseGrade);
                    return PartialView("_Edit", model);
                    //return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return PartialView("_Edit", model);
                }

            }
            else
            {

                return PartialView("_Edit", model);
            }
        }

        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var courseGrade = _CourseGradeRepository.Find(id);
            if (courseGrade is null)
            {
                return NotFound();
            }
           

            return View("Delete", courseGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CourseGrade courseGrade)
        {
           
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                if (courseGrade.CourseType == true)
                {
                    ModelState.AddModelError(nameof(courseGrade.CourseType), "");
                    ViewBag.Message = "لا يمكن حذف درجات المادةالاساسية";
                    return View("Delete", courseGrade);
                }
                _CourseGradeRepository.Delete(id);
                return RedirectToAction(nameof(AllbatchCourseGradeFailed));
                
                
            }
            catch
            {
                return PartialView("_Delete", courseGrade);
            }
        }


        public IActionResult BatchCourseGradeUpload(IFormFile batchGrades, int? BatchId, int? AcademicYearId, StStatusForCourse? stStatusForCourse, Term? term, Level? level, bool? CourseType, int? CourseId)
        {



            var model = getAllbatchCourseGradeViewModel(BatchId,AcademicYearId,stStatusForCourse,term,level,CourseType,CourseId);
            var vmodel = new BatchCourseGradeUploadVM()
            {
                CourseName = model.courseName,
                CourseType = model.CourseType,
                IsCurrentYear = model.IsCurrentYear,
                Level = model.Level,
                StStatusForCourse = model.StStatusForCourse,
                Term = model.Term,
                 CourseGrades = new List<CourseGradeVM>()
           

            };
            
            List<XlsxCourseGrade> xlsxCourseGrades = new List<XlsxCourseGrade>();
            if (ModelState.IsValid)
            {
                if (batchGrades?.Length > 0)
                {
                    vmodel.BatchGrades = batchGrades;
                    var stream = batchGrades.OpenReadStream();
                    
                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;

                            for (var row = 7; row <= rowCount; row++)
                            {
                                try
                                {
                                    var xlsxCourseGrade = new XlsxCourseGrade();
                                   


                                    var xlsxAcademicID = Convert.ToInt32(worksheet.Cells[row, 3].Value?.ToString());
                                    
                                    xlsxCourseGrade.XlsxAcademicID = xlsxAcademicID;

                                    //================
                                    var txtXlsxGrade = worksheet.Cells[row, 4].Value?.ToString();
                                    if(txtXlsxGrade  is not null)
                                    {
                                        var cultureInfo = new CultureInfo("en");
                                        if (!(decimal.TryParse(txtXlsxGrade,
                                        NumberStyles.AllowDecimalPoint,
                                        cultureInfo, out var xlsxGrade)) || !(xlsxGrade >= 0 && xlsxGrade <= 100))
                                        {
                                            xlsxCourseGrade.XlsxErrorMSG = " لن يتم تغير الدرجة ... يجب إدخال  درجة المادة رقماً بين 0 - 100.";
                                        }
                                        else
                                        {
                                            xlsxCourseGrade.XlsxGrade = (float)xlsxGrade;
                                        }
                                    }
                                    //=================                  
                                    xlsxCourseGrades.Add(xlsxCourseGrade);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Something went wrong");
                                }
                            }
                        }

                      

                    }
                    catch (Exception e)
                    {
                        return View();
                    }

                }
            }
            foreach (var courseGrade in model.CourseGrades)
            {
                bool IsGradeChange = false;
                var academicID = courseGrade.StAcademicData.StPersonalData.AcademicID;
                string xlsxErrorMSG="";

                var xlsxCourseGrade = xlsxCourseGrades.SingleOrDefault(x => x.XlsxAcademicID == academicID);

                if (xlsxCourseGrade is not null)
                {
                    courseGrade.Grade = xlsxCourseGrade.XlsxGrade;
                    if (courseGrade.Grade != null)
                    {
                        IsGradeChange = true;
                        courseGrade.StStatusForCourse = courseGrade.Grade >= courseGrade.Course.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
                    }
                    else { courseGrade.StStatusForCourse = StStatusForCourse.غير_محدد; }
                    
                    xlsxErrorMSG = xlsxCourseGrade.XlsxErrorMSG;
                }

                vmodel.CourseGrades.Add(new CourseGradeVM()
                {
                    Id = courseGrade.Id,
                    AcademicID = courseGrade.StAcademicData.StPersonalData.AcademicID,
                    StName = courseGrade.StAcademicData.StPersonalData.StName,
                    StStatusForCourse = courseGrade.StStatusForCourse,
                    Grade = courseGrade.Grade,
                    IsGradeChange= IsGradeChange,
                    Course = courseGrade.Course,
                     Note = xlsxErrorMSG,
                });

            }
            return View(vmodel);
        }
        public IActionResult BatchUserUpload(IFormFile batchUsers)
        {

            

            return View();
        }

        public IActionResult BatchCourseGradeXlsxUpdate(IList<CourseGradeVM> courseGrades)
        {
            foreach (var courseGrade in courseGrades)
            {
                if (courseGrade.IsGradeChange)
                {
                    var oldcourseGrade = _CourseGradeRepository.Find(courseGrade.Id);

                    oldcourseGrade.StStatusForCourse = courseGrade.StStatusForCourse;
                    oldcourseGrade.Grade = courseGrade.Grade;

                    _CourseGradeRepository.Update(courseGrade.Id, oldcourseGrade);
                    if (courseGrade.Course.IsSubCourse is true)
                    {

                        var ParentCourseGrade = _CourseGradeRepository.List()
                            .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
                            .SingleOrDefault(x => x.Course.Id == oldcourseGrade.Course.ParentId);
                        var sumGradeSubCourses = oldcourseGrade.Grade ?? 0.0f;
                        foreach (var SubCourse in ParentCourseGrade.Course.SubCourses)
                        {
                            if (SubCourse.Id != oldcourseGrade.Course.Id)
                            {
                                sumGradeSubCourses += _CourseGradeRepository.List()
                                                        .Where(x => x.StAcademicData.Id == oldcourseGrade.StAcademicData.Id)
                                                        .SingleOrDefault(x => x.Course.Id == SubCourse.Id).Grade ?? 0;
                            }

                        }
                        ParentCourseGrade.Grade = sumGradeSubCourses / ParentCourseGrade.Course.SubCourses.Count;
                        ParentCourseGrade.StStatusForCourse = ParentCourseGrade.Grade >= ParentCourseGrade.Course.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;

                        _CourseGradeRepository.Update(ParentCourseGrade.Id, ParentCourseGrade);
                    }
                }
            }


            return RedirectToAction(nameof(AllbatchCourseGrade));
           
        }
        
        List<Batch> FillSelectBatchsList(string batchName)
        {
            var Batches = _BatchRepository.List().ToList();
            Batches.Insert(0, new Batch { Id = -1, BatchName = batchName  });

            return Batches;
        }

        private AllbatchCourseGradeViewModel getAllbatchCourseGradeViewModel(int? BatchId, int? AcademicYearId, StStatusForCourse? stStatusForCourse, Term? term, Level? level, bool? CourseType, int? CourseId)
        {
            term ??= Term.الأول;
            int? batchId = BatchId;
            int courseId = CourseId ?? 0;

            var model = new AllbatchCourseGradeViewModel();

            var AcademicYearsList = _AcademicYearRepository.List().OrderByDescending(x => x.AcademicYearStart).ToList();

            if (AcademicYearsList != null)
            {

                var academicYearId = AcademicYearId ??= GetCurrentYear().Id;
                model.IsCurrentYear = AcademicYearsList.SingleOrDefault(x => x.Id == academicYearId).IsCurrentYear;
                ViewData["AcademicYearsList"] = new SelectList(AcademicYearsList, "Id", "AcademicYearName", academicYearId);
                ViewData["AcademicYearId"] = academicYearId;
                var Batchs = getBatchsOfOneAcademicYear(academicYearId);
                if (Batchs != null && Batchs.Count > 0)
                {
                    batchId = BatchId ??= Batchs[0].Id;
                    ViewData["BatchId"] = batchId;                    
                    ViewData["BatchsList"] = new SelectList(Batchs, "Id", "BatchName", batchId);

                    level ??= Batchs[0].StAcademicDatas.LastOrDefault().StLevel;
                    var courses = getCoursessOfOneBatch(batchId, level, term);
                    if (courses != null)
                    {
                        courseId = CourseId ??= courses[0].Id;
                        ViewData["CourseId"] = courseId;
                        ViewData["CourseList"] = new SelectList(courses, "Id", "CourseName", CourseId ?? 0);
                        model.courseName = courses.SingleOrDefault(x => x.Id == courseId).CourseName;
                    }

                }

            }




            CourseType ??= true;

            var courseGrades = _CourseGradeRepository.List()
                .Where(x => x.StAcademicData.AcademicYear.Id == AcademicYearId)
                .Where(x => x.StAcademicData.Batch.Id == batchId)
                .Where(x => x.StAcademicData.StLevel == level)
                .Where(x => x.StAcademicData.Term == term)
                .Where(x => x.Course.Id == courseId)
                .Where(x => x.CourseType == CourseType);

            if (stStatusForCourse != null)
            {
                courseGrades = courseGrades.Where(x => x.StStatusForCourse == stStatusForCourse);
                model.StStatusForCourse = stStatusForCourse ?? StStatusForCourse.غير_محدد;
            }

            model.Level = level;
            model.Term = term;
            model.CourseType = CourseType ?? true;
            model.CourseGrades = courseGrades.ToList();
            return (model);
        }

        
        private AllbatchCourseGradeFailedViewModel getAllCourseGradeFailedViewModel(string searchString, int? SearchAcademicID, Term? term, Level? level, int? CourseId, int? SpecializationId, int? AcademicYearId, bool IsSelectCurrentYear)
        {
            bool isTermSelected = false;
            bool isLevelSelected = false;
            bool isSpecializationIdSelected = false;
            int courseId = CourseId ?? -1;
            //int specializationId = SpecializationId ?? _SpecializationRepository.List().FirstOrDefault().Id;

            if (IsSelectCurrentYear == true)
            {
                var currentYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                if (currentYear != null)
                {
                   
                    AcademicYearId = currentYear.Id;
                }

            }
            else
            {
                AcademicYearId = -1;
            }
            ViewBag.CurrentFilter = searchString;
            ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            var model = new AllbatchCourseGradeFailedViewModel();


            var courses = getCoursessOfOneSpecialization(SpecializationId, level, term);
            if (courses != null)
            {
                if (courseId != -1)
                {
                    model.courseName = courses.SingleOrDefault(x => x.Id == courseId).CourseName;
                }

                ViewData["CourseId"] = courseId;
                ViewData["CourseList"] = new SelectList(courses, "Id", "CourseName", courseId);

            }
            var AcademicYearsList = _AcademicYearRepository.List().OrderByDescending(x => x.AcademicYearStart).ToList();

            if (AcademicYearsList != null)
            {

                var academicYearId = AcademicYearId ??= -1;
                // model.IsCurrentYear = AcademicYearsList.SingleOrDefault(x => x.Id == academicYearId).IsCurrentYear;
                ViewData["AcademicYearsList"] = new SelectList(AcademicYearsList, "Id", "AcademicYearName", academicYearId);
                ViewData["AcademicYearId"] = academicYearId;
            }

            var courseGrades = _CourseGradeRepository.List()
                            .Where(x => x.CourseType == false);


            if (AcademicYearId != null && AcademicYearId != -1)
            {
                
                model.AcademicYearId = AcademicYearId ?? 1;
                courseGrades = courseGrades.Where(x => x.StAcademicData.AcademicYear.Id == AcademicYearId).ToList();
            }

            if (SpecializationId != null && SpecializationId != -1)
            {
                isSpecializationIdSelected = true;
                model.SpecializationId = SpecializationId ?? 1;
                courseGrades = courseGrades.Where(x => x.StAcademicData.Batch.Specialization.Id == SpecializationId).ToList();
            }
            if (level != null)
            {
               
                 isLevelSelected = true;
                
                model.Level = level;
                courseGrades = courseGrades.Where(x => x.StAcademicData.StLevel == level).ToList();
            }
            if (term != null)
            {
                isTermSelected = true;
                model.Term = term;
                courseGrades = courseGrades.Where(x => x.StAcademicData.Term == term).ToList();
            }
            if (CourseId != null && CourseId != -1)
            {
                courseGrades = courseGrades.Where(x => x.Course.Id == courseId).ToList();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                courseGrades = courseGrades.Where(s => s.StAcademicData.StPersonalData.StName.Contains(searchString)).ToList();

            }

            if (SearchAcademicID != null)
            {
                ViewData["SearchAcademicID"] = SearchAcademicID;
                courseGrades = courseGrades.Where(s => s.StAcademicData.StPersonalData.AcademicID.Equals(SearchAcademicID)).ToList();

            }





            model.CourseGrades = courseGrades.ToList();
            model.isExportBtnEnable = isTermSelected && isLevelSelected && isSpecializationIdSelected;
            ViewData["Specializations"] = new SelectList(FillSelectSpecializationsList(), "Id", "SpecializationName");

            return (model);

        }


        List<Batch> getBatchsOfOneAcademicYear(int AcademicYearId)
        {
            var Batches = new List<Batch>();
            var StAcademicDatas = _AcademicYearRepository.Find(AcademicYearId).StAcademicDatas;
            var query = StAcademicDatas
                          .GroupBy(x => x.Batch)
                          .Distinct()
                          //.Where(g => g.Count() >= 1)
                          .Select(y => y.Key)
                          .ToList();

            foreach (Batch  batch in query)
            {
                Batches.Add( _BatchRepository.Find(batch.Id));
            }

            return Batches;
        }
        List<AcademicYear> FillSelectAcademicYearesList(string academicYearName)
        {
            var AcademicYeares = _AcademicYearRepository.List().ToList();
            AcademicYeares.Insert(0, new AcademicYear { Id = -1, AcademicYearName = academicYearName });

            return AcademicYeares;
        }
        List<StPersonalData> FillSelectStPersonalDatasList(string stName)
        {
            var StPersonalDatas = _StPersonalDataRepository.List().ToList();
            StPersonalDatas.Insert(0, new StPersonalData {  AcademicID = -1,   StName = stName });

            return StPersonalDatas;
        }
        private void FullAllListes(string text)
        {
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(text), "Id", "AcademicYearName");
            ViewData["BatchId"] = new SelectList(FillSelectBatchsList(text), "Id", "BatchName");

            ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationesList(text), "Id", "SpecializationName");

        }

        private void FullAllStListes(string text)
        {            
            ViewData["AcademicID"] = new SelectList(FillSelectStPersonalDatasList(text), "AcademicID", "StName");
        }

        List<Specialization> FillSelectSpecializationesList(string specializationName)
        {
            var specializationes = _SpecializationRepository.List().ToList();
            specializationes.Insert(0, new Specialization { Id = -1,  SpecializationName = specializationName });

            return specializationes;
        }

        public JsonResult GetBatchs(int academicYearId)
        {
            var BatchsList = getBatchsOfOneAcademicYear(academicYearId);           
            return Json(new SelectList(BatchsList, "Id", "BatchName"));
        }


        List<Course> getCoursessOfOneBatch(int? batchId, Level? level,Term? term)
        {
            Batch selectedBatch = _BatchRepository.Find(batchId ?? 0);
            var courses = _CourseRepository.List()
                                  .Where(x => x.Specialization.Id == selectedBatch.Specialization.Id)
                                  .Where(x => x.Level == level)
                                  .Where(x => x.Term == term).ToList();
            return (courses);
        }

        List<Course> getCoursessOfOneSpecialization(int? SpecializationId, Level? level, Term? term)
        {

            var Courses = _CourseRepository.List().ToList();
            if (SpecializationId != null && SpecializationId != -1)
            {                
                Courses = Courses.Where(x => x.Specialization.Id == SpecializationId).ToList();
            }
            if (level != null)
            {
                Courses = Courses.Where(x => x.Level == level).ToList();
            }
            if (term != null)
            {
               Courses = Courses.Where(x => x.Term == term).ToList();
            }                                 
                                  
                                 
            return (Courses);
        }

        public JsonResult GetCoursess(int? batchId, Level level, Term term)
        {
            if (batchId == null)
            {
                return null;
            }
            var CoursessList = getCoursessOfOneBatch(batchId, level, term);
            return Json(new SelectList(CoursessList, "Id", "CourseName"));
        }

        public JsonResult GetCoursessbySpecialization(int? SpecializationId, Level? level, Term? term)
        {
            if (SpecializationId == null)
            {
                return null;
            }
            var CoursessList = getCoursessOfOneSpecialization(SpecializationId, level, term);
            return Json(new SelectList(CoursessList, "Id", "CourseName"));
        }

        private AcademicYear GetCurrentYear()
        {
            var currentYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
            return (currentYear);
        }

        List<Specialization> FillSelectSpecializationsList()
        {
            var specializations = _SpecializationRepository.List().ToList();


            return specializations;
        }


        public ActionResult ExportCourseGradeToExcel(string searchString, int? SearchAcademicID, Term? term, Level? level, int? CourseId, int? SpecializationId, int? AcademicYearId, bool IsSelectCurrentYear)
        {
           var model = getAllCourseGradeFailedViewModel(searchString, SearchAcademicID, term, level, CourseId, SpecializationId, AcademicYearId, IsSelectCurrentYear);
            // Get the user list 
           
            //var users = GetlistOfUsers();
            var courses = getCoursessOfOneSpecialization(SpecializationId, level, term);
                courses = courses.Where(x => x.IsSubCourse == false).ToList();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add(" كشف التكميلية طلاب");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                const int startColumn = 3;
                var row = startRow;

                worksheet.Column(1).Width = 18.43;
                worksheet.Column(2).Width = 41.71;
                worksheet.Column(3).Width = 101.71;
                int coursesNo = 1;
                for (; coursesNo <= courses.Count(); coursesNo++)
                {
                    worksheet.Column(coursesNo + startColumn).Width = 24.86;
                }
               
                
                
                worksheet.Column(coursesNo + startColumn).Width = 73.71;                
                //==========================
                worksheet.Row(1).Height = 45;
                worksheet.Row(2).Height = 45;
                worksheet.Row(3).Height = 45;
                worksheet.Row(4).Height = 30.5;
                worksheet.Row(5).Height = 144;




                //Create Headers and format them
                worksheet.Cells["B1:C1"].Value = "       الجمهورية اليمنية";
                using (var r = worksheet.Cells["B1:C1"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells["B2:c2"].Value = "       جامعة الإيمان";
                using (var r = worksheet.Cells["B2:C2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells["B3:C3"].Value = "       فرع حضرموت";
                using (var r = worksheet.Cells["B3:C3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //=============================

                worksheet.Cells["A5:N5"].Style.Font.Size = 28;
                worksheet.Cells["B1:B3"].Style.Font.Size = 36;

                worksheet.Cells["A5:N5"].Style.Font.Name = "Khalid Art bold";

                //=============================
                var academicYear = _AcademicYearRepository.Find(model.AcademicYearId);
                worksheet.Cells[4, startColumn + 1, 4, coursesNo + 1].Style.Font.Size = 28;
                
                worksheet.Cells[4, startColumn + 1,4 ,coursesNo+1].Value = academicYear != null ? (" نتيجة إمتحانات       " + "المستوى: " + model.Level + "       الفصل: " + model.Term + "    للعام الجامعي: " + academicYear.AcademicYearNameH +  " الموافق " + academicYear.AcademicYearName ) : (" نتيجة إمتحانات       " + "المستوى: " + model.Level + "       الفصل: " + model.Term );
                using (var r = worksheet.Cells[4, startColumn + 1, 4, coursesNo + 1])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                ////////============================
                worksheet.Cells[1, coursesNo + startColumn-1, 1, coursesNo + startColumn ].Value = "Republic of Yemen";
                using (var r = worksheet.Cells[1, coursesNo + startColumn - 1, 1, coursesNo + startColumn ])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Times New Roman";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells[2, coursesNo + startColumn - 1, 2, coursesNo + startColumn ].Value = "AL - Eman university";
                using (var r = worksheet.Cells[2, coursesNo - 1 + startColumn, 2, coursesNo + startColumn ])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Stenc";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells[3, coursesNo + startColumn - 1, 3, coursesNo + startColumn].Value = "Hadhramout branch";

                using (var r = worksheet.Cells[3, coursesNo + startColumn-1, 3, coursesNo + startColumn ])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Times New Roman";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //=====================================
                //////var academicYear = GetCurrentYear();
                //////worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");
               
                //================================

                int rowIndex = 1;
                int colIndex = 7;
                int PixelTop = 17;
                int PixelLeft = 1721;
                int Height = 153;
                int Width = 100;
                Image img = Image.FromFile(@"wwwroot/images/CollegeIcon.jpg");
                
                ExcelPicture pic = worksheet.Drawings.AddPicture("Sample", img);                
                pic.SetPosition(PixelTop, PixelLeft);  
               

                using (var r = worksheet.Cells["A5:C5"])
                {
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                row = startRow ;
                //================================
                worksheet.Cells["A5"].Value = "م";
                               //================================
                worksheet.Cells["B5"].Value = "الرقم الاكاديمي";
                              //================================
                worksheet.Cells["C5"].Value = "الاسم";
                //================================
                 var Column = startColumn;
                foreach (var course in courses)
                {
                    Column++;
                    using (var r = worksheet.Cells[row, Column])
                    {
                        r.Style.WrapText = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Top.Color.SetColor(Color.Black);

                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Color.SetColor(Color.Black);

                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Color.SetColor(Color.Black);

                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                   
                    worksheet.Cells[row, Column].Value = course.CourseName;
                }


                //================================
                Column++;
                using (var r = worksheet.Cells[row, Column])
                {
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                worksheet.Cells[row, Column].Value = "ملاحظات";



                var stsCourseGrade = model.CourseGrades.GroupBy(x => x.StAcademicData.StPersonalData);


                
                var no = 1;
                foreach (var OneSt in stsCourseGrade)
                {
                    row++;
                    worksheet.Row(row).Height = 40;
                    worksheet.Cells[row, 1].Value = no;

                    DrowCell(worksheet, row, 1, Color.White);
                    
                    
                    //=================================================
                    worksheet.Cells[row, 2].Value = OneSt.Key.AcademicID;
                    DrowCell(worksheet, row, 2, colGradFromHex);                  
                    //================================================
                    worksheet.Cells[row, 3].Value = OneSt.Key.StName;
                    DrowCell(worksheet, row, 3, Color.White);
                    //================================================
                    Column = 3;
                    foreach (var course in courses)
                    {
                        Column++;
                        var courseGradeTemp = OneSt.FirstOrDefault(x => x.Course.Id == course.Id);
                        if (courseGradeTemp != null)

                            worksheet.Cells[row, Column].Value = courseGradeTemp.Grade != null ? courseGradeTemp.Grade : 0 ;
                        else
                            worksheet.Cells[row, Column].Value = "-";
                        DrowCell(worksheet, row, Column, colGradFromHex);
                    }
                    //=================================================
                    worksheet.Cells[row, Column + 1].Value = " ";
                    DrowCell(worksheet, row, Column + 1, Color.White);

                    no++;
                }
                for (; no <=24; no++)
                {
                    row++;
                    worksheet.Row(row).Height = 40;
                    worksheet.Cells[row, 1].Value = no;

                    DrowCell(worksheet, row, 1, Color.White);


                    //=================================================
                    worksheet.Cells[row, 2].Value = "";
                    DrowCell(worksheet, row, 2, colGradFromHex);
                    //================================================
                    worksheet.Cells[row, 3].Value = "";
                    DrowCell(worksheet, row, 3, Color.White);
                    //================================================
                    Column = 3;
                    foreach (var course in courses)
                    {
                        Column++;                      
                        worksheet.Cells[row, Column].Value = "";
                        DrowCell(worksheet, row, Column, colGradFromHex);
                    }
                    //=================================================
                    worksheet.Cells[row, Column + 1].Value = " ";
                    DrowCell(worksheet, row, Column + 1, Color.White);


                }
                worksheet.Row(row + 1).Height = 201.75;
                worksheet.Cells[row + 1, 1, row + 1, Column + 1].Style.Font.Size = 36;
                worksheet.Cells[row + 1, 1, row + 1, Column + 1].Style.Font.Name = "Khalid Art bold";
                worksheet.Cells[row+1,1, row+1, Column + 1].Value = "مدير القبول والتسجيل                                                             نائب رئيس الفرع لشئون الطلاب                                                نائب رئيس الفرع للشئون العلمية";
                using (var r = worksheet.Cells[row + 1, 1, row + 1, Column + 1])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "بيان درجات التكميلية.xlsx");
        }


       

        private void DrowCell(OfficeOpenXml.ExcelWorksheet worksheet,int row ,int Column, Color backgroundColor)
        {
            worksheet.Cells[row, Column].Style.Font.Size = 28;
            worksheet.Cells[row , Column].Style.Font.Name = "Khalid Art bold";
            using (var r = worksheet.Cells[row, Column])
            {
                r.Style.WrapText = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(backgroundColor);

                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Top.Color.SetColor(Color.Black);

                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Left.Color.SetColor(Color.Black);

                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Right.Color.SetColor(Color.Black);

                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Bottom.Color.SetColor(Color.Black);
            }
        }
    }

   
}
