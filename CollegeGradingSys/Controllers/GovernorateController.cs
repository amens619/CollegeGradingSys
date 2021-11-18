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
    public class GovernorateController : Controller
    {
        private readonly ICollegeGradingSysRepository<Governorate> GovernorateRepository;
        private readonly ICollegeGradingSysRepository<Nationality> NationalityRepository;

        public GovernorateController(ICollegeGradingSysRepository<Governorate> GovernorateRepository, ICollegeGradingSysRepository<Nationality> NationalityRepository)
        {
            this.GovernorateRepository = GovernorateRepository;
            this.NationalityRepository = NationalityRepository;
        }
        // GET: GovernorateController
        public ActionResult Index()
        {
            var departments = GovernorateRepository.List();
            
            return View(departments);
        }

        // GET: GovernorateController/Details/5
        public ActionResult Details(int id)
        {
            var department = GovernorateRepository.Find(id);
            return View(department);
        }

        // GET: GovernorateController/Create
        public ActionResult Create()
        {

            var model = new NationalityGovernorateViewModel
            {
                Nationalities = FillSelectList()
            };

            return View(model);
        }

        // POST: GovernorateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NationalityGovernorateViewModel  model)
        {
            try
            {

                if (model.NationalityId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار الكلية من القائمة";

                    return View(GetAllNationalitys());
                }
                var college = NationalityRepository.Find(model.NationalityId);
                Governorate department = new Governorate
                {
                    Id = model.Id,
                     GovernorateName  = model.GovernorateName,
                     Nationality = college,                    
                };                
                GovernorateRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GovernorateController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var department = GovernorateRepository.Find(id);
            if (department is null)
            {
                return NotFound();
            }
            var collegeId = department.Nationality == null ? department.Nationality.Id = 0 : department.Nationality.Id;
            var model = new NationalityGovernorateViewModel
            { 
                Id = department.Id,
                GovernorateName = department.GovernorateName,
                 NationalityId= collegeId,
                Nationalities = NationalityRepository.List().ToList()
        };
            return View(model);
        }

        // POST: GovernorateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,NationalityGovernorateViewModel model)
        {
            try
            {
                var college = NationalityRepository.Find(model.NationalityId);
                Governorate department = new()
                {
                    Id = model.Id,
                    GovernorateName = model.GovernorateName,
                    Nationality = college,
                };                
                GovernorateRepository.Update(id, department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GovernorateController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var department = GovernorateRepository.Find(id);
            if (department is null)
            {
                return NotFound();
            }         
            
            return View(department);       
        }

        // POST: GovernorateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, NationalityGovernorateViewModel model)
        {
            try
            {
                GovernorateRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Nationality> FillSelectList()
        {
            var Nationalitys = NationalityRepository.List().ToList();
            Nationalitys.Insert(0, new Nationality { Id = -1,  NationalityName = "-- أختر --" });

            return Nationalitys;
        }

        NationalityGovernorateViewModel GetAllNationalitys()
        {
            var vmodel = new NationalityGovernorateViewModel
            {
                Nationalities = FillSelectList()
            };
            return vmodel;
        }
    }
}
