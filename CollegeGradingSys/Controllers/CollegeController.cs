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
    public class CollegeController : Controller
    {
        private readonly ICollegeGradingSysRepository<College> CollegeRepository;
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;

        public CollegeController(ICollegeGradingSysRepository<College> CollegeRepository
            ,ICollegeGradingSysRepository<Department> DepartmentRepository)
        {
            this.CollegeRepository = CollegeRepository;
            this.DepartmentRepository = DepartmentRepository;
        }
        // GET: CollegeController
        public ActionResult Index()
        {
            var colleges = CollegeRepository.List();
            return View(colleges);
        }

        // GET: CollegeController/Details/5
        public ActionResult Details(int id)
        {
            var college = CollegeRepository.Find(id);
            return View(college);
        }

        // GET: CollegeController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: CollegeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(College college)
        {
            try
            {
                if(college.CollegeName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(college.CollegeName), " الرجاء كتابة اسم الكلية");
                    return View(college);
                }

                if (CollegeExistsByName((college.CollegeName).Trim()))
                {
                    ModelState.AddModelError(nameof(college.CollegeName), "لقد تم إيجاد كلية سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(college);
                }
                CollegeRepository.Add(college);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CollegeController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var college = CollegeRepository.Find(id);
            if (college is null)
            {
                return NotFound();
            }
            return View(college);
        }

        // POST: CollegeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,College model)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                if (model.CollegeName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.CollegeName), " الرجاء كتابة اسم الكلية");
                    return View(model);
                }
                
                var college1 = CollegeRepository.List().SingleOrDefault(x => x.CollegeName == model.CollegeName);
                if (college1 != null && college1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.CollegeName), "لقد تم إيجاد كلية سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(model);
                }
                var college = CollegeRepository.Find(id);
                college.CollegeName = model.CollegeName;              
                

                CollegeRepository.Update(id, college);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CollegeController/Delete/5
        public ActionResult Delete(int id)
        {
            var college = CollegeRepository.Find(id);
            return View(college);
        }

        // POST: CollegeController/Delete/5
        // POST: StAcademicData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {        
            try
            {
                var DepartmentsOfCollege = DepartmentRepository.List().Where(x => x.College.Id == id).ToList();
                if(DepartmentsOfCollege !=null && DepartmentsOfCollege.Count > 0)
                {
                    var college = CollegeRepository.Find(id);
                    ViewBag.Message =  "لا يمكن حذف الكلية بسبب وجود اقسام تابعة لها.. الرجاء حذف الاقسام التابعة لها أولا ";
                    return View(college);
                }
                CollegeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool CollegeExistsByName(string CollegeName)
        {
            return CollegeRepository.List().Any(e => e.CollegeName == CollegeName);
        }
    }
}
