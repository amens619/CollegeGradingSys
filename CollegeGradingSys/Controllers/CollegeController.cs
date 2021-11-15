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

        public CollegeController(ICollegeGradingSysRepository<College> CollegeRepository)
        {
            this.CollegeRepository = CollegeRepository;
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
            var college = CollegeRepository.Find(id);
            return View(college);
        }

        // POST: CollegeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,College college)
        {
            try
            {

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, College  college)
        {
            try
            {
                CollegeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
