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
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using OfficeOpenXml.Style;

namespace CollegeGradingSys.Controllers
{
    public class StAcademicDataController : Controller
    {       
        private readonly ICollegeGradingSysRepository<StAcademicData> _StAcademicDataRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<Batch> _BatchRepository;
        private readonly ICollegeGradingSysRepository<AcademicYear> _AcademicYearRepository;
        private readonly ICollegeGradingSysRepository<Specialization> _SpecializationRepository;
        private readonly ICollegeGradingSysRepository<CourseGrade> _courseGradeRepository;
        private readonly ICollegeGradingSysRepository<Course> _courseRepository;

        public StAcademicDataController(ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository
            , ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository
            , ICollegeGradingSysRepository<Batch> BatchRepository
            , ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository
            ,ICollegeGradingSysRepository<Specialization> SpecializationRepository
            ,ICollegeGradingSysRepository<CourseGrade> CourseGradeRepository
            ,ICollegeGradingSysRepository<Course> CourseRepository)
        {
            _StAcademicDataRepository = StAcademicDataRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
            _BatchRepository = BatchRepository;
            _AcademicYearRepository = AcademicYearRepository;
            _SpecializationRepository = SpecializationRepository;
            _courseGradeRepository = CourseGradeRepository;
            _courseRepository = CourseRepository;
        }

        // GET: StAcademicData
        public IActionResult Index( string Message, string sortOrder, string currentFilter, bool IsSelectCurrentYear, string StNameSearch, int? BatchId, int? AcademicYearId, StStatus? stStatus, Term? term,Level? level, StudyType? studyType, bool IsCurrentYear, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        {
            ViewBag.Message = Message;
            FullAllListes("-- الكل --");
            var model = new StAcademicDataIndexViewModel();
           
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (StNameSearch != null)
            {

            }
            else
            {
                StNameSearch = currentFilter;
            }

            ViewBag.CurrentFilter = StNameSearch;
            var currentYear = GetCurrentYear();


            if (IsSelectCurrentYear == true)
            {
               
                if (currentYear !=null)
                {                   
                    AcademicYearId = currentYear.Id;
                   
                }                
            }


            //========================================
           
           

            //========================================

            //var StPersonalDatas = _StPersonalDataRepository.List();

            IList<StAcademicData> stAcademicData = new List<StAcademicData>();
          
            if (AcademicYearId != null)
            {
                ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName", AcademicYearId ?? 0);
                var academicYear = _AcademicYearRepository.Find(AcademicYearId ?? 0);
                model.IsCurrentYear = academicYear.IsCurrentYear;

                var StAcademicDataOfAcademicYear = _StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == AcademicYearId).ToList();
                if (StAcademicDataOfAcademicYear != null)
                {
                    stAcademicData = StAcademicDataOfAcademicYear;
                }               
               
            }
            if (!String.IsNullOrEmpty(StNameSearch))
            {
                stAcademicData = stAcademicData.Where(s => s.StPersonalData.StName.Contains(StNameSearch)).ToList();
            }
            if (BatchId != null)
            {
                
                var BatchOfStAcademicData = stAcademicData.Where(x => x.Batch.Id == BatchId).ToList();
                if (BatchOfStAcademicData != null)
                {
                    stAcademicData = BatchOfStAcademicData;
                }               
                            
                ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
            }


            if (term == null)
            {
                term = Term.الأول;
            }
                model.Term = term;
                var TermOfStAcademicData = stAcademicData.Where(x => x.Term == term).ToList();
                if (TermOfStAcademicData != null)
                {
                    stAcademicData = TermOfStAcademicData;
                }
               
          
          
           
            if (stStatus != null)
            {
                model.StStatus = stStatus;
                IList<StPersonalData> sts = new List<StPersonalData>();
                var StStatusOfStAcademicData = stAcademicData.Where(x => x.StStatus == stStatus).ToList();
                if (StStatusOfStAcademicData != null)
                {
                    stAcademicData = StStatusOfStAcademicData;
                }
            }

            if (studyType != null)
            {
                model.StudyType = studyType;               
                var StStatusOfStAcademicData = stAcademicData.Where(x => x.StudyType == studyType).ToList();
                if (StStatusOfStAcademicData != null)
                {
                    stAcademicData = StStatusOfStAcademicData;
                }
            }

            model.StAcademicDataVMs = new List<StAcademicDataVM>();
            foreach (var  stA in stAcademicData)
            {

                model.StAcademicDataVMs.Add( new StAcademicDataVM()
                {
                    Id = stA.Id,
                    AcademicYear = stA.AcademicYear,
                    Average = stA.Average,
                    Batch = stA.Batch,
                    GPA = stA.GPA,
                    IsCurrentYear = stA.IsTerm,
                    StLevel = stA.StLevel,
                    StStatus = stA.StStatus,
                    StudyType = stA.StudyType,
                    Term = stA.Term,
                    Valuation = stA.Valuation,
                    StPersonalData = stA.StPersonalData,
                    IsSelected = false
                });
               
            }
            
             

            return View(model);
        }

       
        

        public IActionResult AllStAcademicDatas (int id)
        {


            //ViewBag.Message = "يجب اقفال الفصل  بإدخال جميع درجات المواد";
            var stPersonalData = _StPersonalDataRepository.Find(id);
            
            IList<StAcademicData> stAcademicDatas =new List<StAcademicData>();
            if (id != null)
            {
                stAcademicDatas = _StAcademicDataRepository.List().Where(s => s.StPersonalData.AcademicID.Equals(id))
                    .OrderBy(x => x.AcademicYear.AcademicYearStart.Year)
                    .ThenBy(x => x.StLevel)
                    .ThenBy(x => x.Term)
                    .ToList();
                
            }
            var model = new StAcademicDataDataViewModel()
            {
                Id = id,
                AcademicID = stPersonalData.AcademicID,
                StName = stPersonalData.StName,
                IsCanRegisterInCurrentYear = false,
                StAcademicDatas = stAcademicDatas                
            };
            var currentYear = GetCurrentYear();
            var stACourseGrades = stAcademicDatas.Where(x => x.AcademicYear.Id == currentYear.Id).ToList();
            if(stACourseGrades == null || stACourseGrades.Count <= 0)
            {

                var isSTwithdrew = stAcademicDatas.Any(x => x.StStatus == StStatus.منحسب);
                model.IsCanRegisterInCurrentYear = !isSTwithdrew;
               
            }
            
                
            
           
            return View(model);
        }

