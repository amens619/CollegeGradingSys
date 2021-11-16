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
    public class DepartmentController : Controller
    {
        private readonly ICollegeGradingSysRepository<Department> DepartmentRepository;

        public DepartmentController(ICollegeGradingSysRepository<Department> DepartmentRepository)
        {
            this.DepartmentRepository = DepartmentRepository;
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

            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            try
            {
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
            return View(department);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Department department)
        {
            try
            {

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
        public ActionResult Delete(int id, Department  department)
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
    }
}
