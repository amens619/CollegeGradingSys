using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class DepartmentController : Controller
    {
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;
        private readonly ICollegeGradingSysRepository<College> CollegeRepository;
        private readonly ICollegeGradingSysRepository<Specialization> SpecializationRepository;

        public DepartmentController(ICollegeGradingSysRepository<Department> DepartmentRepository,
            ICollegeGradingSysRepository<College> CollegeRepository,
            ICollegeGradingSysRepository<Specialization> SpecializationRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
            this.CollegeRepository = CollegeRepository;
            this.SpecializationRepository = SpecializationRepository;
        }
        // GET: DepartmentController
        public ActionResult Index()
        {
            var departments = DepartmentRepository.List();
            
            return View(departments);
        }

        // GET: DepartmentController/Details/5
        //public ActionResult Details(int id)
        //{
        //    var department = DepartmentRepository.Find(id);
        //    return View(department);
        //}

        // GET: DepartmentController/Create
        [Authorize(Policy = "CreateDepartmentPolicy")]
        public ActionResult Create()
        {

            var model = new CollegeDepartmentViewModel
            {
                 Colleges = FillSelectList()
            };

            return View(model);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateDepartmentPolicy")]
        public ActionResult Create(CollegeDepartmentViewModel  model)
        {
            try
            {
                model.Colleges = FillSelectList();
                if (model.DepartmentName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.DepartmentName), " الرجاء كتابة اسم القسم");

                    return View(model);
                }

                if (isDepartmentExistsByName((model.DepartmentName).Trim()))
                {
                    ModelState.AddModelError(nameof(model.DepartmentName), "لقد تم إيجاد قسم سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(model);
                }
                if (model.CollegeId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار الكلية من القائمة";

                    return View(GetAllColleges());
                }
                var college = CollegeRepository.Find(model.CollegeId);
                Department department = new Department
                {
                    Id = model.Id,
                     DepartmentName  = model.DepartmentName,
                     College = college,                    
                };                
                DepartmentRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Edit/5
        [Authorize(Policy = "EditDepartmentPolicy")]
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var department = DepartmentRepository.Find(id);
            if (department is null)
            {
                return NotFound();
            }
            var collegeId = department.College.Id ;
            var model = new CollegeDepartmentViewModel
            { 
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                 CollegeId= collegeId,
                Colleges = CollegeRepository.List().ToList()
        };
            return View(model);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDepartmentPolicy")]
        public ActionResult Edit(int id,CollegeDepartmentViewModel model)
        {
            try
            {
                if (model.DepartmentName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.DepartmentName), " الرجاء كتابة اسم القسم");

                    return View(model);
                }

                var department1 = DepartmentRepository.List().SingleOrDefault(x => x.DepartmentName == model.DepartmentName);
                if (department1 !=null && department1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.DepartmentName), "لقد تم إيجاد قسم سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(model);
                }
                var college = CollegeRepository.Find(model.CollegeId);
                var department = DepartmentRepository.Find(model.Id);

                department.DepartmentName = model.DepartmentName;
                department.College = college;
                              
                DepartmentRepository.Update(id, department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Delete/5
        [Authorize(Policy = "DeleteDepartmentPolicy")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var department = DepartmentRepository.Find(id);
            if (department is null)
            {
                return NotFound();
            }         
            
            return View(department);       
        }

        // POST: DepartmentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteDepartmentPolicy")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var SpecializationsOFDepartment = SpecializationRepository.List().Where(x => x.Department.Id == id).ToList();
                if (SpecializationsOFDepartment != null && SpecializationsOFDepartment.Count > 0)
                {
                    var department = DepartmentRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف القسم بسبب وجود تخصصات تابعة له.. الرجاء حذف التخصصات التابعة له أولا ";
                    return View(department);
                }
                DepartmentRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<College> FillSelectList()
        {
            var Colleges = CollegeRepository.List().ToList();
            Colleges.Insert(0, new College { Id = -1,  CollegeName = "-- أختر --" });

            return Colleges;
        }

        CollegeDepartmentViewModel GetAllColleges()
        {
            var vmodel = new CollegeDepartmentViewModel
            {
                 Colleges  = FillSelectList()
            };
            return vmodel;
        }

        private bool isDepartmentExistsByName(string DepartmentName)
        {
            return DepartmentRepository.List().Any(e => e.DepartmentName == DepartmentName);
        }
    }
}
