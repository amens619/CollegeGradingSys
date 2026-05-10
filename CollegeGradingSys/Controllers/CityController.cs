using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.City;
using CollegeGradingSys.ViewModels.District;
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
    public class CityController : Controller
    {
        private readonly IGenericService<City> _cityService;
        private readonly IGenericService<District> _districtService;

        public CityController(IGenericService<City> cityService, IGenericService<District> districtService)
        {
            _cityService = cityService;
            _districtService = districtService;
        }
        // GET: CityController
        public async Task<ActionResult> Index()
        {
            var Citys =await _cityService.GetAllAsync();
            
            var vm = Citys.Select(c => new CityIndexVM
            {
                Id = c.Id,
                CityName = c.CityName,
                DistrictName = c.District?.DistrictName
            }).ToList();


            
            return View(vm);
        }

        // GET: CityController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var city =await _cityService.GetByIdAsync(id);


            return View(city);
        }

        // GET: CityController/Create
        public async Task<ActionResult> Create()
        {

            var model = new DistrictCityViewModel
            {
                DistrictsSelectItems = await FillSelectListAsync()
            };

            return View(model);
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DistrictCityViewModel  model)
        {
            try
            {

                if (model.DistrictId == -1)
                {
                    ViewBag.Message = "الرجاء اختيار المديرية من القائمة";

                    return View(await GetDistrictCityViewModelAsync());
                }

                var district =await _districtService.GetByIdAsync(model.DistrictId);
                City   city  = new()
                {
                    Id = model.Id,
                     CityName  = model.CityName,
                     District = district,                    
                };                
                await _cityService.CreateAsync(city);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CityController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var city = await  _cityService.GetByIdAsync(id);
            var governorateId = city.District == null ? city.District.Id = 0 : city.District.Id;
            var model = new DistrictCityViewModel
            { 
                Id = city.Id,
                CityName = city.CityName,
                 DistrictId= governorateId,
                DistrictsSelectItems =await _districtService.GetSelectItemsAsync(b => new SelectItemVM { Id = b.Id, Name = b.DistrictName })
        };
            return View(model);
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,DistrictCityViewModel model)
        {
            try
            {
                var district =await _districtService.GetByIdAsync(model.DistrictId);
                City city = new()
                {
                    Id = model.Id,
                    CityName = model.CityName,
                    District = district,
                };                
                await _cityService.UpdateAsync(city);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CityController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var  city = await _cityService.GetByIdAsync(id);
            
            return View(city);       
        }

        // POST: CityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id,City model)
        {
            try
            {
                await _cityService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        async Task<List<SelectItemVM>> FillSelectListAsync()
        {
            var DistrictsSelectItemVM = await _districtService.GetSelectItemsAsync(b => new SelectItemVM
            {
                Id = b.Id,
                Name = b.DistrictName
            }, "-- أختر --");
            return DistrictsSelectItemVM;
        }

        async Task<DistrictCityViewModel> GetDistrictCityViewModelAsync()
        {
            var vmodel = new DistrictCityViewModel
            {
                 DistrictsSelectItems  = await FillSelectListAsync()
            };
            return vmodel;
        }
    }
}
