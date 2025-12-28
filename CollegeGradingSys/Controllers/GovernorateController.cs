using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using CollegeGradingSys.Repositories.Interfaces;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class GovernorateController : Controller
    {
        private readonly ICollegeGradingSysRepository<Governorate> GovernorateRepository;
        private readonly ICollegeGradingSysRepository<Nationality> NationalityRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<District> DistrictRepository;

        public GovernorateController(ICollegeGradingSysRepository<Governorate> GovernorateRepository,
            ICollegeGradingSysRepository<Nationality> NationalityRepository,
            ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository,
            ICollegeGradingSysRepository<District> DistrictRepository)
        {
            this.GovernorateRepository = GovernorateRepository;
            this.NationalityRepository = NationalityRepository;
            this.StPersonalDataRepository = StPersonalDataRepository;
            this.DistrictRepository = DistrictRepository;
        }
        // GET: GovernorateController
        public ActionResult Index2()
        {
            var governorates = GovernorateRepository.List();
            
            return View(governorates);
        }

        public ActionResult Index(int pageNumber =1,int pageSize=5)
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
        [Authorize(Policy = "CreateGovernoratePolicy")]
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
        [Authorize(Policy = "CreateGovernoratePolicy")]
        public ActionResult Create(NationalityGovernorateViewModel  model)
        {
            try
            {
               
                if (model.GovernorateName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.GovernorateName), " الرجاء كتابة اسم المحافظة/المنطقة");

                    return View(GetAllNationalities());
                }

                if (isGovernorateNameExists((model.GovernorateName).Trim()))
                {
                    ModelState.AddModelError(nameof(model.GovernorateName), "لقد تم إيجاد محافظة/منطقة سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(GetAllNationalities());
                }
               
               

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
        [Authorize(Policy = "EditGovernoratePolicy")]
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
            var nationalityId =  governorate.Nationality.Id;
            var model = new NationalityGovernorateViewModel
            { 
                Id = governorate.Id,
                GovernorateName = governorate.GovernorateName,
                 NationalityId= nationalityId,
                Nationalities = NationalityRepository.List().ToList()
            };
            return View(model);
        }

        // POST: GovernorateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditGovernoratePolicy")]
        public ActionResult Edit(int id,NationalityGovernorateViewModel model)
        {
            try
            {

                if (model.GovernorateName == null)
                {

                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.GovernorateName), " الرجاء كتابة اسم المحافظة/المنطقة");

                    return View(GetAllNationalities(model.NationalityId));
                }

                var department1 = GovernorateRepository.List().SingleOrDefault(x => x.GovernorateName == model.GovernorateName);
                if (department1 != null && department1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.GovernorateName), "لقد تم إيجاد محافظة/منطقة سابقة بنفس اسم .. الرجاء كتابة اسم آخر");
                    return View(GetAllNationalities(model.NationalityId));
                }
                var nationality = NationalityRepository.Find(model.NationalityId);
                var governorate = GovernorateRepository.Find(model.Id);

                governorate.GovernorateName = model.GovernorateName;
                governorate.Nationality = nationality;
                             
                GovernorateRepository.Update(id, governorate);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GovernorateController/Delete/5
        [Authorize(Policy = "DeleteGovernoratePolicy")]
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteGovernoratePolicy")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var StPersonalDatasOFBirthGovernorate = StPersonalDataRepository.List().Where(x => x.BirthGovernorate.Id == id).ToList();
                if (StPersonalDatasOFBirthGovernorate != null && StPersonalDatasOFBirthGovernorate.Count > 0)
                {
                    var governorate = GovernorateRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف المحافظة بسبب وجود بيانات طلاب تابعة لها.. الرجاء حذف البيانات التابعة لها أولا ";
                    return View(governorate);
                }
                //var DistrictsOFGovernorate = DistrictRepository.List().Where(x => x.Governorate.Id == id).ToList();
                //if (DistrictsOFGovernorate != null && DistrictsOFGovernorate.Count > 0)
                //{
                //    var governorate = GovernorateRepository.Find(id);
                //    ViewBag.Message = "لا يمكن حذف المحافظة بسبب وجود مديرية تابعة لها.. الرجاء حذف المديرية التابعة له أولا ";
                //    return View(governorate);
                //}
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

        NationalityGovernorateViewModel GetAllNationalities(int nationalityId)
        {
            var vmodel = new NationalityGovernorateViewModel
            {
                 NationalityId = nationalityId,
                Nationalities = FillSelectList()
            };
            return vmodel;
        }

        private bool isGovernorateNameExists(string governorateName)
        {
            return GovernorateRepository.List().Any(e => e.GovernorateName == governorateName);
        }
    }
}
