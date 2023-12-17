using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
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
    public class NationalityController : Controller
    {
        private readonly ICollegeGradingSysRepository<Nationality> _NationalityRepository;
        private readonly ICollegeGradingSysRepository<Governorate> _GovernorateRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;

        public NationalityController(ICollegeGradingSysRepository<Nationality> NationalityRepository,
            ICollegeGradingSysRepository<Governorate> GovernorateRepository,
            ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository)
        {
            this._NationalityRepository = NationalityRepository;
            _GovernorateRepository = GovernorateRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
        }
        // GET: NationalityController
        public ActionResult Index()
        {
            var nationalites = _NationalityRepository.List();
            return View(nationalites);
        }

        // GET: NationalityController/Details/5
        //public ActionResult Details(int id)
        //{
        //    var nationality = _NationalityRepository.Find(id);
        //    return View(nationality);
        //}

        // GET: NationalityController/Create
        [Authorize(Policy = "CreateNationalityPolicy")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: NationalityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateNationalityPolicy")]
        public ActionResult Create(Nationality nationality)
        {
            try
            {                
                if (nationality.CountryName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(nationality.CountryName), " الرجاء كتابة اسم الدولة");

                    return View(nationality);
                }
                if (nationality.NationalityName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(nationality.NationalityName), " الرجاء كتابة اسم الجنسية");

                    return View(nationality);
                }

                if (isCountryNameExists((nationality.CountryName).Trim()))
                {
                    ModelState.AddModelError(nameof(nationality.CountryName), "لقد تم إيجاد دولة سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(nationality);
                }

                if (isNationalityNameExists((nationality.NationalityName).Trim()))
                {
                    ModelState.AddModelError(nameof(nationality.NationalityName), "لقد تم إيجاد جنسية سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(nationality);
                }
                
                _NationalityRepository.Add(nationality);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Edit/5
        [Authorize(Policy = "EditNationalityPolicy")]
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var nationality = _NationalityRepository.Find(id);
            if (nationality is null)
            {
                return NotFound();
            }
            
            return View(nationality);
        }

        // POST: NationalityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditNationalityPolicy")]
        public ActionResult Edit(int id,Nationality model)
        {
            try
            {
                if (model.CountryName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.CountryName), " الرجاء كتابة اسم الدولة");

                    return View(model);
                }
                if (model.NationalityName == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(nameof(model.NationalityName), " الرجاء كتابة اسم الجنسية");

                    return View(model);
                }


                var nationality1 = _NationalityRepository.List().SingleOrDefault(x => x.CountryName == model.CountryName);
                if (nationality1 != null && nationality1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.CountryName), "لقد تم إيجاد دولة سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(model);
                }

                var nationality2 = _NationalityRepository.List().SingleOrDefault(x => x.NationalityName == model.NationalityName);
                if (nationality2 != null && nationality2.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.NationalityName), "لقد تم إيجاد جنسية سابقة بنفس اسم .. الرجاء كتابة اسم آخر ");
                    return View(model);
                }
                var nationality = _NationalityRepository.Find(model.Id);
                nationality.CountryName = model.CountryName;
                nationality.NationalityName = model.NationalityName;

                _NationalityRepository.Update(id, nationality);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Delete/5
        [Authorize(Policy = "DeleteNationalityPolicy")]
        public ActionResult Delete(int id)
        {
            var nationality = _NationalityRepository.Find(id);
            return View(nationality);
        }

        // POST: NationalityController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteNationalityPolicy")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var GovernoratesOFNationality = _GovernorateRepository.List().Where(x => x.Nationality.Id == id).ToList();
                if (GovernoratesOFNationality != null && GovernoratesOFNationality.Count > 0)
                {
                    var nationality = _NationalityRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف الدولة بسبب وجود محافظات تابعة لها.. الرجاء حذف المحافظات التابعة لها أولا ";
                    return View(nationality);

                }
                var StPersonalDatasOFNationality = _StPersonalDataRepository.List().Where(x => x.Nationality.Id == id || x.Birthcountry.Id == id).ToList();
                if (StPersonalDatasOFNationality != null && StPersonalDatasOFNationality.Count > 0)
                {
                    var nationality = _NationalityRepository.Find(id);
                    ViewBag.Message = "لا يمكن حذف الدولة بسبب وجود بيانات طلاب تابعة لها.. الرجاء حذف البيانات التابعة لها أولا ";
                    return View(nationality);
                }
                _NationalityRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool isCountryNameExists(string countryName)
        {
            return _NationalityRepository.List().Any(e =>  e.CountryName == countryName);
        }

        private bool isNationalityNameExists(string nationalityName)
        {
            return _NationalityRepository.List().Any(e => e.NationalityName == nationalityName );
        }
    }
}
