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
    public class SpecializationController : Controller
    {
        private readonly ICollegeGradingSysRepository<Specialization> SpecializationRepository;
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;
        private readonly ICollegeGradingSysRepository<Course> CourseRepository;
        private readonly ICollegeGradingSysRepository<Batch> BatchRepository;

        public SpecializationController(ICollegeGradingSysRepository<Specialization> SpecializationRepository,
            ICollegeGradingSysRepository<Department> DepartmentRepository
            ,ICollegeGradingSysRepository<Course> CourseRepository
            ,ICollegeGradingSysRepository<Batch> BatchRepository)
        {
            this.SpecializationRepository = SpecializationRepository;
            this.DepartmentRepository = DepartmentRepository;
            this.CourseRepository = CourseRepository;
            this.BatchRepository = BatchRepository;
        }
        // GET: SpecializationController
        public ActionResult Index()
        {
            var Specializations = SpecializationRepository.List();
            
            return View(Specializations);
        }

        // GET: SpecializationController/Details/5
        public ActionResult Details(int id)
        {
            var department = SpecializationRepository.Find(id);
            return View(department);
        }

        // GET: SpecializationController/Create
        [Authorize(Policy = "CreateSpecializationPolicy")]
        public ActionResult Create()
        {

            var model = new DepartmentSpecializationViewModel
            {
                Departments = FillSelectList()
            };

            return View(model);
        }

        // POST: SpecializationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateSpecializationPolicy")]
        public ActionResult Create(DepartmentSpecializationViewModel  model)
        {
            try
            {
                if (model.SpecializationName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.SpecializationName), " الرجاء كتابة اسم التخصص");
                    return View(GetAllDepartments(model));
                }

                if (SpecializationExistsByName((model.SpecializationName).Trim()))
                {
                    ModelState.AddModelError(nameof(model.SpecializationName), "لقد تم إيجاد تخصص سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(GetAllDepartments(model));
                }

                if (model.DepartmentId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار القسم من القائمة";

                    return View(GetAllDepartments(model));
                }
                var department = DepartmentRepository.Find(model.DepartmentId);
                Specialization specialization = new()
                {
                    Id = model.Id,
                     SpecializationName  = model.SpecializationName,
                     Department = department,                    
                };                
                SpecializationRepository.Add(specialization);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SpecializationController/Edit/5
        [Authorize(Policy = "EditSpecializationPolicy")]
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var specialization = SpecializationRepository.Find(id);
            if (specialization is null)
            {
                return NotFound();
            }           
            var departmentId =  specialization.Department.Id;
            var model = new DepartmentSpecializationViewModel
            { 
                Id = specialization.Id,
                SpecializationName = specialization.SpecializationName,
                 DepartmentId= departmentId,
                Departments = DepartmentRepository.List().ToList()
        };
            return View(model);
        }

        // POST: SpecializationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditSpecializationPolicy")]
        public ActionResult Edit(int id,DepartmentSpecializationViewModel model)
        {
            try
            {
                if (model.SpecializationName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.SpecializationName), " الرجاء كتابة اسم التخصص");

                    return View(GetAllDepartments(model));
                }

                var Specialization1 = SpecializationRepository.List().SingleOrDefault(x => x.SpecializationName == model.SpecializationName);
                if (Specialization1 != null && Specialization1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.SpecializationName), "لقد تم إيجاد تخصص سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(GetAllDepartments(model));
                }
                
                var department = DepartmentRepository.Find(model.DepartmentId);
                var specialization = SpecializationRepository.Find(model.Id);

                specialization.SpecializationName = model.SpecializationName;
                specialization.Department = department;
                                
                SpecializationRepository.Update(id, specialization);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SpecializationController/Delete/5
        [Authorize(Policy = "DeleteSpecializationPolicy")]
        public ActionResult Delete(int id)
        {
            var specialization = SpecializationRepository.Find(id);
            
            return View(specialization);       
        }

        // POST: SpecializationController/Delete/5       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteSpecializationPolicy")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var CoursesOFSpecialization = CourseRepository.List().Where(x => x.Specialization.Id == id).ToList();
                if (CoursesOFSpecialization != null && CoursesOFSpecialization.Count > 0)
                {
                    var specialization = SpecializationRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف التخصص بسبب وجود مواد تابعة له.. الرجاء حذف المواد التابعة له أولا ";
                    return View(specialization);
                }
                var BatchsOFSpecialization = BatchRepository.List().Where(x => x.Specialization.Id == id).ToList();
                if (BatchsOFSpecialization != null && BatchsOFSpecialization.Count > 0)
                {
                    var specialization = SpecializationRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف التخصص بسبب وجود دفعات تابعة له.. الرجاء حذف الدفعات التابعة له أولا ";
                    return View(specialization);
                }
                SpecializationRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Department> FillSelectList()
        {
            var Departments = DepartmentRepository.List().ToList();
            Departments.Insert(0, new Department { Id = -1,  DepartmentName = "-- أختر --" });

            return Departments;
        }

        DepartmentSpecializationViewModel GetAllDepartments(DepartmentSpecializationViewModel model)
        {
            var vmodel = new DepartmentSpecializationViewModel
            {
                Id=model.Id,
                DepartmentId = model.DepartmentId,
                SpecializationName = model.SpecializationName,
                Departments  = FillSelectList()
            };
            return vmodel;
        }

        private bool SpecializationExistsByName(string specializationName)
        {
            return SpecializationRepository.List().Any(e => e.SpecializationName == specializationName);
        }
    }
}
