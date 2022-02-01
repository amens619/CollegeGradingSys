using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;

namespace CollegeGradingSys.Controllers
{
    public class StPersonalDataController : Controller
    {
        private readonly ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<Governorate> GovernorateRepository;
        private readonly ICollegeGradingSysRepository<Nationality> NationalityRepository;
        private readonly ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository;
        private readonly ICollegeGradingSysRepository<Batch> BatchRepository;

        public StPersonalDataController(ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository,
            ICollegeGradingSysRepository<Governorate> GovernorateRepository ,
            ICollegeGradingSysRepository<Nationality> NationalityRepository, 
            ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository,
            ICollegeGradingSysRepository<Batch> BatchRepository
            )
        {
            this.StPersonalDataRepository = StPersonalDataRepository;
            this.GovernorateRepository = GovernorateRepository;
            this.NationalityRepository =  NationalityRepository;
            this.StHighSchoolDataRepository = StHighSchoolDataRepository;
            this.BatchRepository = BatchRepository;
        }
        // GET: StPersonalDataController
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string BatchName, int? id, StStatus? StStatus, int? SearchAcademicID, int pageNumber = 1, int pageSize = 5)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SexSortParm = sortOrder == "SexSortParm" ? "SexSortParm_desc" : "SexSortParm";

           

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var StPersonalDatasR = StPersonalDataRepository.List();

            if (!String.IsNullOrEmpty(searchString))
            {
                 StPersonalDatasR = StPersonalDatasR.Where(s => s.StName.Contains(searchString)).ToList();
                    
            }

            if (SearchAcademicID != null)
            {
                StPersonalDatasR = StPersonalDatasR.Where(s => s.AcademicID.Equals(SearchAcademicID)).ToList();

            }

            var StPersonalDatas = StPersonalDatasR
                .Skip(ExcludeRecords).Take(pageSize);

            switch (sortOrder)
            {
                case "name_desc":
                    StPersonalDatas = StPersonalDatas.OrderByDescending(s => s.StName).ToList();
                    break;
                case "SexSortParm":
                    StPersonalDatas = StPersonalDatas.OrderBy(s => s.Sex).ToList();
                    break;
                case "SexSortParm_desc":
                    StPersonalDatas = StPersonalDatas.OrderByDescending(s => s.Sex).ToList();
                    break;
                default:
                    StPersonalDatas = StPersonalDatas.OrderBy(s => s.StName).ToList();
                    break;
            }

           



