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
    public class SpecializationController : Controller
    {
        private readonly ICollegeGradingSysRepository<Specialization> SpecializationRepository;
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;

        public SpecializationController(ICollegeGradingSysRepository<Specialization> SpecializationRepository, ICollegeGradingSysRepository<Department> DepartmentRepository)
        {
            this.SpecializationRepository = SpecializationRepository;
            this.DepartmentRepository = DepartmentRepository;
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
        public ActionResult Create(DepartmentSpecializationViewModel  model)
        {
            try
            {

                if (model.DepartmentId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار القسم من القائمة";

                    return View(GetAllDepartments());
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
        public ActionResult Edit(int id)
        {
            var department = SpecializationRepository.Find(id);
            var departmentId = department.Department == null ? department.Department.Id = 0 : department.Department.Id;
            var model = new DepartmentSpecializationViewModel
            { 
                Id = department.Id,
                SpecializationName = department.SpecializationName,
                 DepartmentId= departmentId,
                Departments = DepartmentRepository.List().ToList()
        };
            return View(model);
        }

        // POST: SpecializationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,DepartmentSpecializationViewModel model)
        {
            try
            {
                var department = DepartmentRepository.Find(model.DepartmentId);
                Specialization specialization = new()
                {
                    Id = model.Id,
                    SpecializationName = model.SpecializationName,
                    Department = department,
                };                
                SpecializationRepository.Update(id, specialization);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SpecializationController/Delete/5
        public ActionResult Delete(int id)
        {
            var specialization = SpecializationRepository.Find(id);
            
            return View(specialization);       
        }

        // POST: SpecializationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DepartmentSpecializationViewModel model)
        {
            try
            {
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

        DepartmentSpecializationViewModel GetAllDepartments()
        {
            var vmodel = new DepartmentSpecializationViewModel
            {
                 Departments  = FillSelectList()
            };
            return vmodel;
        }
    }
}
