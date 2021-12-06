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
    public class CityController : Controller
    {
        private readonly ICollegeGradingSysRepository<City> CityRepository;
        private readonly ICollegeGradingSysRepository<District> DistrictRepository;

        public CityController(ICollegeGradingSysRepository<City> CityRepository, ICollegeGradingSysRepository<District> DistrictRepository)
        {
            this.CityRepository = CityRepository;
            this.DistrictRepository = DistrictRepository;
        }
        // GET: CityController
        public ActionResult Index()
        {
            var Citys = CityRepository.List();
            
            return View(Citys);
        }

        // GET: CityController/Details/5
        public ActionResult Details(int id)
        {
            var city = CityRepository.Find(id);
            return View(city);
        }

        // GET: CityController/Create
        public ActionResult Create()
        {

            var model = new DistrictCityViewModel
            {
                Districts = FillSelectList()
            };

            return View(model);
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DistrictCityViewModel  model)
        {
            try
            {

                if (model.DistrictId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار المديرية من القائمة";

                    return View(GetAllDistricts());
                }

                var district = DistrictRepository.Find(model.DistrictId);
                City   city  = new()
                {
                    Id = model.Id,
                     CityName  = model.CityName,
                     District = district,                    
                };                
                CityRepository.Add(city);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CityController/Edit/5
        public ActionResult Edit(int id)
        {
            var city = CityRepository.Find(id);
            var governorateId = city.District == null ? city.District.Id = 0 : city.District.Id;
            var model = new DistrictCityViewModel
            { 
                Id = city.Id,
                CityName = city.CityName,
                 DistrictId= governorateId,
                Districts = DistrictRepository.List().ToList()
        };
            return View(model);
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,DistrictCityViewModel model)
        {
            try
            {
                var district = DistrictRepository.Find(model.DistrictId);
                City city = new()
                {
                    Id = model.Id,
                    CityName = model.CityName,
                    District = district,
                };                
                CityRepository.Update(id, city);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CityController/Delete/5
        public ActionResult Delete(int id)
        {
            var  city = CityRepository.Find(id);
            
            return View(city);       
        }

        // POST: CityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,City model)
        {
            try
            {
                CityRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<District> FillSelectList()
        {
            var Districts = DistrictRepository.List().ToList();
            Districts.Insert(0, new District { Id = -1,  DistrictName = "-- أختر --" });

            return Districts;
        }

        DistrictCityViewModel GetAllDistricts()
        {
            var vmodel = new DistrictCityViewModel
            {
                 Districts  = FillSelectList()
            };
            return vmodel;
        }
    }
}
