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

namespace CollegeGradingSys.Controllers
{
    public class StHighSchoolDataController : Controller
    {
        private readonly ICollegeGradingSysRepository<StHighSchoolData> _StHighSchoolDataRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;

        public StHighSchoolDataController(ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository,ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository)
        {
            
            _StHighSchoolDataRepository = StHighSchoolDataRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
        }

        // GET: StHighSchoolData
        public  IActionResult Index()
        {
            var stHighSchoolDatas = _StHighSchoolDataRepository.List();            
            return View(stHighSchoolDatas);
        }

        // GET: StHighSchoolData/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
          
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = new StHighSchoolData();
            model.AcademicID = id ?? 0;
            

            //ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH");
            return PartialView("_Create", model);
        }

        // POST: StHighSchoolData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolData stHighSchoolData)
        {
            if (ModelState.IsValid)
            {
                _StHighSchoolDataRepository.Add(stHighSchoolData);                
                return RedirectToAction(nameof(Index), "StPersonalData");
                
            }          
            return PartialView("_Create", stHighSchoolData);
        }

        // GET: StHighSchoolData/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StHighSchoolData stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", stHighSchoolData); 
        }

        // POST: StHighSchoolData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolData stHighSchoolData)
        {
            if (id != stHighSchoolData.AcademicID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                        _StHighSchoolDataRepository.Update(id, stHighSchoolData);                      
                        return RedirectToAction(nameof(Details), "StPersonalData", new { id = stHighSchoolData.AcademicID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!StHighSchoolDataExists(stHighSchoolData.AcademicID))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                        throw;
                    //}
                }

                
            }           
            return PartialView("_Edit", stHighSchoolData);
        }

        // GET: StHighSchoolData/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
                
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", stHighSchoolData);
        }

        // POST: StHighSchoolData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            
            _StHighSchoolDataRepository.Delete(id);

            return RedirectToAction(nameof(Details), "StPersonalData", new { id = id });
           
        }

        //private bool StHighSchoolDataExists(int id)
        //{
        //    return _context.StHighSchoolData.Any(e => e.AcademicID == id);
        //}
    }
}
