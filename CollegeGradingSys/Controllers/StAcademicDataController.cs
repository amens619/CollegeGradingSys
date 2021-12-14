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
    public class StAcademicDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StAcademicDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StAcademicData
        public async Task<IActionResult> Index()
        {
            return View(await _context.StAcademicData.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: StAcademicData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StLevel,Term,StStatus,Average,GPA,Valuation,IsCurrentYear")] StAcademicData stAcademicData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stAcademicData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stAcademicData);
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
    }
}
