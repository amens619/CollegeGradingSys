using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
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
        public ActionResult Index2()
        {
            var governorates = GovernorateRepository.List();
            
            return View(governorates);
        }

        public ActionResult Index(int pageNumber =1,int pageSize=3)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var governoratesR = GovernorateRepository.List();


           var governorates= governoratesR.OrderBy(s => s.GovernorateName)
                .Skip(ExcludeRecords)
                .Take(pageSize)
                .ToList();


            var result = new PagedResult<Governorate>
            {
                Data = governorates.ToList(),
                TotalItems = governoratesR.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return View(result);
        }
        // GET: GovernorateController/Details/5
        public ActionResult Details(int id)
        {
            var governorate = GovernorateRepository.Find(id);
            return View(governorate);
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
                    ViewBag.Message = "الرجاء اختيار الدولة من القائمة";

                    return View(GetAllNationalities());
                }
                var nationality = NationalityRepository.Find(model.NationalityId);
                Governorate governorate = new Governorate
                {
                    Id = model.Id,
                     GovernorateName  = model.GovernorateName,
                     Nationality = nationality,                    
                };                
                GovernorateRepository.Add(governorate);
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
            var governorate = GovernorateRepository.Find(id);
            if (governorate is null)
            {
                return NotFound();
            }
            var collegeId = governorate.Nationality == null ? governorate.Nationality.Id = 0 : governorate.Nationality.Id;
            var model = new NationalityGovernorateViewModel
            { 
                Id = governorate.Id,
                GovernorateName = governorate.GovernorateName,
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
                var nationality = NationalityRepository.Find(model.NationalityId);
                Governorate governorate = new()
                {
                    Id = model.Id,
                    GovernorateName = model.GovernorateName,
                    Nationality = nationality,
                };                
                GovernorateRepository.Update(id, governorate);
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
            var governorate = GovernorateRepository.Find(id);
            if (governorate is null)
            {
                return NotFound();
            }         
            
            return View(governorate);       
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
            var Nationalities = NationalityRepository.List().ToList();
            Nationalities.Insert(0, new Nationality { Id = -1, CountryName = "-- أختر --" });

            return Nationalities;
        }

        NationalityGovernorateViewModel GetAllNationalities()
        {
            var vmodel = new NationalityGovernorateViewModel
            {
                Nationalities = FillSelectList()
            };
            return vmodel;
        }
    }
}
