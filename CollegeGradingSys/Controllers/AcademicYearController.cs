using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class AcademicYearController : Controller
    {
        private readonly ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository;

        public AcademicYearController(ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository)
        {
            this.AcademicYearRepository = AcademicYearRepository;
        }
        // GET: AcademicYearController
        public ActionResult Index()
        {
            var academicYears = AcademicYearRepository.List();
            return View(academicYears);
        }

        // GET: AcademicYearController/Details/5
        public ActionResult Details(int id)
        {
            var academicYear = AcademicYearRepository.Find(id);
            return View(academicYear);
        }

        // GET: AcademicYearController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: AcademicYearController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AcademicYear academicYear)
        {
            try
            {
                AcademicYearRepository.Add(academicYear);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AcademicYearController/Edit/5
        public ActionResult Edit(int id)
        {
            var academicYear = AcademicYearRepository.Find(id);
            return View(academicYear);
        }

        // POST: AcademicYearController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,AcademicYear academicYear)
        {
            try
            {

                AcademicYearRepository.Update(id, academicYear);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AcademicYearController/Delete/5
        public ActionResult Delete(int id)
        {
            var academicYear = AcademicYearRepository.Find(id);
            return View(academicYear);
        }

        // POST: AcademicYearController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AcademicYear  academicYear)
        {
            try
            {
                AcademicYearRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
