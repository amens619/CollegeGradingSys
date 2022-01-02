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
    public class StudentBatchController : Controller
    {
       
        private readonly ICollegeGradingSysRepository<StudentBatch> _studentBatchRepository;
        private readonly ICollegeGradingSysRepository<AcademicYear> _academicYearRepository;

        public StudentBatchController(ICollegeGradingSysRepository<StudentBatch> studentBatchRepository, ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository)
        {
            _studentBatchRepository = studentBatchRepository;
            _academicYearRepository = AcademicYearRepository;
        }

        // GET: StudentBatche
        public async Task<IActionResult> Index(int? id , int? AcademicYearId)
        {
            if (id != null)
            {
                AcademicYearId = id;
            }
            var viewModel = new StudentBatchIndexData();
            IList<StudentBatch> studentBatches = _studentBatchRepository.List();              
            
            if (AcademicYearId is not null and not (-1))
            {
                viewModel.AcademicYearId = AcademicYearId;
                studentBatches = studentBatches.Where(a => a.AcademicYear.Id == AcademicYearId).ToList();
               
            }
            viewModel.StudentBatches = studentBatches;
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearsList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1 );
            return View(viewModel);
        }

        // GET: StudentBatche/Details/5
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

        // GET: StudentBatche/Create
        public IActionResult Create()
        {
            
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearsList("-- اختر --"), "Id", "AcademicYearName");
            return View();
        }

        // POST: StudentBatche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentBatchName,AcademicYearId,Note")] StudentBatchCreateData studentBatchVM)
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
                StudentBatch studentBatch = new StudentBatch
                {
                    Id = studentBatchVM.Id,
                     StudentBatchName = studentBatchVM.StudentBatchName,
                      Note= studentBatchVM.Note,
                    AcademicYear = academicYear
                };
                _studentBatchRepository.Add(studentBatch);
                return RedirectToAction(nameof(Index));
            }
            return View(studentBatchVM);
        }

        // GET: StudentBatche/Edit/5
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
            
            var model = new StudentBatchCreateData
            {
                Id = studentBatch.Id,
                StudentBatchName = studentBatch.StudentBatchName,
                Note = studentBatch.Note,
                AcademicYear = studentBatch.AcademicYear
            };
            ViewData["AcademicYearId"] = new SelectList(_academicYearRepository.List().ToList(), "Id", "AcademicYearName", studentBatch.AcademicYear.Id);
            return View(model);
        }

        // POST: StudentBatche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentBatchName,AcademicYearId,Note")] StudentBatchCreateData studentBatchVM)
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
                    StudentBatch studentBatch = new()
                    {
                        Id = studentBatchVM.Id,
                        StudentBatchName = studentBatchVM.StudentBatchName,
                        Note = studentBatchVM.Note,
                        AcademicYear = academicYear
                    };
                    _studentBatchRepository.Update(id, studentBatch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!StudentBatchExists(studentBatch.Id))
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

        // GET: StudentBatche/Delete/5
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

        // POST: StudentBatche/Delete/5
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

        //private bool StudentBatchExists(int id)
        //{
        //    return _context.StudentBatch.Any(e => e.Id == id);
        //}

        List<AcademicYear> FillSelectAcademicYearsList(string academicYearName)
        {
            var academicYears = _academicYearRepository.List().ToList();
            academicYears.Insert(0, new AcademicYear { Id = -1, AcademicYearName = academicYearName });

            return academicYears;
        }

       
    }
}
