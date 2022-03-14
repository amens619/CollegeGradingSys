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
using CollegeGradingSys.ViewModels;

namespace CollegeGradingSys.Controllers
{
    public class BatchController : Controller
    {
       
        private readonly ICollegeGradingSysRepository<Batch> _studentBatchRepository;
        private readonly ICollegeGradingSysRepository<Specialization> _specializationRepository;
        private readonly ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository;

        public BatchController(ICollegeGradingSysRepository<Batch> studentBatchRepository,
            ICollegeGradingSysRepository<Specialization> specializationRepository,
            ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository)
        {
            _studentBatchRepository = studentBatchRepository;
            _specializationRepository = specializationRepository;
            this.StAcademicDataRepository = StAcademicDataRepository;
        }

        // GET: Batche
        public async Task<IActionResult> Index(int? id , int? SpecializationId)
        {
            //if (id != null)
            //{
            //    AcademicYearId = id;
            //}
            var viewModel = new BatchIndexData();
            IList<Batch> Batches = _studentBatchRepository.List().OrderByDescending(x => x.Id).ToList();

            if (SpecializationId is not null and not (-1))
            {
                viewModel.SpecializationId = SpecializationId;
                Batches = Batches.Where(a => a.Specialization.Id == SpecializationId).ToList();

            }
            viewModel.Batches = Batches;
            ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationsList("-- الكل --"), "Id", "SpecializationName", SpecializationId ?? -1);
            return View(viewModel);
        }

        // GET: Batche/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            
            var studentBatch = _studentBatchRepository.Find(id ?? 0);
               
            if (studentBatch == null)
            {
                return NotFound();
            }

            return View(studentBatch);
        }

        // GET: Batche/Create
        public IActionResult Create()
        {

            ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationsList("-- اختر --"), "Id", "SpecializationName");
            return View();
        }

        // POST: Batche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Id,BatchName,SpecializationId,Note")] BatchCreateData  batchCreateData)
        {
            ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationsList("-- اختر --"), "Id", "SpecializationName");
            if (ModelState.IsValid)
            {
                if (batchCreateData.BatchName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(batchCreateData.BatchName), "الرجاء ادخال اسم الدفعة");

                    return View(batchCreateData);
                }

                if (isBatchNameExists((batchCreateData.BatchName).Trim()))
                {
                    ModelState.AddModelError(nameof(batchCreateData.BatchName), "لقد تم إيجاد دفعة سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(batchCreateData);
                }

                if (batchCreateData.SpecializationId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار  العام الدراسي من القائمة";
                    
                    return View(batchCreateData);
                }
                var specialization = _specializationRepository.Find(batchCreateData.SpecializationId);
                Batch batch = new Batch
                {
                    Id = batchCreateData.Id,
                    BatchName = batchCreateData.BatchName,
                    Note = batchCreateData.Note,
                    Specialization = specialization
                };
                _studentBatchRepository.Add(batch);
                return RedirectToAction(nameof(Index));
            }
            return View(batchCreateData);
        }

        // GET: Batche/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentBatch = _studentBatchRepository.Find(id);

            if (studentBatch == null)
            {
                return NotFound();
            }
            
            var model = new BatchCreateData
            {
                
                Id = studentBatch.Id,
                BatchName = studentBatch.BatchName,
                SpecializationId = studentBatch.Specialization.Id,
                Note = studentBatch.Note
                 
            };
            ViewData["SpecializationId"] = new SelectList(_specializationRepository.List().ToList(), "Id", "SpecializationName");
            return View(model);
        }

        // POST: Batche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchName,SpecializationId,Note")] BatchCreateData studentBatchVM)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != studentBatchVM.Id)
            {
                return NotFound();
            }
            ViewData["SpecializationId"] = new SelectList(_specializationRepository.List().ToList(), "Id", "SpecializationName");
            if (ModelState.IsValid)
            {
                try
                {
                    if (studentBatchVM.BatchName == null)
                    {

                        ModelState.Clear();
                        ModelState.AddModelError(nameof(studentBatchVM.BatchName), "الرجاء ادخال اسم الدفعة");

                        return View(studentBatchVM);
                    }

                    var department1 = _studentBatchRepository.List().SingleOrDefault(x => x.BatchName == studentBatchVM.BatchName);
                    if (department1 != null && department1.Id != studentBatchVM.Id)
                    {
                        ModelState.AddModelError(nameof(studentBatchVM.BatchName), "لقد تم إيجاد دفعة سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                        return View(studentBatchVM);
                    }
                    var specialization = _specializationRepository.Find(studentBatchVM.SpecializationId);
                    var studentBatch = _studentBatchRepository.Find(studentBatchVM.Id);

                    studentBatch.BatchName = studentBatchVM.BatchName;
                    studentBatch.Specialization = specialization;
                    studentBatch.Note = studentBatchVM.Note;
                        
                    
                    _studentBatchRepository.Update(id, studentBatch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View(studentBatchVM);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentBatchVM);
        }

        // GET: Batche/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentBatch = _studentBatchRepository.Find(id);
            if (studentBatch == null)
            {
                return NotFound();
            }

            return View(studentBatch);
        }

        // POST: Batche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var StAcademicDatasOFBatch = StAcademicDataRepository.List().Where(x => x.Batch.Id == id).ToList();
                if (StAcademicDatasOFBatch != null && StAcademicDatasOFBatch.Count > 0)
                {
                    var studentBatch = _studentBatchRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف الدفعة بسبب وجود سجل اكاديمي لبعض الطلاب تابعة لها.. الرجاء حذف السجلات التابعة لها أولا ";
                    return View(studentBatch);
                }
                _studentBatchRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
          
        }

        //private bool BatchExists(int id)
        //{
        //    return _context.Batch.Any(e => e.Id == id);
        //}

        List<Specialization> FillSelectSpecializationsList(string specializationName)
        {
            var specializations = _specializationRepository.List().ToList();
            specializations.Insert(0, new Specialization { Id = -1,  SpecializationName  = specializationName });

            return specializations;
        }

        private bool isBatchNameExists(string batchName)
        {
            return _studentBatchRepository.List().Any(e => e.BatchName == batchName);
        }
    }
}
