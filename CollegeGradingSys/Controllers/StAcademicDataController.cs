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

namespace CollegeGradingSys.Controllers
{
    public class StAcademicDataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICollegeGradingSysRepository<StAcademicData> _StAcademicDataRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<Batch> _BatchRepository;
        private readonly ICollegeGradingSysRepository<AcademicYear> _AcademicYearRepository;

        public StAcademicDataController(ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository
            , ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository
            , ICollegeGradingSysRepository<Batch> BatchRepository
            , ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository)
        {
            _StAcademicDataRepository = StAcademicDataRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
            _BatchRepository = BatchRepository;
            _AcademicYearRepository = AcademicYearRepository;
        }

        // GET: StAcademicData
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, string BatchName, int? id, StStatus? StStatus, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";



            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var StAcademicDatasR = _StAcademicDataRepository.List();

            if (!String.IsNullOrEmpty(searchString))
            {
                StAcademicDatasR = StAcademicDatasR.Where(s => s.StPersonalData.StName.Contains(searchString)).ToList();

            }

            //if (SearchAcademicID != null)
            //{
            //    StPersonalDatasR = StPersonalDatasR.Where(s => s.AcademicID.Equals(SearchAcademicID)).ToList();

            //}

            var StPersonalDatas = StAcademicDatasR
                .Skip(ExcludeRecords).Take(pageSize);

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        StPersonalDatas = StPersonalDatas.OrderByDescending(s => s.StName).ToList();
            //        break;
            //    case "SexSortParm":
            //        StPersonalDatas = StPersonalDatas.OrderBy(s => s.Sex).ToList();
            //        break;
            //    case "SexSortParm_desc":
            //        StPersonalDatas = StPersonalDatas.OrderByDescending(s => s.Sex).ToList();
            //        break;
            //    default:
            //        StPersonalDatas = StPersonalDatas.OrderBy(s => s.StName).ToList();
            //        break;
            //}





            var result = new PagedResult<StAcademicData>
            {
                Data = StPersonalDatas.ToList(),
                TotalItems = StAcademicDatasR.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

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

            //ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "studentBatchName", -1);
            var StAcademicDatas = _StAcademicDataRepository.List();
            return View(StAcademicDatas);
        }


        public IActionResult AllStAcademicDatas (int id)
        {
            var stPersonalData = _StPersonalDataRepository.Find(id);
            
            var stAcademicDatas = _StAcademicDataRepository.List();
            if (id != null)
            {                
                stAcademicDatas = stAcademicDatas.Where(s => s.StPersonalData.AcademicID.Equals(id))
                    //.OrderBy(x => x.AcademicYear.AcademicYearStart)
                    .OrderBy(x => x.Term)
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

        // GET: StAcademicData/Create
        public IActionResult Create(int id)
        {
            var stPersonalData = _StPersonalDataRepository.Find(id);
            var model = new CreateStAcademicDataDataViewModel()
            {
                AcademicID = stPersonalData.AcademicID
            };

            
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName");
            ViewData["BatchId"] = new SelectList(FillSelectBatchsList(), "Id", "BatchName");
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
                    FullAllAcademicYearsAndBatches();
                    return View(model);
                }
                if (model.AcademicYearId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار الدفعة من القائمة";
                    FullAllAcademicYearsAndBatches();
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
                        FullAllAcademicYearsAndBatches();
                        return View(model);
                    }
                }
                var StAcademicDataList = _StAcademicDataRepository.List();

                //var checkStAcademicDataF = StAcademicDataList
                //.Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.AcademicYear.Id == model.AcademicYearId && x.Term == model.Term);
                    
                //if(checkStAcademicDataF)
                //{
                //    ViewBag.Message = "لقد تم إيجاد بيانات سابقة بنفس العام الاكادمي و الفصل الدراسي للطالب .. الرجاء اختيار عام أخر او اختيار فصل دراسي آخر";
                //    FullAllAcademicYearsAndBatches();
                //    return View(model);
                //}
                var checkStAcademicDataS = StAcademicDataList
                .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.StLevel == model.StLevel && x.Term == model.Term);

                if (checkStAcademicDataS)
                {
                    ViewBag.Message = "لقد تم إيجاد بيانات سابقة بنفس المستوى و الفصل الدراسي للطالب .. الرجاء اختيار مستوى أخر او اختيار فصل دراسي آخر";
                    FullAllAcademicYearsAndBatches();
                    return View(model);
                }

                var checkStAcademicDatath = StAcademicDataList
              .Any(x => x.StPersonalData.AcademicID == model.AcademicID && x.IsCurrentYear == true );

                if (checkStAcademicDatath && model.IsCurrentYear == true )
                {
                    ViewBag.Message = " لقد تم تعين فصل دراسي سابق  كفصل حالي للطالب .. الرجاء تعين فصل واحد فقط كفصل حالي للطالب  ";
                    FullAllAcademicYearsAndBatches();
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
                    //AcademicYear = academicYear,
                    //Batch = studentBatch
                };

                _StAcademicDataRepository.Add(stAcademicData);               
                return RedirectToAction(nameof(AllStAcademicDatas), new { id =model.AcademicID });
            }
            return View(model);
        }

       

        // GET: StAcademicData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stAcademicData = await _context.StAcademicData.FindAsync(id);
            if (stAcademicData == null)
            {
                return NotFound();
            }
            return View(stAcademicData);
        }

        // POST: StAcademicData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear")] StAcademicData stAcademicData)
        {
            if (id != stAcademicData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stAcademicData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StAcademicDataExists(stAcademicData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stAcademicData);
        }

        // GET: StAcademicData/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: StAcademicData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stAcademicData = await _context.StAcademicData.FindAsync(id);
            _context.StAcademicData.Remove(stAcademicData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StAcademicDataExists(int id)
        {
            return _context.StAcademicData.Any(e => e.Id == id);
        }

        public JsonResult GetBatchs(int Id)
        {
            var studentBatchsList = _BatchRepository.List().Where(a => a.Id == Id);
            return Json(new SelectList(studentBatchsList, "Id", "BatchName"));
        }

        List<Batch> FillSelectBatchsList()
        {
            var Batches = _BatchRepository.List().ToList();
            Batches.Insert(0, new Batch { Id = -1, BatchName = "-- أختر --"});

            return Batches;
        }

        List<AcademicYear> FillSelectAcademicYearesList()
        {
            var AcademicYeares = _AcademicYearRepository.List().ToList();
            AcademicYeares.Insert(0, new AcademicYear { Id = -1, AcademicYearName = "-- أختر --" });

            return AcademicYeares;
        }

        private void FullAllAcademicYearsAndBatches()
        {
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList(), "Id", "AcademicYearName");
            ViewData["BatchId"] = new SelectList(FillSelectBatchsList(), "Id", "BatchName");
        }
    }
}
