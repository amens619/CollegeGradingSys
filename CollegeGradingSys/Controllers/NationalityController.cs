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
    public class NationalityController : Controller
    {
        private readonly ICollegeGradingSysRepository<Nationality> _NationalityRepository;

        public NationalityController(ICollegeGradingSysRepository<Nationality> NationalityRepository)
        {
            this._NationalityRepository = NationalityRepository;
        }
        // GET: NationalityController
        public ActionResult Index()
        {
            var nationalites = _NationalityRepository.List();
            return View(nationalites);
        }

        // GET: NationalityController/Details/5
        public ActionResult Details(int id)
        {
            var nationality = _NationalityRepository.Find(id);
            return View(nationality);
        }

        // GET: NationalityController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: NationalityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nationality nationality)
        {
            try
            {
                _NationalityRepository.Add(nationality);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Edit/5
        public ActionResult Edit(int id)
        {
            var nationality = _NationalityRepository.Find(id);
            return View(nationality);
        }

        // POST: NationalityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Nationality nationality)
        {
            try
            {

                _NationalityRepository.Update(id, nationality);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Delete/5
        public ActionResult Delete(int id)
        {
            var nationality = _NationalityRepository.Find(id);
            return View(nationality);
        }

        // POST: NationalityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Nationality  nationality)
        {
            try
            {
                _NationalityRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