            var result = new PagedResult<StPersonalData>
            {
                Data = StPersonalDatas.ToList(),
                TotalItems = StPersonalDatasR.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var model = new StPersonalDataFilteringIndexData
            {
                BatchId=-1,
                Batches = FillSelectBatchsList("-- الكل --"),
                //StPersonalDatas = StPersonalDatas.ToList()
                pagedResult = result
            };

            if (id != null)
            {
                model.StHighSchoolData = StHighSchoolDataRepository.Find(id ?? 0);
                ViewData["AcademicID"] = id;
            }
            //BatchNamelist
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);

            ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- الكل --"), "Id", "studentBatchName",-1);
            return View(model);
            
           
        }

        // GET: StPersonalDataController/Details/5
        public ActionResult Details(int id)
        {
            var stPersonalData = StPersonalDataRepository.Find(id);
            return View(stPersonalData);
        }

        // GET: StPersonalDataController/Create
        public ActionResult Create()
        {

            var model = new StPersonalDataViewModel
            {
                BirthDate = new DateTime(2000,1,1), 
                Governorates = FillSelectGovernoratesList(),
                 Nationalities = FillSelectNationalitiesList()
            };

            return View(model);
        }

        // POST: StPersonalDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StPersonalDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (model.GovernorateId == -1)
                    {
                        ViewBag.Message = "الرجاء اختيار المحافظة من القائمة";

                        return View(GetAllStPersonalDatas(model));
                    }
                    if (model.NationalityId == -1)
                    {
                        ViewBag.Message = "الرجاء اختيار الدولة من القائمة";

                        return View(GetAllStPersonalDatas(model));
                    }
                    if (model.BirthPlaceId == -1)
                    {
                        ViewBag.Message = "الرجاء اختيار الدولة من القائمة";

                        return View(GetAllStPersonalDatas(model));
                    }
                    var governorate = GovernorateRepository.Find(model.GovernorateId);
                    var nationality = NationalityRepository.Find(model.NationalityId);
                    var birthPlace = NationalityRepository.Find(model.BirthPlaceId);
                    StPersonalData stPersonalData = new()
                    {
                        AcademicID = model.AcademicID,
                        StName = model.StName,
                        IdentificatioNO = model.IdentificatioNO,
                        Sex = model.Sex,
                        BirthDate = model.BirthDate,
                        Birthcountry = birthPlace,
                        EnrollmentYearH = model.EnrollmentYearH,
                        EnrollmentYearM = model.EnrollmentYearM,
                        Nationality = nationality,
                        BirthGovernorate = governorate,
                    };
                    StPersonalDataRepository.Add(stPersonalData);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View(GetAllStPersonalDatas(model));
           
        }

        // GET: StPersonalDataController/Edit/5
        public ActionResult Edit(int id)
        {
            var stPersonalData = StPersonalDataRepository.Find(id);
            var governorateId = stPersonalData.BirthGovernorate == null ?  0 : stPersonalData.BirthGovernorate.Id;
            var nationalityId = stPersonalData.Nationality == null ?  0 : stPersonalData.Nationality.Id;
            var birthPlaceId = stPersonalData.Birthcountry == null ? 0 : stPersonalData.Birthcountry.Id;
            var model = new StPersonalDataViewModel
            {
                AcademicID = stPersonalData.AcademicID,

                StName = stPersonalData.StName,
                IdentificatioNO = stPersonalData.IdentificatioNO,
                Sex = stPersonalData.Sex,
                BirthDate = stPersonalData.BirthDate,
                BirthPlaceId = birthPlaceId,
                EnrollmentYearH = stPersonalData.EnrollmentYearH,
                EnrollmentYearM = stPersonalData.EnrollmentYearM,
                NationalityId = nationalityId,
                Nationalities = NationalityRepository.List().ToList(),
                GovernorateId = governorateId,
                Governorates = GovernorateRepository.List().ToList()
            };
            return View(model);
        }

        // POST: StPersonalDataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,StPersonalDataViewModel model)
        {
            try
            {
                var governorate = GovernorateRepository.Find(model.GovernorateId);
                var nationality = NationalityRepository.Find(model.NationalityId);
                var birthPlace = NationalityRepository.Find(model.BirthPlaceId);
                StPersonalData stPersonalData = new()
                {
                    AcademicID = model.AcademicID,
                    StName = model.StName,
                    IdentificatioNO = model.IdentificatioNO,
                    Sex = model.Sex,
                    BirthDate = model.BirthDate,
                    Birthcountry = birthPlace,
                    EnrollmentYearH = model.EnrollmentYearH,
                    EnrollmentYearM = model.EnrollmentYearM,
                    Nationality = nationality,
                    BirthGovernorate = governorate,
                };
                StPersonalDataRepository.Update(id, stPersonalData);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StPersonalDataController/Delete/5
        public ActionResult Delete(int id)
        {
            var  stPersonalData = StPersonalDataRepository.Find(id);
            
            return View(stPersonalData);       
        }

        // POST: StPersonalDataController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,StPersonalData model)
        {
            try
            {
                StPersonalDataRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Governorate> FillSelectGovernoratesList()
        {
            var Governorates = GovernorateRepository.List().ToList();
            Governorates.Insert(0, new Governorate { Id = -1,  GovernorateName = "-- أختر --" });

            return Governorates;
        }

        List<Batch> FillSelectBatchsList(string studentBatchName)
        {
            var Batchs = BatchRepository.List().ToList();
            Batchs.Insert(0, new Batch { Id = -1, BatchName = studentBatchName });

            return Batchs;
        }

        List<Nationality> FillSelectNationalitiesList()
        {
            var Nationalities = NationalityRepository.List().ToList();
            Nationalities.Insert(0, new Nationality { Id = -1, NationalityName = "-- أختر --" , CountryName = "-- أختر --" });

            return Nationalities;
        }
        StPersonalDataViewModel GetAllStPersonalDatas(StPersonalDataViewModel  Model)
        {
            Model.Governorates = FillSelectGovernoratesList();
            Model.Nationalities = FillSelectNationalitiesList();            
            return Model;
        }
        public JsonResult GetGovernorate(int Id)
        {
            var GovernoratesList = GovernorateRepository.List().Where(a => a.Nationality.Id == Id);
            return Json(new SelectList(GovernoratesList, "Id", "GovernorateName"));
        }

 
    }
}
