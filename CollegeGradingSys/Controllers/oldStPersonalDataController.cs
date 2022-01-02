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
    public class oldStPersonalDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public oldStPersonalDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StPersonalData
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                ViewData["AcademicID"] = id;
            }
            return View(await _context.StPersonalData.ToListAsync());
        }

        // GET: StPersonalData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stPersonalData = await _context.StPersonalData
                .FirstOrDefaultAsync(m => m.AcademicID == id);
            if (stPersonalData == null)
            {
                return NotFound();
            }

            return View(stPersonalData);
        }

        // GET: StPersonalData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StPersonalData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademicID,StName,IdentificatioNO,Sex,BirthDate,EnrollmentYearM,EnrollmentYearH")] StPersonalData stPersonalData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stPersonalData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stPersonalData);
        }

        // GET: StPersonalData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stPersonalData = await _context.StPersonalData.FindAsync(id);
            if (stPersonalData == null)
            {
                return NotFound();
            }
            return View(stPersonalData);
        }

        // POST: StPersonalData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademicID,StName,IdentificatioNO,Sex,BirthDate,EnrollmentYearM,EnrollmentYearH")] StPersonalData stPersonalData)
        {
            if (id != stPersonalData.AcademicID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stPersonalData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StPersonalDataExists(stPersonalData.AcademicID))
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
            return View(stPersonalData);
        }

        // GET: StPersonalData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stPersonalData = await _context.StPersonalData
                .FirstOrDefaultAsync(m => m.AcademicID == id);
            if (stPersonalData == null)
            {
                return NotFound();
            }

            return View(stPersonalData);
        }

        // POST: StPersonalData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stPersonalData = await _context.StPersonalData.FindAsync(id);
            _context.StPersonalData.Remove(stPersonalData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StPersonalDataExists(int id)
        {
            return _context.StPersonalData.Any(e => e.AcademicID == id);
        }
    }
}
