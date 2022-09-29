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
        public IActionResult Index( string Message, string sortOrder, string currentFilter, bool IsSelectCurrentYear, string StNameSearch, int? BatchId, int? AcademicYearId, StStatus? stStatus, Term? term,Level? level,bool IsCurrentYear, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
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
                IsRegisteredInCurrentYear = true,
                StAcademicDatas = stAcademicDatas                
            };
            var currentYear = GetCurrentYear();
            var stACourseGrades = stAcademicDatas.Where(x => x.AcademicYear.Id == currentYear.Id).ToList();
                if(stACourseGrades == null || stACourseGrades.Count <= 0)
                {
                    model.IsRegisteredInCurrentYear = false;
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
            if (model.StStatus == StStatus.متخرج)
            {
                if (model.StLevel != Level.الرابع || model.Term != Term.الثاني)
                {
                    ViewBag.Message = "عذراً .. يتم اختيار حالة الطالب 'متخرج' فقط في حالة النجاح في الفصل الثاني من المستوى الرابع   ";
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
                var courseGradesFailed = CourseGradesFailed(model.AcademicID, model.Term);
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
            if (StAcademicDatalistTerm.StStatus == StStatus.ناجح)
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
                    //case Level.الرابع:
                    //    nextlevel = Level.الخامس;
                    //    break;
                    //case Level.الخامس:
                    //    nextlevel = Level.السادس;
                    //    break;
                    //case Level.السادس:
                    //    nextlevel = Level.السابع;
                    //    break;
                    //case Level.السابع:
                    //    nextlevel = Level.الشامل;
                    //    break;

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

    }
}