        // GET: StAcademicData/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var stAcademicData = await _context.StAcademicData
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (stAcademicData == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(stAcademicData);
        //}


        public IActionResult NextLevel(StAcademicDataIndexViewModel model)
        {

            if (model != null)
            {
                FullAllListes("-- أختر --");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NextLevel([Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear,AcademicID,AcademicYearId,BatchId")] CreateStAcademicDataDataViewModel model)
        {
            return RedirectToAction(nameof(Index));
        }



        // GET: StAcademicData/Create
        public IActionResult Create(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stPersonalData = _StPersonalDataRepository.Find(id);
            if (stPersonalData == null)
            {
                return NotFound();
            }
            FullAllListes("-- أختر --");
            var model = new CreateStAcademicDataDataViewModel()
            {
                StName = stPersonalData.StName,
                AcademicID = stPersonalData.AcademicID,
                AcademicYearId = GetCurrentYear().Id
            };
            var StAcademicDatalistTerm = _StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == id).LastOrDefault();
            Level nextlevel = GetNextStAcademicDataLevel(StAcademicDatalistTerm);
            
         
            
            if(StAcademicDatalistTerm !=null)
            {
                model.BatchId = StAcademicDatalistTerm.StStatus == StStatus.ناجح ? StAcademicDatalistTerm.Batch.Id: -1;
                model.StLevel = nextlevel;
                model.preAcademicYear = StAcademicDatalistTerm.AcademicYear.AcademicYearName;
                model.preStStatus = StAcademicDatalistTerm.StStatus.ToString();
                model.preGPA = StAcademicDatalistTerm.GPA.ToString();
                model.preSpecialization = StAcademicDatalistTerm.Batch.Specialization.SpecializationName;
                model.preValuation = StAcademicDatalistTerm.Valuation.ToString();
                model.preLevel = StAcademicDatalistTerm.StLevel.ToString();
            }
          
           
           
            return View(model);
        }

       
            // POST: StAcademicData/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateStAcademicDataDataViewModel model)
        {
            ModelState.ClearValidationState(nameof(model));
            if (model.BatchId == -1)
            {
                ModelState.AddModelError(nameof(model.BatchId), "الرجاء اختيار الدفعة من القائمة");
                   
            }
            if (!TryValidateModel(model, nameof(model)))
            {
                FullAllListes("-- أختر --");
                return View(model);
            }


            try
            {
                var stPersonalData = _StPersonalDataRepository.Find(model.AcademicID);
                var studentBatch = _BatchRepository.Find(model.BatchId);
                var academicYear = GetCurrentYear(); 
                
                //var stAcademicData = new StAcademicData()
                //{
                //    StLevel = model.StLevel,
                //    //Term = model.Term,
                //    StStatus =  StStatus.مقيد,
                //    //Average = model.Average,
                //    //GPA = model.GPA,
                //     StudyType = model.StudyType,
                   
                //    //IsTerm = model.IsCurrentYear,
                //    StPersonalData = stPersonalData,
                //    AcademicYear = academicYear,
                //    Batch = studentBatch
                //};
                

                //_StAcademicDataRepository.Add(stAcademicData);
                //var stAcCourses = _courseRepository.List().Where(x => x.Level == stAcademicData.StLevel).Where(x => x.Term == stAcademicData.Term).Where(x => x.Specialization == stAcademicData.Batch.Specialization).ToList();
                //foreach (var course in stAcCourses)
                //{

                //    var courseGrade = new CourseGrade()
                //    {
                //        Course = course,
                //        CourseType = true,
                //        StAcademicData = stAcademicData,
                //        StStatusForCourse = StStatusForCourse.غير_محدد,
                //    };
                //    _courseGradeRepository.Add(courseGrade);
                //}
            //========================Add StAcademicData to Repository  Term.الأول,.================================

           

            var stAcademicData = new StAcademicData()
            {
                StLevel = model.StLevel,
                Term = Term.الأول,
                StStatus = StStatus.مقيد,
                Valuation = Valuation.غير_محدد,
                IsTerm = true,
                 StudyType = model.StudyType,
                StPersonalData = stPersonalData,
                AcademicYear = academicYear,
                Batch = studentBatch
            };


            _StAcademicDataRepository.Add(stAcademicData);
            //=================================================
            //===================Add CourseGrade to Repository Term.الأول,.==============================

            AddCourseGradeTostAcademicData(stAcademicData);



            //========================Add StAcademicData to Repository  Term.الثاني.================================
            var stAcademicData2 = new StAcademicData()
            {
                StLevel = model.StLevel,
                Term = Term.الثاني,
                StStatus = StStatus.مقيد,
                Valuation = Valuation.غير_محدد,
                IsTerm = true,
                StudyType = model.StudyType,
                StPersonalData = stPersonalData,
                AcademicYear = academicYear,
                Batch = studentBatch
            };

            _StAcademicDataRepository.Add(stAcademicData2);
            //===================Add CourseGrade to Repository Term.الثاني.==============================


            AddCourseGradeTostAcademicData(stAcademicData2);
                //========================================================
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction(nameof(AllStAcademicDatas), new { id =model.AcademicID });           
        }

       

        // GET: StAcademicData/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stAcademicData = _StAcademicDataRepository.Find(id ?? 0);
            if (stAcademicData == null)
            {
                return NotFound();
            }
            var stringGPA = "";
            if (stAcademicData.GPA != null)
            {
                stringGPA = stAcademicData.GPA.Value.ToString("N2", new CultureInfo("en"));
            }
            var stringAverage = "";
            if (stAcademicData.Average != null)
            {
                stringAverage = stAcademicData.Average.Value.ToString("N2", new CultureInfo("en"));
            }
            var model = new CreateStAcademicDataDataViewModel()
            {
                AcademicID = stAcademicData.StPersonalData.AcademicID,
                StLevel = stAcademicData.StLevel,
                Term = stAcademicData.Term,
                //IsCurrentYear = stAcademicData.IsTerm,
                StStatus = stAcademicData.StStatus,
                StudyType = stAcademicData.StudyType,
                AcademicYearId = stAcademicData.AcademicYear.Id,
                BatchId = stAcademicData.Batch.Id,
                GPA = stringGPA,
                Average = stringAverage,
                Valuation = stAcademicData.Valuation,
                Id = stAcademicData.Id
            };

            FullAllListes();
            return View(model);           
        }

        // POST: StAcademicData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear,AcademicID,StudyType,AcademicYearId,BatchId")] CreateStAcademicDataDataViewModel model)
        {
            FullAllListes();
            if (id != model.Id)
            {
                return NotFound();
            }
            ModelState.ClearValidationState(nameof(model));

            //var studentBatch = _BatchRepository.Find(model.BatchId);

            var stAcademicData = _StAcademicDataRepository.Find(model.Id);
            model.AcademicYearId = stAcademicData.AcademicYear.Id;
            model.BatchId = stAcademicData.Batch.Id;
            model.StLevel = stAcademicData.StLevel;
            model.Term = stAcademicData.Term;
            var courseGradesFailed = CourseGradesFailed(model.AcademicID, model.Term);

            if (model.StStatus == StStatus.متخرج)
            {
                if (model.StLevel != Level.الرابع || model.Term != Term.الثاني)
                {
                    ViewBag.Message = "عذراً .. يتم اختيار حالة الطالب 'متخرج' فقط في حالة النجاح في الفصل الثاني من المستوى الرابع   ";
                    return View(model);
                }
                if ( (!IsAllStfaildedCourseGradesSucceeded(model.AcademicID))   || (courseGradesFailed is not null && courseGradesFailed.Count() > 0))
                {
                    ViewBag.Message = "يجب النجاح في جميع المواد المقررة للطالب العامة والتكميلي";
                    return View(model);
                }

               
                
            }
            if (model.StStatus == StStatus.ناجح )
            { 
                if (model.StLevel == Level.الرابع && model.Term == Term.الثاني)
                {
                    ViewBag.Message = "في حالة النجاح في الفصل الثاني من المستوى الرابع يجب اختيار 'متخرج'  ";
                    return View(model);
                }
            }
            if (model.StStatus == StStatus.ناجح || model.StStatus == StStatus.متخرج)
            {
                if (!IsCourseGradesEntered(model.Id))
                {
                    ViewBag.Message = "يجب اقفال الفصل  بإدخال جميع درجات المواد";
                    return View(model);
                }
                
                if (model.Average is not null)
                {
                    var cultureInfo = new CultureInfo("en");
                    if (!(float.TryParse(model.Average.ToString(),
                        NumberStyles.Float,
                        cultureInfo, out var modelAverage)) || !(modelAverage >= 0 && modelAverage <= 100))
                    {
                        ModelState.AddModelError(nameof(model.Average), " الرجاء إدخال  المعدل رقماً  بين 0 - 100.");


                    }
                    else { stAcademicData.Average = modelAverage; }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Average), "الرجاء إدخال  المعدل  رقماً بين 0 - 100.");
                }
                if (model.GPA is not null)
                {
                    var cultureInfo = new CultureInfo("en");
                    if (!(float.TryParse(model.GPA.ToString(),
                        NumberStyles.Float,
                        cultureInfo, out var modelGPA)) || !(modelGPA >= 0 && modelGPA <= 100))
                    {
                        ModelState.AddModelError(nameof(model.GPA), " الرجاء إدخال  المعدل التراكمي رقماً بين 0 - 100.");


                    }
                    else { stAcademicData.GPA = modelGPA; }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.GPA), "الرجاء إدخال  المعدل التراكمي  رقماً بين 0 - 100.");
                }
                if (model.Valuation == Valuation.غير_محدد)
                {
                    ModelState.AddModelError(nameof(model.Valuation), " الرجاء تحديد التقدير من القائمة.");                    
                }
                else { stAcademicData.Valuation = model.Valuation; }
            }

            if (model.StStatus == StStatus.متخرج)
            {

            }


            if (!TryValidateModel(model, nameof(model)))
            {
                FullAllListes();
                return View(model);
            }

            stAcademicData.StudyType = model.StudyType;
            stAcademicData.StStatus = model.StStatus;          
        //    stAcademicData.Batch = studentBatch;

            if (model.StStatus == StStatus.ناجح)
            {
               
                // اضافة المواد الرسوب الى التكميلي
                foreach (var courseGradeFailed in courseGradesFailed)
                {

                    var courseGrade = new CourseGrade()
         
                    {
                        Course = courseGradeFailed.Course,
                        CourseType = false,
                        StAcademicData = stAcademicData,
                        StStatusForCourse = StStatusForCourse.غير_محدد,
                    };

                    _courseGradeRepository.Add(courseGrade);
                }
            }

            _StAcademicDataRepository.Update(id,stAcademicData);              
                

                return RedirectToAction(nameof(AllStAcademicDatas), new { id = model.AcademicID });
           
           
           
            
            //return View(stAcademicData);
        }

        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = _StAcademicDataRepository.Find(id);

