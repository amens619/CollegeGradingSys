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
    public class DistrictController : Controller
    {
        private readonly ICollegeGradingSysRepository<District> DistrictRepository;
        private readonly ICollegeGradingSysRepository<Governorate> GovernorateRepository;

        public DistrictController(ICollegeGradingSysRepository<District> DistrictRepository, ICollegeGradingSysRepository<Governorate> GovernorateRepository)
        {
            this.DistrictRepository = DistrictRepository;
            this.GovernorateRepository = GovernorateRepository;
        }
        // GET: DistrictController
        public ActionResult Index()
        {
            var Districts = DistrictRepository.List();
            
            return View(Districts);
        }

        // GET: DistrictController/Details/5
        public ActionResult Details(int id)
        {
            var department = DistrictRepository.Find(id);
            return View(department);
        }

        // GET: DistrictController/Create
        public ActionResult Create()
        {

            var model = new GovernorateDistrictViewModel
            {
                Governorates = FillSelectList()
            };

            return View(model);
        }

        // POST: DistrictController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GovernorateDistrictViewModel  model)
        {
            try
            {

                if (model.GovernorateId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار القسم من القائمة";

                    return View(GetAllGovernorates());
                }
                var department = GovernorateRepository.Find(model.GovernorateId);
                District specialization = new()
                {
                    Id = model.Id,
                     DistrictName  = model.DistrictName,
                     Governorate = department,                    
                };                
                DistrictRepository.Add(specialization);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DistrictController/Edit/5
        public ActionResult Edit(int id)
        {
            var department = DistrictRepository.Find(id);
            var departmentId = department.Governorate == null ? department.Governorate.Id = 0 : department.Governorate.Id;
            var model = new GovernorateDistrictViewModel
            { 
                Id = department.Id,
                DistrictName = department.DistrictName,
                 GovernorateId= departmentId,
                Governorates = GovernorateRepository.List().ToList()
        };
            return View(model);
        }

        // POST: DistrictController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,GovernorateDistrictViewModel model)
        {
            try
            {
                var department = GovernorateRepository.Find(model.GovernorateId);
                District specialization = new()
                {
                    Id = model.Id,
                    DistrictName = model.DistrictName,
                    Governorate = department,
                };                
                DistrictRepository.Update(id, specialization);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DistrictController/Delete/5
        public ActionResult Delete(int id)
        {
            var specialization = DistrictRepository.Find(id);
            
            return View(specialization);       
        }

        // POST: DistrictController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GovernorateDistrictViewModel model)
        {
            try
            {
                DistrictRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Governorate> FillSelectList()
        {
            var Governorates = GovernorateRepository.List().ToList();
            Governorates.Insert(0, new Governorate { Id = -1,  GovernorateName = "-- أختر --" });

            return Governorates;
        }

        GovernorateDistrictViewModel GetAllGovernorates()
        {
            var vmodel = new GovernorateDistrictViewModel
            {
                 Governorates  = FillSelectList()
            };
            return vmodel;
        }
    }
}
