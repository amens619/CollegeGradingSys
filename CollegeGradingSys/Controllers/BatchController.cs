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
        private readonly ICollegeGradingSysRepository<AcademicYear> _academicYearRepository;

        public BatchController(ICollegeGradingSysRepository<Batch> studentBatchRepository, ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository)
        {
            _studentBatchRepository = studentBatchRepository;
            _academicYearRepository = AcademicYearRepository;
        }

        // GET: Batche
        public async Task<IActionResult> Index(int? id , int? AcademicYearId)
        {
            if (id != null)
            {
                AcademicYearId = id;
            }
            var viewModel = new BatchIndexData();
            IList<Batch> studentBatches = _studentBatchRepository.List();              
            
            if (AcademicYearId is not null and not (-1))
            {
                viewModel.AcademicYearId = AcademicYearId;
                //studentBatches = studentBatches.Where(a => a.AcademicYear.Id == AcademicYearId).ToList();
               
            }
            viewModel.Batches = studentBatches;
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearsList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1 );
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
            
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearsList("-- اختر --"), "Id", "AcademicYearName");
            return View();
        }

        // POST: Batche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchName,AcademicYearId,Note")] BatchCreateData studentBatchVM)
        {
            if (ModelState.IsValid)
            {
                if (studentBatchVM.AcademicYearId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار  العام الدراسي من القائمة";
                    ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearsList("-- اختر --"), "Id", "AcademicYearName");
                    return View(studentBatchVM);
                }
                var academicYear = _academicYearRepository.Find(studentBatchVM.AcademicYearId);
                Batch studentBatch = new Batch
                {
                    Id = studentBatchVM.Id,
                     BatchName = studentBatchVM.BatchName,
                      Note= studentBatchVM.Note,
                    //AcademicYear = academicYear
                };
                _studentBatchRepository.Add(studentBatch);
                return RedirectToAction(nameof(Index));
            }
            return View(studentBatchVM);
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
                    var academicYear = _academicYearRepository.Find(studentBatchVM.AcademicYearId);
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

        List<AcademicYear> FillSelectAcademicYearsList(string academicYearName)
        {
            var academicYears = _academicYearRepository.List().ToList();
            academicYears.Insert(0, new AcademicYear { Id = -1, AcademicYearName = academicYearName });

            return academicYears;
        }

       
    }
}