            return PartialView("_Delete", course);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Course course)
        {

            try
            {

                _StAcademicDataRepository.Delete(id);
                return PartialView("_Delete", course);
            }
            catch
            {
                return PartialView("_Delete", course);
            }
        }



        // GET: AddAcademicDataForAllSts
        public IActionResult AddAcademicDataForAllSts()
        {
            //=======================  Get previousYear ================================
           
           
            var currentYear = GetCurrentYear();
            var previousYear = GetpreviousYear(currentYear.Id); 

            foreach (var StAcademicData in previousYear.StAcademicDatas)
            {
                var oldStAcademicData = _StAcademicDataRepository.Find(StAcademicData.Id);

                if (isCanAddStAcademicData(oldStAcademicData))
                { 
                    Batch studentBatch;
                    if (oldStAcademicData.StStatus == StStatus.راسب)
                    {
                        studentBatch = GetNextBatch(oldStAcademicData);
                        if (studentBatch is null)
                            return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true, Message = "عذراً...لم يتم اكمال الاجراء بسبب انه لم يتم إضافة دفعة جديدة في السنة الحالية " });
                    }
                    else
                    {
                        studentBatch = oldStAcademicData.Batch;
                    }
                   //========================Add StAcademicData to Repository  Term.الأول,.================================                   
                   Level stNextLevel = GetNextStAcademicDataLevel(oldStAcademicData);
                    var stPersonalData = oldStAcademicData.StPersonalData;
                    
                    var academicYear = currentYear;
                        var studyType = oldStAcademicData.StudyType;

                        var stAcademicData = new StAcademicData()
                        {
                            StLevel = stNextLevel,
                            Term = Term.الأول,
                            StStatus = StStatus.مقيد,
                            Valuation = Valuation.غير_محدد,
                            IsTerm = true,
                            StudyType = studyType,
                            StPersonalData = stPersonalData,
                            AcademicYear = academicYear,
                            Batch = studentBatch
                        };


                        _StAcademicDataRepository.Add(stAcademicData);
                        ////=================================================
                        ////===================Add CourseGrade to Repository Term.الأول,.==============================

                        AddCourseGradeTostAcademicData(stAcademicData);



                        ////========================Add StAcademicData to Repository  Term.الثاني.================================
                        var stAcademicData2 = new StAcademicData()
                        {
                            StLevel = stNextLevel,
                            Term = Term.الثاني,
                            StStatus = StStatus.مقيد,
                            Valuation = Valuation.غير_محدد,
                            IsTerm = true,
                            StudyType = studyType,
                            StPersonalData = stPersonalData,
                            AcademicYear = academicYear,
                            Batch = studentBatch
                        };

                        _StAcademicDataRepository.Add(stAcademicData2);
                        ////===================Add CourseGrade to Repository Term.الثاني.==============================


                        AddCourseGradeTostAcademicData(stAcademicData2);
                        //========================================================
                   

                }
            }
               
            return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true });

        }


        public ActionResult ExportGraduateStToExcel(bool IsCurrentYear ,bool IsSelectCurrentYear, int? AcademicYearId, int? BatchId, StudyType? studyType )
        {
            var StPersonalDatasR = _StPersonalDataRepository.List();

            var currentYear = GetCurrentYear();


            if (IsSelectCurrentYear == true)
            {

                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;

                }
            }
            IList<StAcademicData> stAcademicDataList = new List<StAcademicData>();

            stAcademicDataList = GetStAcademicDatas(IsCurrentYear, IsSelectCurrentYear, AcademicYearId, BatchId, studyType,StStatus.متخرج , Term.الثاني);

            //if (AcademicYearId != null)
            //{
            //    ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName", AcademicYearId ?? 0);
            //    var academicYear = _AcademicYearRepository.Find(AcademicYearId ?? 0);
            //    //model.IsCurrentYear = academicYear.IsCurrentYear;

            //    var StAcademicDataOfAcademicYear = _StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == AcademicYearId).ToList();
            //    if (StAcademicDataOfAcademicYear != null)
            //    {
            //        stAcademicDataList = StAcademicDataOfAcademicYear;
            //    }

            //}
            //if (BatchId != null)
            //{

            //    var BatchOfStAcademicData = stAcademicDataList.Where(x => x.Batch.Id == BatchId).ToList();
            //    if (BatchOfStAcademicData != null)
            //    {
            //        stAcademicDataList = BatchOfStAcademicData;
            //    }

            //    ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
            //}

            
               
            //    var StStatusOfStAcademicData = stAcademicDataList.Where(x => x.StStatus == StStatus.متخرج).ToList();
            //    if (StStatusOfStAcademicData != null)
                //{
                //    stAcademicDataList = StStatusOfStAcademicData;
                //}
            

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            //ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            //if (AcademicYearId != null)
            //{
            //    StPersonalDatasR = StPersonalDatasR.Where(x => x.EnrollmentYear.Id == AcademicYearId).ToList();
            //}
            // Get the user list 
            //var users = GetlistOfUsers();

            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("(1)");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;

                worksheet.Column(1).Width = 4.57;
                worksheet.Column(2).Width = 37.57;
                worksheet.Column(3).Width = 10.57;
                worksheet.Column(4).Width = 13;
                worksheet.Column(5).Width = 18;
                worksheet.Column(6).Width = 16.57;
                worksheet.Column(7).Width = 21.29;
                worksheet.Column(8).Width = 16.29;
                worksheet.Column(9).Width = 15.29;
                worksheet.Column(10).Width = 12.29;
                worksheet.Column(11).Width = 15.29;
                worksheet.Column(12).Width = 10;
                worksheet.Column(13).Width = 13;
                worksheet.Column(14).Width = 9.57;
                worksheet.Column(15).Width = 9;
                worksheet.Column(16).Width = 9.43;
                worksheet.Column(17).Width = 14.71;
                worksheet.Column(18).Width = 19;
                worksheet.Column(19).Width = 22.14;
                //==========================
                worksheet.Row(1).Height = 24.75;
                worksheet.Row(2).Height = 30.25;
                worksheet.Row(3).Height = 18.75;
                worksheet.Row(4).Height = 53.5;            
               
                //============================
                var academicYear = GetCurrentYear();
                worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");
                Color PinkFromHex = System.Drawing.ColorTranslator.FromHtml("#F2DCDB");
                
               
               
                //================================
                worksheet.Cells["A1"].Value = "اسم الجامعة";
                using (var r = worksheet.Cells["A1:E1"])
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
                worksheet.Cells["F1"].Value = "للعام الدراسي";
                using (var r = worksheet.Cells["F1:N1"])
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
                worksheet.Cells["O1"].Value = "Name Of University";
                using (var r = worksheet.Cells["O1:S1"])
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
                worksheet.Cells["A2"].Value = "جامعة الإيمان فرع حضرموت";
                using (var r = worksheet.Cells["A2:E2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.None;
                    r.Style.Border.Left.Style = ExcelBorderStyle.None;
                    r.Style.Border.Right.Style = ExcelBorderStyle.None;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                //================================
                worksheet.Cells["F2"].Value = academicYear.AcademicYearName + "  " + academicYear.AcademicYearNameH;
                using (var r = worksheet.Cells["F2:N2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.None;
                    r.Style.Border.Left.Style = ExcelBorderStyle.None;
                    r.Style.Border.Right.Style = ExcelBorderStyle.None;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                //================================
                worksheet.Cells["O2"].Value = "";
                using (var r = worksheet.Cells["O2:S2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.None;
                    r.Style.Border.Left.Style = ExcelBorderStyle.None;
                    r.Style.Border.Right.Style = ExcelBorderStyle.None;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                //================================
                worksheet.Cells["A3"].Value = "م";
                using (var r = worksheet.Cells["A3:A4"])
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
                worksheet.Cells["B3"].Value = "اسم الطالب";
                using (var r = worksheet.Cells["B3:B4"])
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
                worksheet.Cells["C3"].Value = "الجنس";
                using (var r = worksheet.Cells["C3:C4"])
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
                worksheet.Cells["D3"].Value = "الجنسية";
                using (var r = worksheet.Cells["D3:D4"])
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
                worksheet.Cells["E3"].Value = "محل للميلاد";
                using (var r = worksheet.Cells["E3:E4"])
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
                worksheet.Cells["F3"].Value = "تاريخ الميلاد";
                using (var r = worksheet.Cells["F3:F4"])
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
                worksheet.Cells["G3"].Value = "الرقم الوطني (الهوية)";
                using (var r = worksheet.Cells["G3:G4"])
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
                worksheet.Cells["H3"].Value = "رقم القيد";
                using (var r = worksheet.Cells["H3:H4"])
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
                worksheet.Cells["I3"].Value = "اسم الكلية";
                using (var r = worksheet.Cells["I3:I4"])
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
                worksheet.Cells["J3"].Value = "اسم القسم";
                using (var r = worksheet.Cells["J3:J4"])
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
                worksheet.Cells["K3"].Value = "التخصص";
                using (var r = worksheet.Cells["K3:K4"])
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
                worksheet.Cells["L3"].Value = "عام الالتحاق";
                using (var r = worksheet.Cells["L3:L4"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["M3"].Value = "النسبة المئوية للتخرج %";
                using (var r = worksheet.Cells["M3:M4"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["N3"].Value = "التقدير العام";
                using (var r = worksheet.Cells["N3:N4"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["O3"].Value = "نظام الدراسة";
                using (var r = worksheet.Cells["O3:O4"])
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
                worksheet.Cells["P3"].Value = "بيانات المؤهل السابق";
                using (var r = worksheet.Cells["P3:S3"])
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
                worksheet.Cells["p4"].Value = "نوعه";

                worksheet.Cells["p4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["p4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["p4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["p4"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["p4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["p4"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["p4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["p4"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["p4"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["p4"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                //================================
                worksheet.Cells["Q4"].Value = "تاريخ الحصول عليه";
                worksheet.Cells["Q4"].Style.WrapText = true;
                worksheet.Cells["Q4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["Q4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["Q4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["Q4"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["Q4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["Q4"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["Q4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["Q4"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["Q4"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["Q4"].Style.Border.Bottom.Color.SetColor(BrownFromHex);


                //================================
                worksheet.Cells["R4"].Value = "جهة الحصول عليه";
                worksheet.Cells["R4"].Style.WrapText = true;
                worksheet.Cells["R4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["R4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["R4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["R4"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["R4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["R4"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["R4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["R4"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["R4"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["R4"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                //================================
                worksheet.Cells["S4"].Value = "النسبة المئوية %";

                worksheet.Cells["S4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["S4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["S4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["S4"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["S4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["S4"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["S4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["S4"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["S4"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["S4"].Style.Border.Bottom.Color.SetColor(BrownFromHex);


                row = 5;
                var no = 1;
                foreach (var stAcademicData in stAcademicDataList)
                {
                    

                    worksheet.Cells[row, 1].Value = no;
                    worksheet.Cells[row, 2].Value = stAcademicData.StPersonalData.StName;
                    worksheet.Cells[row, 3].Value = stAcademicData.StPersonalData.Sex;
                    worksheet.Cells[row, 4].Value = stAcademicData.StPersonalData.Nationality.CountryName;
                    worksheet.Cells[row, 5].Value = stAcademicData.StPersonalData.BirthGovernorate.GovernorateName;
                    worksheet.Cells[row, 6].Value = stAcademicData.StPersonalData.BirthDate.Date.ToString("d");
                    worksheet.Cells[row, 7].Value = stAcademicData.StPersonalData.IdentificatioNO;
                    worksheet.Cells[row, 8].Value = stAcademicData.StPersonalData.AcademicID;
                    worksheet.Cells[row, 9].Value = stAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 10].Value = stAcademicData.Batch.Specialization.Department.DepartmentName;
                    worksheet.Cells[row, 11].Value = stAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 12].Value = stAcademicData.StPersonalData.EnrollmentYear.AcademicYearName;
                    worksheet.Cells[row, 13].Value = stAcademicData.GPA;
                    worksheet.Cells[row, 14].Value = stAcademicData.Valuation.ToString();

                    worksheet.Cells[row, 15].Value = stAcademicData.StudyType;
                    if (stAcademicData.StPersonalData.StHighSchoolData != null)
                    {
                        worksheet.Cells[row, 16].Value = stAcademicData.StPersonalData.StHighSchoolData.CertificateType;
                        worksheet.Cells[row, 17].Value = stAcademicData.StPersonalData.StHighSchoolData.CertificateYear;
                        worksheet.Cells[row, 18].Value = stAcademicData.StPersonalData.StHighSchoolData.Source;
                        worksheet.Cells[row, 19].Value = stAcademicData.StPersonalData.StHighSchoolData.Average;
                    }
                    else
                    {
                        worksheet.Cells[row, 16].Value = "";
                        worksheet.Cells[row, 17].Value = "";
                        worksheet.Cells[row, 18].Value = "";
                        worksheet.Cells[row, 19].Value = "";
                    }


                    string modelRange = "A" + row.ToString() + ":S" + row.ToString();
                    var modelTable = worksheet.Cells[modelRange];
                    worksheet.Row(row).Height = 42.5;
                    modelTable.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    modelTable.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Color.SetColor(Color.Black);

                     modelRange = "L" + row.ToString() + ":N" + row.ToString();
                     modelTable = worksheet.Cells[modelRange];
                    modelTable.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    modelTable.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    //worksheet.Cells[row, 17].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, 17].Style.Border.Left.Color.SetColor(Color.Black);


                    row++;
                    no++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "Ameen Bashaaib";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "كشف الخريجين.xlsx");
        }
        //==============================================
        public ActionResult ExportStAcademicDataToExcel(bool IsCurrentYear, bool IsSelectCurrentYear, int? AcademicYearId, int? BatchId, StudyType? studyType)
        {
            var StPersonalDatasR = _StPersonalDataRepository.List();

            if (IsSelectCurrentYear == true)
            {
                var currentYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;
                }

            }
            ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            
            if (AcademicYearId != null)
            {
                ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName", AcademicYearId ?? 0);
                StPersonalDatasR = StPersonalDatasR.Where(x => x.EnrollmentYear.Id == AcademicYearId).ToList();
            }
            // Get the user list 
            //var users = GetlistOfUsers();

            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("(1)");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 8;
                var row = startRow;

                worksheet.Column(1).Width = 6.71;
                worksheet.Column(2).Width = 50.29;
                worksheet.Column(3).Width = 12.71;
                worksheet.Column(4).Width = 15.29;
                worksheet.Column(5).Width = 26.57;
                worksheet.Column(6).Width = 17.57;
                worksheet.Column(7).Width = 27.86;
                worksheet.Column(8).Width = 20.86;
                worksheet.Column(9).Width = 18;
                worksheet.Column(10).Width = 17.29;
                worksheet.Column(11).Width = 17.71;
                worksheet.Column(12).Width = 20.57;
                worksheet.Column(13).Width = 14.43;
                worksheet.Column(14).Width = 10.71;
                worksheet.Column(15).Width = 14.86;
                worksheet.Column(16).Width = 11;
                worksheet.Column(17).Width = 15.86;
                worksheet.Column(18).Width = 22.43;
                worksheet.Column(19).Width = 17.29;               
                //==========================
                worksheet.Row(1).Height = 25;
                worksheet.Row(2).Height = 25;
                worksheet.Row(3).Height = 25;               
                worksheet.Row(4).Height = 30;
                worksheet.Row(5).Height = 30;
                worksheet.Row(6).Height = 30;
                worksheet.Row(7).Height = 55;



                //Create Headers and format them
                worksheet.Cells["B1"].Value = "الجمهورية اليمنية";
                worksheet.Cells["B2"].Value = "وزارة التعليم العالي والبحث العلمي";
                worksheet.Cells["B3"].Value = "قطاع الشؤون التعليمية";
                using (var r = worksheet.Cells["B1:B3"])
                {                   
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
                //=============================
                worksheet.Cells["A4:P7"].Style.Font.Size = 22;
                //=============================
                worksheet.Cells["E2"].Value = "كشف الطلاب المقيدين للعام الدراسي:";
                using (var r = worksheet.Cells["E2:H2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //============================
                var academicYear = GetCurrentYear();
                worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");
                Color PinkFromHex = System.Drawing.ColorTranslator.FromHtml("#F2DCDB");
                using (var r = worksheet.Cells["I2:J2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["M2"].Value = "نظام الدراسة :";
                using (var r = worksheet.Cells["M2:M2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //===============================================               
                worksheet.Cells["N2"].Value = studyType.ToString() ?? "الكل";
                using (var r = worksheet.Cells["N2:N2"])
                {                   
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["A4"].Value = "اسم الجامعة";
                using (var r = worksheet.Cells["A4:E4"])
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
                worksheet.Cells["F4"].Value = "للعام الدراسي";
                using (var r = worksheet.Cells["F4:K4"])
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
                worksheet.Cells["L4"].Value = "Name Of University";
                using (var r = worksheet.Cells["L4:R4"])
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
                using (var r = worksheet.Cells["A5:E5"])
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
                worksheet.Cells["F5"].Value = academicYear.AcademicYearName + "  " + academicYear.AcademicYearNameH;
                using (var r = worksheet.Cells["F5:K5"])
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
                worksheet.Cells["L5"].Value = "";
                using (var r = worksheet.Cells["L5:R5"])
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
                    //r.Style.TextRotation = 90;
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
                worksheet.Cells["L6"].Value = "عام الالتحاق";
                using (var r = worksheet.Cells["L6:L7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["M6"].Value = "المستوى الحالي";
                using (var r = worksheet.Cells["M6:M7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(PinkFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
               
                //================================
                worksheet.Cells["N6"].Value = "نظام الدراسة";
                using (var r = worksheet.Cells["N6:N7"])
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
                worksheet.Cells["O6"].Value = "بيانات المؤهل السابق";
                using (var r = worksheet.Cells["O6:R6"])
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
                worksheet.Cells["O7"].Value = "نوعه";             

                //================================
                worksheet.Cells["P7"].Value = "تاريخ الحصول عليه";          


                //================================
                worksheet.Cells["Q7"].Value = "جهة الحصول عليه";
               
              

                //================================
                worksheet.Cells["R7"].Value = "النسبة المئوية %";

               
                

                using (var r = worksheet.Cells["O7:R7"])
                {
                   
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
                worksheet.Cells["S6"].Value = "ملاحظات";
                using (var r = worksheet.Cells["S6:S7"])
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


                    row = 8;
                var no = 1;
                IList<StAcademicData> stAcademicDataList = new List<StAcademicData>();

                stAcademicDataList = GetStAcademicDatas(IsCurrentYear, IsSelectCurrentYear, AcademicYearId, BatchId, studyType, StStatus.مقيد , Term.الأول);
                foreach (var stAcademicData in stAcademicDataList)
                {


                    worksheet.Cells[row, 1].Value = no;
                    worksheet.Cells[row, 2].Value = stAcademicData.StPersonalData.StName;
                    worksheet.Cells[row, 3].Value = stAcademicData.StPersonalData.Sex;
                    worksheet.Cells[row, 4].Value = stAcademicData.StPersonalData.Nationality.CountryName;
                    worksheet.Cells[row, 5].Value = stAcademicData.StPersonalData.BirthGovernorate.GovernorateName;
                    worksheet.Cells[row, 6].Value = stAcademicData.StPersonalData.BirthDate.Date.ToString("d");
                    worksheet.Cells[row, 7].Value = stAcademicData.StPersonalData.IdentificatioNO;
                    worksheet.Cells[row, 8].Value = stAcademicData.StPersonalData.AcademicID;
                    worksheet.Cells[row, 9].Value = stAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 10].Value = stAcademicData.Batch.Specialization.Department.DepartmentName;
                    worksheet.Cells[row, 11].Value = stAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 12].Value = stAcademicData.StPersonalData.EnrollmentYear.AcademicYearName;
                    worksheet.Cells[row, 13].Value = stAcademicData.StLevel;
                   
                    worksheet.Cells[row, 14].Value = stAcademicData.StudyType;
                    if (stAcademicData.StPersonalData.StHighSchoolData != null)
                    {
                        worksheet.Cells[row, 15].Value = stAcademicData.StPersonalData.StHighSchoolData.CertificateType;
                        worksheet.Cells[row, 16].Value = stAcademicData.StPersonalData.StHighSchoolData.CertificateYear;
                        worksheet.Cells[row, 17].Value = stAcademicData.StPersonalData.StHighSchoolData.Source;
                        worksheet.Cells[row, 18].Value = stAcademicData.StPersonalData.StHighSchoolData.Average;
                        worksheet.Cells[row, 19].Value = "";
                    }
                    else
                    {
                        worksheet.Cells[row, 15].Value = "";
                        worksheet.Cells[row, 16].Value = "";
                        worksheet.Cells[row, 17].Value = "";
                        worksheet.Cells[row, 18].Value = "";
                        worksheet.Cells[row, 19].Value = "";
                    }


                    string modelRange = "A" + row.ToString() + ":S" + row.ToString();
                    var modelTable = worksheet.Cells[modelRange];
                    worksheet.Row(row).Height = 42.5;
                    modelTable.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    modelTable.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Color.SetColor(Color.Black);

                    modelRange = "L" + row.ToString() + ":M" + row.ToString();
                    modelTable = worksheet.Cells[modelRange];
                    modelTable.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    modelTable.Style.Fill.BackgroundColor.SetColor(PinkFromHex);

                    modelRange = "O" + row.ToString() + ":R" + row.ToString();
                    modelTable = worksheet.Cells[modelRange];
                    modelTable.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    modelTable.Style.Fill.BackgroundColor.SetColor(colGradFromHex);


                    //worksheet.Cells[row, 17].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, 17].Style.Border.Left.Color.SetColor(Color.Black);


                    row++;
                    no++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "Ameen Bashaaib";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "كشف المقيدين.xlsx");
        }
        //==============================================
        private bool StAcademicDataExists(int id)
        {
            return _StAcademicDataRepository.List().Any(e => e.Id == id);
        }



        List<Batch> FillSelectBatchsList(string batchName)
        {
            var Batches = _BatchRepository.List().ToList();
            Batches.Insert(0, new Batch { Id = -1, BatchName = batchName  });

            return Batches;
        }

        List<AcademicYear> FillSelectAcademicYearesList()
        {
            List<AcademicYear> AcademicYeares = _AcademicYearRepository.List().ToList();   

            return AcademicYeares;
        }

        private void FullAllListes(string text)
        {
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName");
            ViewData["BatchId"] = new SelectList(FillSelectBatchsList(text), "Id", "BatchName");

            ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationesList(text), "Id", "SpecializationName");
        }

        private void FullAllListes()
        {
            ViewData["AcademicYearId"] = new SelectList(_AcademicYearRepository.List().ToList(), "Id", "AcademicYearName");
            ViewData["BatchId"] = new SelectList(_BatchRepository.List().ToList(), "Id", "BatchName");

            ViewData["SpecializationId"] = new SelectList(_SpecializationRepository.List().ToList(), "Id", "SpecializationName");
        }

        List<Specialization> FillSelectSpecializationesList(string specializationName)
        {
            var specializationes = _SpecializationRepository.List().ToList();
            specializationes.Insert(0, new Specialization { Id = -1,  SpecializationName = specializationName });

            return specializationes;
        }


        ErrorResult CheckModelErrors(CreateStAcademicDataDataViewModel model)
        {
            
            if (model.BatchId == -1)
            {                               
                return (GetErrorResult(true, "الرجاء اختيار الدفعة من القائمة"));
            }
           
           
            //var StAcademicDataList = _StAcademicDataRepository.List();

            //var checkStAcademicDataF = StAcademicDataList
            //.Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.AcademicYear.Id == model.AcademicYearId && x.Term == model.Term && x.Id != model.Id);

            //if (checkStAcademicDataF)
            //{
            //    return (GetErrorResult(true, "لقد تم إيجاد بيانات سابقة بنفس العام الاكادمي " +
            //        "و الفصل الدراسي للطالب .. الرجاء اختيار عام أخر او اختيار فصل دراسي آخر"));
            //}
            //var checkStAcademicDataS = StAcademicDataList
            //    .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.StLevel == model.StLevel && x.Term == model.Term && x.Id != model.Id);

            //if (checkStAcademicDataS)
            //{                
            //    return (GetErrorResult(true, "لقد تم إيجاد بيانات سابقة بنفس المستوى و الفصل الدراسي للطالب .." +
            //        " الرجاء اختيار مستوى أخر او اختيار فصل دراسي آخر"));
            //}

            //var checkStAcademicDatath = StAcademicDataList
            // .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.IsCurrentYear == true && x.Id != model.Id);

            //if (checkStAcademicDatath && model.IsCurrentYear == true)
            //{
            //    ViewBag.Message = " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. الرجاء تعين فصل واحد فقط كفصل حالي للطالب  ";
                
            //    return (GetErrorResult(true, " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. " +
            //        "الرجاء تعين فصل واحد فقط كفصل حالي للطالب  "));
            //}

            return (GetErrorResult(false,""));
        }

        private bool IsCourseGradesEntered(int StAcademicDataId)
        {
            var StA = _StAcademicDataRepository.Find(StAcademicDataId);
            var stAs = StA.CourseGrades.Where(x => x.StStatusForCourse == StStatusForCourse.غير_محدد && x.CourseType == true).ToList();
            if (stAs != null)
            {
                if (stAs.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAllStfaildedCourseGradesSucceeded(int academicID)
        {

            var StAList = _StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == academicID).ToList() ;

            foreach (var StA in StAList)
            {
               

                var isAllStCourseGradesSucceeded = StA.CourseGrades.Where(y => y.CourseType == false).All(x => x.StStatusForCourse == StStatusForCourse.ناجح);
                if (!isAllStCourseGradesSucceeded)
                {
                    return false;
                }
            }          
            return true;
        }

        private bool IsCourseGradefaildIsExist(CourseGrade courseGrade)
        {
            var StA = _StAcademicDataRepository.Find(courseGrade.Id);
            var stAs = StA.CourseGrades.Where(x => x.StStatusForCourse == StStatusForCourse.غير_محدد && x.CourseType == true).ToList();
            if (stAs != null)
            {
                if (stAs.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private List<CourseGrade> CourseGradesFailed(int AcademicId,Term? term)
        {
            var stAs = _StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == AcademicId);
            List<CourseGrade> AllcourseGradesFailed = new();
            foreach (var StA in stAs)
            {
                var CourseGradesFailedForStAcademicData = StA.CourseGrades.Where(x => x.StStatusForCourse != StStatusForCourse.ناجح && x.StStatusForCourse != StStatusForCourse.غير_محدد && x.Course.IsSubCourse == false && x.Course.Term == term ).ToList();
                foreach (var CourseGradeFailed in CourseGradesFailedForStAcademicData)
                {
                    var IsCourseGradesFailedExit = StA.CourseGrades.Any(x => x.StAcademicData.StPersonalData.AcademicID == CourseGradeFailed.StAcademicData.StPersonalData.AcademicID && x.Course.Id == CourseGradeFailed.Course.Id && x.CourseType == false);
                    if (CourseGradeFailed is not null && !IsCourseGradesFailedExit)
                    {
                        AllcourseGradesFailed.Add(CourseGradeFailed);
                    }
                }

            }
            
           
            return AllcourseGradesFailed;
        }


        private AcademicYear GetCurrentYear()
        {
            var currentYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
            return (currentYear);
        }


        private AcademicYear GetpreviousYear(int currentYearId)
        {
            var academicYearlist = _AcademicYearRepository.List().ToList();
            AcademicYear previousYear = (from AcademicYear in academicYearlist
                          where AcademicYear.Id == currentYearId                         
                          let reversed = academicYearlist.OrderByDescending(p => p.Id)
                          let previous = reversed.SkipWhile(p => p.Id != currentYearId).Skip(1).FirstOrDefault()                         
                          select  previous ).First();
            ////var previousYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
            return (previousYear);
        }

        private bool isCanAddStAcademicData(StAcademicData oldStAcademicData)
        {
            var IsCanAddStAcademicData = false;
            var StAcademicDatas = _StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == oldStAcademicData.StPersonalData.AcademicID && x.AcademicYear == oldStAcademicData.AcademicYear);
            bool IsHasCurrentStAcademicData = _StAcademicDataRepository.List().Any(x => x.StPersonalData.AcademicID == oldStAcademicData.StPersonalData.AcademicID && x.AcademicYear.IsCurrentYear == true);
            if (IsHasCurrentStAcademicData)
            {
                return false;
            }
            foreach (var StAcademicData in StAcademicDatas)
            {
                IsCanAddStAcademicData = (StAcademicData.StStatus == StStatus.ناجح || StAcademicData.StStatus == StStatus.راسب) ? true : false;
                           
            }
            return IsCanAddStAcademicData;
        }
          
        private Batch GetNextBatch(StAcademicData oldstAcademicData)
        {
            var BatchList = _BatchRepository.List();
            Batch nextBatch = (from StAcademicData in BatchList
                               where StAcademicData.Id == oldstAcademicData.Batch.Id
                               let ordered = BatchList.OrderBy(p => p.Id)
                                         let Next = ordered.SkipWhile(p => p.Id != oldstAcademicData.Batch.Id).Skip(1).FirstOrDefault()
                                         select Next).First();
            return nextBatch;
        }

        private Level GetNextStAcademicDataLevel(StAcademicData StAcademicDatalistTerm)
        {
            
            Level nextlevel = StAcademicDatalistTerm.StLevel;
            if (StAcademicDatalistTerm.StStatus == StStatus.ناجح || StAcademicDatalistTerm.StStatus == StStatus.متخرج)
            {
                switch (StAcademicDatalistTerm.StLevel)
                {
                    case Level.الأول:
                        nextlevel = Level.الثاني;
                        break;
                    case Level.الثاني:
                        nextlevel = Level.الثالث;
                        break;
                    case Level.الثالث:
                        nextlevel = Level.الرابع;
                        break;
                    case Level.الرابع:
                        nextlevel = Level.الخامس;
                        break;
                    case Level.الخامس:
                        nextlevel = Level.السادس;
                        break;
                    case Level.السادس:
                        nextlevel = Level.السابع;
                        break;
                    case Level.السابع:
                        nextlevel = Level.الشامل;
                        break;

                }
            }
            return (nextlevel);
        }
        static ErrorResult GetErrorResult(bool IshasError,string ErrorMsg)
        {
            var ER = new ErrorResult()
            {
                IshasError = IshasError,
                ErrorMsg = ErrorMsg
            };
            return (ER);
        }

        private void AddCourseGradeTostAcademicData(StAcademicData stAcademicData)
        {
            var stAcCourses = _courseRepository.List().Where(x => x.Level == stAcademicData.StLevel).Where(x => x.Term == stAcademicData.Term).Where(x => x.Specialization == stAcademicData.Batch.Specialization).ToList();
            foreach (var course in stAcCourses)
            {

                var courseGrade = new CourseGrade()
                {
                    Course = course,
                    CourseType = true,
                    StAcademicData = stAcademicData,
                    StStatusForCourse = StStatusForCourse.غير_محدد,
                };
                _courseGradeRepository.Add(courseGrade);
            }
        }


        IList<StAcademicData> GetStAcademicDatas(bool IsCurrentYear, bool IsSelectCurrentYear, int? AcademicYearId, int? BatchId, StudyType? studyType, StStatus? stStatus, Term? term)
        {
            IList<StAcademicData> stAcademicDataList = new List<StAcademicData>();

            if (AcademicYearId != null)
            {
                ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName", AcademicYearId ?? 0);
                var academicYear = _AcademicYearRepository.Find(AcademicYearId ?? 0);
                //model.IsCurrentYear = academicYear.IsCurrentYear;

                var StAcademicDataOfAcademicYear = _StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == AcademicYearId && x.Term == term).ToList();
                if (StAcademicDataOfAcademicYear != null)
                {
                    stAcademicDataList = StAcademicDataOfAcademicYear;
                }

            }
            if (BatchId != null)
            {

                var BatchOfStAcademicData = stAcademicDataList.Where(x => x.Batch.Id == BatchId).ToList();
                if (BatchOfStAcademicData != null)
                {
                    stAcademicDataList = BatchOfStAcademicData;
                }

                ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "BatchName", BatchId ?? -1);
            }

            if (studyType != null)
            {
               
                var StudyTypesOfStAcademicData = stAcademicDataList.Where(x => x.StudyType == studyType).ToList();
                if (StudyTypesOfStAcademicData != null)
                {
                    stAcademicDataList = StudyTypesOfStAcademicData;
                }
            }

            if (stStatus != null)
            {
                var StStatusOfStAcademicData = stAcademicDataList.Where(x => x.StStatus == stStatus).ToList();
                if (StStatusOfStAcademicData != null)
                {
                    stAcademicDataList = StStatusOfStAcademicData;
                }
            }

            return stAcademicDataList;

        }
    }
}
