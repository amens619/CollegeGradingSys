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

namespace CollegeGradingSys.Controllers
{
    public class StAcademicDataController : Controller
    {
        private readonly ApplicationDbContext _context;
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
        public IActionResult Index( string sortOrder, string currentFilter, bool IsSelectCurrentYear, string StNameSearch, int? BatchId, int? AcademicYearId, StStatus? stStatus, Term? term,Level? level,bool IsCurrentYear, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        {
            FullAllListes("-- الكل --");
            var model = new StAcademicDataIndexViewModel();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";

            if (IsSelectCurrentYear == true)
            {
                var currentYear = _AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                AcademicYearId = currentYear.Id;
            }


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

       
        

        public IActionResult AllStAcademicDatas (int id)
        {
            var stPersonalData = _StPersonalDataRepository.Find(id);
            
            var stAcademicDatas = _StAcademicDataRepository.List();
            if (id != null)
            {
                stAcademicDatas = stAcademicDatas.Where(s => s.StPersonalData.AcademicID.Equals(id))
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
                
                StAcademicDatas = stAcademicDatas
            };
            return View(model);
        }

        // GET: StAcademicData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stAcademicData = await _context.StAcademicData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stAcademicData == null)
            {
                return NotFound();
            }

            return View(stAcademicData);
        }


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
            var stPersonalData = _StPersonalDataRepository.Find(id);
            if (stPersonalData == null)
            {
                return NotFound();
            }
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

                var isHasError = CheckModelErrors(model);

                if (isHasError.IshasError)
                {
                    ViewBag.Message = isHasError.ErrorMsg;
                    FullAllListes("-- أختر --");
                    return View(model);
                }

                          
                var stPersonalData = _StPersonalDataRepository.Find(model.AcademicID);
                var studentBatch = _BatchRepository.Find(model.BatchId);
                var academicYear = _AcademicYearRepository.Find(model.AcademicYearId); 
                
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

                return RedirectToAction(nameof(AllStAcademicDatas), new { id =model.AcademicID });
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
            var stAcademicData = _StAcademicDataRepository.Find(id ?? 0);
            if (stAcademicData == null)
            {
                return NotFound();
            }
            var model = new CreateStAcademicDataDataViewModel()
            {
                AcademicID = stAcademicData.StPersonalData.AcademicID,
                StLevel = stAcademicData.StLevel,
                Term = stAcademicData.Term,
                IsCurrentYear = stAcademicData.IsCurrentYear,
                StStatus = stAcademicData.StStatus,
                AcademicYearId = stAcademicData.AcademicYear.Id,
                BatchId = stAcademicData.Batch.Id,
                GPA = stAcademicData.GPA,
                Average = stAcademicData.Average,
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear,AcademicID,AcademicYearId,BatchId")] CreateStAcademicDataDataViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                var isHasError = CheckModelErrors(model);

                if (isHasError.IshasError)
                {
                    ViewBag.Message = isHasError.ErrorMsg;
                    FullAllListes();
                    return View(model);
                }


                var stPersonalData = _StPersonalDataRepository.Find(model.AcademicID);
                var studentBatch = _BatchRepository.Find(model.BatchId);
                var academicYear = _AcademicYearRepository.Find(model.AcademicYearId);

                var stAcademicData = new StAcademicData()
                {
                    Id = model.Id,
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


                _StAcademicDataRepository.Update(id,stAcademicData);
               
                

                return RedirectToAction(nameof(AllStAcademicDatas), new { id = model.AcademicID });
            }
            return View(model);
           
           
            
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

      
       
        private bool StAcademicDataExists(int id)
        {
            return _context.StAcademicData.Any(e => e.Id == id);
        }



        List<Batch> FillSelectBatchsList(string batchName)
        {
            var Batches = _BatchRepository.List().ToList();
            Batches.Insert(0, new Batch { Id = -1, BatchName = batchName  });

            return Batches;
        }

        List<AcademicYear> FillSelectAcademicYearesList(string academicYearName)
        {
            var AcademicYeares = _AcademicYearRepository.List().ToList();
            AcademicYeares.Insert(0, new AcademicYear { Id = -1, AcademicYearName = academicYearName });

            return AcademicYeares;
        }

        private void FullAllListes(string text)
        {
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(text), "Id", "AcademicYearName");
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
            if (model.AcademicYearId == -1)
            {                
                return (GetErrorResult(true, "الرجاء اختيار العام الاكاديمي من القائمة"));
            }
            if (!model.IsCurrentYear && model.StStatus == StStatus.مقيد)
            {
                if (model.Average == null || model.GPA == null || model.Valuation == Valuation.غير_محدد)
                {                   
                    return (GetErrorResult(true, "الرجاء اكمال ادخال البيانات او اختيار الفصل الحالي"));
                }
            }
            var StAcademicDataList = _StAcademicDataRepository.List();

            var checkStAcademicDataF = StAcademicDataList
            .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.AcademicYear.Id == model.AcademicYearId && x.Term == model.Term && x.Id != model.Id);

            if (checkStAcademicDataF)
            {
                return (GetErrorResult(true, "لقد تم إيجاد بيانات سابقة بنفس العام الاكادمي " +
                    "و الفصل الدراسي للطالب .. الرجاء اختيار عام أخر او اختيار فصل دراسي آخر"));
            }
            var checkStAcademicDataS = StAcademicDataList
                .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.StLevel == model.StLevel && x.Term == model.Term && x.Id != model.Id);

            if (checkStAcademicDataS)
            {                
                return (GetErrorResult(true, "لقد تم إيجاد بيانات سابقة بنفس المستوى و الفصل الدراسي للطالب .." +
                    " الرجاء اختيار مستوى أخر او اختيار فصل دراسي آخر"));
            }

            var checkStAcademicDatath = StAcademicDataList
             .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.IsCurrentYear == true && x.Id != model.Id);

            if (checkStAcademicDatath && model.IsCurrentYear == true)
            {
                ViewBag.Message = " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. الرجاء تعين فصل واحد فقط كفصل حالي للطالب  ";
                
                return (GetErrorResult(true, " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. " +
                    "الرجاء تعين فصل واحد فقط كفصل حالي للطالب  "));
            }

            return (GetErrorResult(false,""));
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

    }
}
