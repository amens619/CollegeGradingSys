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
        private readonly ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository;

        public AcademicYearController(ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository,
            ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository)
        {
            this.AcademicYearRepository = AcademicYearRepository;
            this.StAcademicDataRepository = StAcademicDataRepository;
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
            if (ModelState.IsValid)
            {
                try
                {
                    if (academicYear.AcademicYearStart >= academicYear.AcademicYearEnd)
                    {
                        ModelState.AddModelError(nameof(academicYear.AcademicYearEnd), "يجب ان يكون تاريخ نهاية العام بعد تاريخ بداية العام");
                        return View(academicYear);
                    }

                    AcademicYearRepository.Add(academicYear);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(academicYear);
                }
                
            }
            return View(academicYear);
        }

        // GET: AcademicYearController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var academicYear = AcademicYearRepository.Find(id);
            if (academicYear is null)
            {
                return NotFound();
            }           
            return View(academicYear);
        }

        // POST: AcademicYearController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,AcademicYear academicYear)
        {
            try
            {
                if (academicYear.AcademicYearStart >= academicYear.AcademicYearEnd)
                {
                    ModelState.AddModelError(nameof(academicYear.AcademicYearEnd), "يجب ان يكون تاريخ نهاية العام بعد تاريخ بداية العام");
                    return View(academicYear);
                }
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
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var academicYear = AcademicYearRepository.Find(id);
            if (academicYear is null)
            {
                return NotFound();
            }
           
            return View(academicYear);
        }

        // POST: AcademicYearController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var StAcademicDatasOFBirthAcademicYear = StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == id).ToList();
                if (StAcademicDatasOFBirthAcademicYear != null && StAcademicDatasOFBirthAcademicYear.Count > 0)
                {
                    var academicYear = AcademicYearRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف العام الجامعي بسبب وجود سجل اكاديمي لبعض الطلاب تابعة له.. الرجاء حذف السجلات التابعة لها أولا ";
                    return View(academicYear);
                }
                
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
