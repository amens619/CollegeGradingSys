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

        public BatchController(ICollegeGradingSysRepository<Batch> studentBatchRepository, ICollegeGradingSysRepository<Specialization> specializationRepository)
        {
            _studentBatchRepository = studentBatchRepository;
            _specializationRepository = specializationRepository;
        }

        // GET: Batche
        public async Task<IActionResult> Index(int? id , int? SpecializationId)
        {
            //if (id != null)
            //{
            //    AcademicYearId = id;
            //}
            var viewModel = new BatchIndexData();
            IList<Batch> Batches = _studentBatchRepository.List().OrderBy(x => x.BatchName).ToList();

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
            if (ModelState.IsValid)
            {
                if (batchCreateData.SpecializationId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار  العام الدراسي من القائمة";
                    ViewData["SpecializationId"] = new SelectList(FillSelectSpecializationsList("-- اختر --"), "Id", "SpecializationName");
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
        public async Task<IActionResult> Edit(int? id)
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
            
            var model = new BatchCreateData
            {
                Id = studentBatch.Id,
                BatchName = studentBatch.BatchName,
                Note = studentBatch.Note
                
            };
            //ViewData["AcademicYearId"] = new SelectList(_academicYearRepository.List().ToList(), "Id", "AcademicYearName", studentBatch.Id);
            return View(model);
        }

        // POST: Batche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchName,AcademicYearId,Note")] BatchCreateData studentBatchVM)
        {
            if (id != studentBatchVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var academicYear = _specializationRepository.Find(studentBatchVM.SpecializationId);
                    Batch studentBatch = new()
                    {
                        Id = studentBatchVM.Id,
                        BatchName = studentBatchVM.BatchName,
                        Note = studentBatchVM.Note,
                        
                    };
                    _studentBatchRepository.Update(id, studentBatch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!BatchExists(studentBatch.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentBatchVM);
        }

        // GET: Batche/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentBatch = _studentBatchRepository.Delete(id ?? 0);
            if (studentBatch == null)
            {
                return NotFound();
            }

            return View(studentBatch);
        }

        // POST: Batche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
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

       
    }
}
