using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeGradingSys.Data;
using CollegeGradingSys.Models;

namespace CollegeGradingSys.Controllers
{
    public class StHighSchoolDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StHighSchoolDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StHighSchoolData
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StHighSchoolData.Include(s => s.StPersonalData);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StHighSchoolData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = await _context.StHighSchoolData
                .Include(s => s.StPersonalData)
                .FirstOrDefaultAsync(m => m.AcademicID == id);
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Create
        public IActionResult Create(int? id)
        {
            var model = new StHighSchoolData();
            if (id != null)
            {
                model.AcademicID = id ?? 0;
            }

            //ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH");
            return PartialView("_Create", model);
        }

        // POST: StHighSchoolData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolData stHighSchoolData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stHighSchoolData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "stPersonalData");
            }
            ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH", stHighSchoolData.AcademicID);
            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = await _context.StHighSchoolData.FindAsync(id);
            if (stHighSchoolData == null)
            {
                return NotFound();
            }
            ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH", stHighSchoolData.AcademicID);
            return View(stHighSchoolData);
        }

        // POST: StHighSchoolData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolData stHighSchoolData)
        {
            if (id != stHighSchoolData.AcademicID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stHighSchoolData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StHighSchoolDataExists(stHighSchoolData.AcademicID))
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
            ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH", stHighSchoolData.AcademicID);
            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = await _context.StHighSchoolData
                .Include(s => s.StPersonalData)
                .FirstOrDefaultAsync(m => m.AcademicID == id);
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return View(stHighSchoolData);
        }

        // POST: StHighSchoolData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stHighSchoolData = await _context.StHighSchoolData.FindAsync(id);
            _context.StHighSchoolData.Remove(stHighSchoolData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StHighSchoolDataExists(int id)
        {
            return _context.StHighSchoolData.Any(e => e.AcademicID == id);
        }
    }
}
