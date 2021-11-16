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
    public class DepartmentController : Controller
    {
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;
        private readonly ICollegeGradingSysRepository<College> CollegeRepository;

        public DepartmentController(ICollegeGradingSysRepository<Department> DepartmentRepository, ICollegeGradingSysRepository<College> CollegeRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
            this.CollegeRepository = CollegeRepository;
        }
        // GET: DepartmentController
        public ActionResult Index()
        {
            var departments = DepartmentRepository.List();
            
            return View(departments);
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(int id)
        {
            var department = DepartmentRepository.Find(id);
            return View(department);
        }

        // GET: DepartmentController/Create
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
        public ActionResult Create(CollegeDepartmentViewModel  model)
        {
            try
            {

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
        public ActionResult Edit(int id)
        {
            var department = DepartmentRepository.Find(id);
            var collegeId = department.College == null ? department.College.Id = 0 : department.College.Id;
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
        public ActionResult Edit(int id,CollegeDepartmentViewModel model)
        {
            try
            {
                var college = CollegeRepository.Find(model.CollegeId);
                Department department = new()
                {
                    Id = model.Id,
                    DepartmentName = model.DepartmentName,
                    College = college,
                };                
                DepartmentRepository.Update(id, department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            var department = DepartmentRepository.Find(id);
            
            return View(department);       
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CollegeDepartmentViewModel model)
        {
            try
            {
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
    }
}
