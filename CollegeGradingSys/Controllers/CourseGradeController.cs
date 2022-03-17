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
using GemBox.Spreadsheet;

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
                    var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.IsCurrentYear == IsCurrentYear).ToList();
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
            model.StPersonalDataVMs = new List<StPersonalDataVM>();
            foreach (var stPersonalData in StPersonalDatas)
            {

                model.StPersonalDataVMs.Add( new StPersonalDataVM()
                {
                    AcademicID = stPersonalData.AcademicID,
                    StName = stPersonalData.StName,
                     IsSelected= false,
                    StAcademicData = stPersonalData.StAcademicDatas
                    .OrderBy(x => x.AcademicYear.AcademicYearStart.Year)
                    .ThenBy(x => x.StLevel)
                    .ThenBy(x => x.Term).LastOrDefault(),

                });
               
            }
            
             

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
                IsCurrentYear = stAcademicData.IsCurrentYear,
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
            level ??= Level.الأول;
            term ??= Term.الأول;
            int? batchId= BatchId;
            int courseId = CourseId ?? 0;

            var model = new AllbatchCourseGradeViewModel();
            var AcademicYearsList = _AcademicYearRepository.List().OrderByDescending(x => x.AcademicYearStart).ToList();
            
            if (AcademicYearsList != null)
            {
                var academicYearId = AcademicYearId ??= AcademicYearsList[0].Id;
                ViewData["AcademicYearId"] = new SelectList(AcademicYearsList, "Id", "AcademicYearName", academicYearId);
                
                var Batchs = getBatchsOfOneAcademicYear(academicYearId);
                if (Batchs != null && Batchs.Count > 0)
                {
                     batchId = BatchId ??= Batchs[0].Id;
                    ViewData["BatchId"] = new SelectList(Batchs, "Id", "BatchName", batchId);
                    
                    
                     var courses = getCoursessOfOneBatch(batchId, level, term);
                    if (courses !=null)
                    {
                        courseId = CourseId ??= courses[0].Id;
                        ViewData["CourseId"] = new SelectList(courses, "Id", "CourseName", CourseId ?? 0);
                        model.courseName = courses.SingleOrDefault(x => x.Id == courseId).CourseName;
                    }
                    
                }

            }






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

            //ICollection<CourseGrade> courseGrades = new List<CourseGrade>();


            //if (AcademicYearId != null)
            //{
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {

            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.AcademicYear.Id == AcademicYearId).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //    //StPersonalDatas = StPersonalDatas.Where(x => x.StAcademicDatas.SingleOrDefault(x => x.AcademicYear.Id == AcademicYearId)).ToList();
            //    ViewBag.SelectedBatchId = BatchId;
            //    ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            //}
            //if (BatchId != null)
            //{
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {
            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Batch.Id == BatchId).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //    ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
            //}

            //if (level != null)
            //{
            //    model.Level = level;
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {
            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StLevel == level).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //}
            //if (term != null)
            //{
            //    model.Term = term;
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {
            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.Term == term).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //}
            //if (IsCurrentYear)
            //{
            //    model.IsCurrentYear = IsCurrentYear;
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {
            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.IsCurrentYear == IsCurrentYear).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //}
            //if (stStatus != null)
            //{
            //    model.StStatus = stStatus;
            //    IList<StPersonalData> sts = new List<StPersonalData>();
            //    foreach (var StPersonalData in StPersonalDatas)
            //    {
            //        var StAcademicDatasTemp = StPersonalData.StAcademicDatas.Where(x => x.StStatus == stStatus).ToList();
            //        if (StAcademicDatasTemp.Count > 0)
            //        {
            //            StPersonalData.StAcademicDatas = StAcademicDatasTemp;
            //            sts.Add(StPersonalData);
            //        }
            //    }
            //    StPersonalDatas = sts;
            //}


            //{

            //    //AcademicID = stAcademicData.StPersonalData.AcademicID,
            //    //StName = stAcademicData.StPersonalData.StName,
            //    //AcademicYear = stAcademicData.AcademicYear.AcademicYearName,
            //    //Batch = stAcademicData.Batch.BatchName,
            //    CourseType = _courseType,

            //    //Level = stAcademicData.StLevel.ToString(),
            //    //Specialization = stAcademicData.Batch.Specialization.SpecializationName,
            //    //StStatus = stAcademicData.StStatus.ToString(),
            //    //Term = stAcademicData.Term.ToString(),
            //    CourseGrades = courseGrades
            //};
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
            if (ModelState.IsValid)
            {
                if (model.BatchId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار العام الاكاديمي من القائمة";
                    FullAllListes("-- أختر --");
                    return View(model);
                }
                if (model.AcademicYearId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار الدفعة من القائمة";
                    FullAllListes("-- أختر --");
                    return View(model);
                }
                var stPersonalData = _StPersonalDataRepository.Find(model.AcademicID);
                var studentBatch = _BatchRepository.Find(model.BatchId);
                var academicYear = _AcademicYearRepository.Find(model.AcademicYearId);
               
                if (!model.IsCurrentYear && model.StStatus == StStatus.مقيد )
                {
                    if(model.Average == null || model.GPA == null || model.Valuation == Valuation.غير_محدد)
                    {
                        ViewBag.Message = "الرجاء اكمال ادخال البيانات او اختيار الفصل الحالي";
                        FullAllListes("-- أختر --");
                        return View(model);
                    }
                }
                var StAcademicDataList = _StAcademicDataRepository.List();

                var checkStAcademicDataF = StAcademicDataList
                .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.AcademicYear.Id == model.AcademicYearId && x.Term == model.Term);

                if (checkStAcademicDataF)
                {
                    ViewBag.Message = "لقد تم إيجاد بيانات سابقة بنفس العام الاكادمي و الفصل الدراسي للطالب .. الرجاء اختيار عام أخر او اختيار فصل دراسي آخر";
                    FullAllListes("-- أختر --");
                    return View(model);
                }
                var checkStAcademicDataS = StAcademicDataList
                .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.StLevel == model.StLevel && x.Term == model.Term);

                if (checkStAcademicDataS)
                {
                    ViewBag.Message = "لقد تم إيجاد بيانات سابقة بنفس المستوى و الفصل الدراسي للطالب .. الرجاء اختيار مستوى أخر او اختيار فصل دراسي آخر";
                    FullAllListes("-- أختر --");
                    return View(model);
                }

                var checkStAcademicDatath = StAcademicDataList
              .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.IsCurrentYear == true );

                if (checkStAcademicDatath && model.IsCurrentYear == true )
                {
                    ViewBag.Message = " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. الرجاء تعين فصل واحد فقط كفصل حالي للطالب  ";
                    FullAllListes("-- أختر --");
                    return View(model);
                }
                var stAcademicData = new StAcademicData()
                {
                    StLevel = model.StLevel,
                    Term = model.Term,
                    StStatus = model.StStatus,
                    Average = model.Average,
                    GPA = model.GPA,
                    Valuation = model.Valuation,
                    IsCurrentYear = model.IsCurrentYear,
                    StPersonalData = stPersonalData,
                    AcademicYear = academicYear,
                    Batch = studentBatch
                };

                _StAcademicDataRepository.Add(stAcademicData);               
                return RedirectToAction(nameof(AllStCourseGrade), new { id =model.AcademicID });
            }
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
                    if (Grade is not null)
                    {
                        var cultureInfo = new CultureInfo("en");
                        if (!(decimal.TryParse(Grade,
                            NumberStyles.AllowDecimalPoint,
                            cultureInfo, out var modelGrade)) || !(modelGrade >= 0 && modelGrade <= 100))
                        {                            
                                ModelState.AddModelError(nameof(model.Grade)," يجب إدخال  درجة المادة رقماً بين 0 - 100.");

                                return PartialView("_Edit", model);                           
                        }
                        newGrade = (float)modelGrade;
                        NewStStatus = newGrade >= model.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
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

        // GET: StAcademicData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            

            return View();
        }

        // POST: StAcademicData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            return RedirectToAction(nameof(Index));
        }

        



        List<Batch> FillSelectBatchsList(string batchName)
        {
            var Batches = _BatchRepository.List().ToList();
            Batches.Insert(0, new Batch { Id = -1, BatchName = batchName  });

            return Batches;
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
       

        public JsonResult GetCoursess(int? batchId, Level level, Term term)
        {
            if (batchId == null)
            {
                return null;
            }
            var CoursessList = getCoursessOfOneBatch(batchId, level, term);
            return Json(new SelectList(CoursessList, "Id", "CourseName"));
        }
        
    }
}
