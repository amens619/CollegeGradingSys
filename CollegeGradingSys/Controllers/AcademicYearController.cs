using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
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
        private readonly ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository;

        public AcademicYearController(ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository,
            ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository,
            ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository)
        {
            this.AcademicYearRepository = AcademicYearRepository;
            this.StAcademicDataRepository = StAcademicDataRepository;
            this.StPersonalDataRepository = StPersonalDataRepository;
        }
        // GET: AcademicYearController
        public ActionResult Index()
        {
            
            var academicYears = AcademicYearRepository.List().OrderByDescending(x => x.AcademicYearStart);
            var model = new AcademicYearVM()
            {
                AcademicYears = academicYears.ToList(),
                IsCurrentYearClosed = IsCurrentYearClosed()
            };                       
            return View(model);
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
            if (!IsCurrentYearClosed())
            {
                return NotFound();
            }
            return View();
        }

        // POST: AcademicYearController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AcademicYear academicYear)
        {
            if (!IsCurrentYearClosed())
            {
                return NotFound();
            }          

            if (ModelState.IsValid)
            {
                try
                {
                    if (academicYear.AcademicYearName == null)
                    {

                        ModelState.Clear();
                        ModelState.AddModelError(nameof(academicYear.AcademicYearName), " الرجاء كتابة العام الاكاديمي");

                        return View(academicYear);
                    }

                    if (isAcademicYearNameExists((academicYear.AcademicYearName).Trim()))
                    {
                        ModelState.AddModelError(nameof(academicYear.AcademicYearName), "لقد تم إيجاد عما سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                        return View(academicYear);
                    }
                    if (academicYear.AcademicYearStart >= academicYear.AcademicYearEnd)
                    {
                        ModelState.AddModelError(nameof(academicYear.AcademicYearEnd), "يجب ان يكون تاريخ نهاية العام بعد تاريخ بداية العام");
                        return View(academicYear);
                    }
                   var previousAcademicYears = AcademicYearRepository.List().Where(x => x.IsCurrentYear == true);
                    foreach (var pAcademicYear in previousAcademicYears)
                    {
                        pAcademicYear.IsCurrentYear = false;
                        AcademicYearRepository.Update(pAcademicYear.Id, pAcademicYear);
                    }


                    academicYear.IsCurrentYear = true;
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
                var StPersonalDatasOFBirthAcademicYear = StPersonalDataRepository.List().Where(x => x.EnrollmentYear.Id == id).ToList();
                if (StPersonalDatasOFBirthAcademicYear != null && StPersonalDatasOFBirthAcademicYear.Count > 0)
                {
                    var academicYear = AcademicYearRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف العام الجامعي بسبب تسجيل بعض الطلاب في نفس السنة.. الرجاء حذف الطلاب المسجلين فيها أولا ";
                    return View(academicYear);
                }

                var StAcademicDatasOFBirthAcademicYear = StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == id).ToList();
                if (StAcademicDatasOFBirthAcademicYear != null && StAcademicDatasOFBirthAcademicYear.Count > 0)
                {
                    var academicYear = AcademicYearRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف العام الجامعي بسبب وجود سجل اكاديمي لبعض الطلاب تابعة له.. الرجاء حذف السجلات التابعة لها أولا ";
                    return View(academicYear);
                }
               
                
                AcademicYearRepository.Delete(id);
                var LastYear = AcademicYearRepository.List().LastOrDefault();
                if(LastYear != null)
                {
                    LastYear.IsCurrentYear = true;
                    AcademicYearRepository.Update(LastYear.Id, LastYear);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool IsCurrentYearClosed()
        {
            var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
            var StAcademicOfAllStInCurrentYear = StAcademicDataRepository.List().Where(x => x.AcademicYear.Id == currentYear.Id);


            var stAs = StAcademicOfAllStInCurrentYear.Where(x => x.StStatus == StStatus.مقيد).ToList();
            if (stAs != null)
            {
                if (stAs.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
        
      
        private bool isAcademicYearNameExists(string academicYearName)
        {
            return AcademicYearRepository.List().Any(e => e.AcademicYearName == academicYearName);
        }
    }
}
