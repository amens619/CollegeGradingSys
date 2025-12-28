using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
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
            var district = DistrictRepository.Find(id);
            return View(district);
        }

        // GET: DistrictController/Create
        [Authorize(Policy = "CreateUserPolicy")]
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
                    ViewBag.Message = "الرجاء اختيار المديرية من القائمة";

                    return View(GetAllGovernorates());
                }
                var governorate = GovernorateRepository.Find(model.GovernorateId);
                District  district  = new()
                {
                    Id = model.Id,
                     DistrictName  = model.DistrictName,
                     Governorate = governorate,                    
                };                
                DistrictRepository.Add(district);
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
            var district = DistrictRepository.Find(id);
            var governorateId = district.Governorate == null ? district.Governorate.Id = 0 : district.Governorate.Id;
            var model = new GovernorateDistrictViewModel
            { 
                Id = district.Id,
                DistrictName = district.DistrictName,
                 GovernorateId= governorateId,
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
                var governorate = GovernorateRepository.Find(model.GovernorateId);
                District specialization = new()
                {
                    Id = model.Id,
                    DistrictName = model.DistrictName,
                    Governorate = governorate,
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
            var  district = DistrictRepository.Find(id);
            
            return View(district);       
        }

        // POST: DistrictController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,District model)
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
