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
using System.Globalization;

namespace CollegeGradingSys.Controllers
{
    public class StPersonalDataController : Controller
    {
        private readonly ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository;
        private readonly ICollegeGradingSysRepository<Governorate> GovernorateRepository;
        private readonly ICollegeGradingSysRepository<Nationality> NationalityRepository;
        private readonly ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository;
        private readonly ICollegeGradingSysRepository<Batch> BatchRepository;
        private readonly ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository;
        private readonly ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository;
        private readonly ICollegeGradingSysRepository<Course> CourseRepository;
        private readonly ICollegeGradingSysRepository<CourseGrade> CourseGradeRepository;

        public StPersonalDataController(ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository,
            ICollegeGradingSysRepository<Governorate> GovernorateRepository ,
            ICollegeGradingSysRepository<Nationality> NationalityRepository, 
            ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository,
            ICollegeGradingSysRepository<Batch> BatchRepository,
            ICollegeGradingSysRepository<StAcademicData> StAcademicDataRepository,
            ICollegeGradingSysRepository<AcademicYear> AcademicYearRepository,
            ICollegeGradingSysRepository<Course> CourseRepository,
            ICollegeGradingSysRepository<CourseGrade> CourseGradeRepository
            )
        {
            this.StPersonalDataRepository = StPersonalDataRepository;
            this.GovernorateRepository = GovernorateRepository;
            this.NationalityRepository =  NationalityRepository;
            this.StHighSchoolDataRepository = StHighSchoolDataRepository;
            this.BatchRepository = BatchRepository;
            this.StAcademicDataRepository = StAcademicDataRepository;
            this.AcademicYearRepository = AcademicYearRepository;
            this.CourseRepository = CourseRepository;
            this.CourseGradeRepository = CourseGradeRepository;
        }
        // GET: StPersonalDataController
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string BatchName, int? id, bool IsSelectCurrentYear, int? AcademicYearId, StStatus? StStatus, int? SearchAcademicID, int pageNumber = 1, int pageSize = 10)
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

            if (IsSelectCurrentYear == true)
            {
                var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                AcademicYearId = currentYear.Id;               
            }
            ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            if (AcademicYearId != null)
            {
                StPersonalDatasR = StPersonalDatasR.Where(x => x.EnrollmentYear.Id == AcademicYearId).ToList();
            }

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
            var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true); 
           
            var model = new StPersonalDataViewModel
            {

                EnrollmentYearId= currentYear.Id,
                EnrollmentYearM = currentYear.AcademicYearName,
                EnrollmentYearH = currentYear.AcademicYearNameH,
                BirthDate = new DateTime(2000,1,1), 
                Governorates = FillSelectGovernoratesList(),
                 Nationalities = FillSelectNationalitiesList()
            };
             ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- أختر --"), "Id", "BatchName");
            return View(model);
        }

        // POST: StPersonalDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StPersonalDataViewModel model)
        {
            var currentYear = AcademicYearRepository.Find(model.EnrollmentYearId);
            model.EnrollmentYearM = currentYear.AcademicYearName;
            model.EnrollmentYearH = currentYear.AcademicYearNameH;



            ModelState.ClearValidationState(nameof(model));
            //if (ModelState.IsValid)
            //{
                try
                {

                    if (model.GovernorateId == -1)
                    {
                        
                        ModelState.AddModelError(nameof(model.GovernorateId), "الرجاء اختيار المحافظة من القائمة");
                        //return View(GetAllStPersonalDatas(model));
                    }
                    if (model.NationalityId == -1)
                    {
                       
                    ModelState.AddModelError(nameof(model.NationalityId), "الرجاء اختيار الدولة من القائمة");
                    //return View(GetAllStPersonalDatas(model));
                }
                    if (model.BirthPlaceId == -1)
                    {
                        
                    ModelState.AddModelError(nameof(model.BirthPlaceId), "الرجاء اختيار الدولة من القائمة");
                    //return View(GetAllStPersonalDatas(model));
                }

                    //=====================================
                   
                    int newAcademicID = 0;
                    if (model.AcademicID is not null)
                    {
                        var cultureInfo = new CultureInfo("en");
                        if (!(int.TryParse(model.AcademicID,
                            NumberStyles.Integer,
                            cultureInfo, out var modelAcademicID)))
                        {
                            ModelState.AddModelError(nameof(model.AcademicID), " الرجاء إدخال  رقم القيد رقماً صحيحا .");

                            //return PartialView("_Create", model);
                        }
                        else { newAcademicID = modelAcademicID; }


                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.AcademicID), " الرجاء إدخال  رقم القيد .");

                        //return PartialView("_Create", model);

                    }

                    if(model.StName== null || (model.StName).Trim() =="")
                    {
                        ModelState.AddModelError(nameof(model.StName), " الرجاء إدخال اسم الطالب  .");
                    }
                    if (model.IdentificatioNO == null || (model.IdentificatioNO).Trim() == "")
                    {
                        ModelState.AddModelError(nameof(model.IdentificatioNO), " الرجاء إدخال رقم الهوية  .");
                    }

                    if (newAcademicID != 0 && IsAcademicIDExists(newAcademicID))
                    {
                        ModelState.AddModelError(nameof(model.AcademicID), "لقد تم إيجاد رقم قيد سابق بنفس الرقم .. الرجاء كتابة رقم آخر ");
                   
                    }
                   
                //============================================
                if (model.BatchId == -1)
                {
                    ModelState.AddModelError(nameof(model.BatchId), "الرجاء اختيار الدفعة من القائمة");                   
                }


                //===========================================

                if (!TryValidateModel(model, nameof(model)))
                {
                    ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- أختر --"), "Id", "BatchName");
                    return View(GetAllStPersonalDatas(model));
                }

                //==================Add StPersonalData to Repository=======================
                var governorate = GovernorateRepository.Find(model.GovernorateId);
                    var nationality = NationalityRepository.Find(model.NationalityId);
                    var birthPlace = NationalityRepository.Find(model.BirthPlaceId);
                    var enrollmentYear = AcademicYearRepository.Find(model.EnrollmentYearId);
                    StPersonalData stPersonalData = new()
                    {
                        AcademicID = newAcademicID,
                        StName = model.StName,
                        IdentificatioNO = model.IdentificatioNO,
                        Sex = model.Sex,
                        BirthDate = model.BirthDate,
                        Birthcountry = birthPlace,
                        EnrollmentYear = enrollmentYear,
                        Nationality = nationality,
                        BirthGovernorate = governorate,
                        StHighSchoolData = model.StHighSchoolData,
                          
                    };
                    StPersonalDataRepository.Add(stPersonalData);
                //========================================================
                //========================Add StAcademicData to Repository  Term.الأول,.================================

                stPersonalData = StPersonalDataRepository.Find(newAcademicID);
                var studentBatch = BatchRepository.Find(model.BatchId);
                var academicYear = enrollmentYear;

                var stAcademicData = new StAcademicData()
                {
                    StLevel =  Level.الأول,
                    Term = Term.الأول,
                    StStatus =  StStatus.مقيد,                  
                    Valuation =  Valuation.غير_محدد,
                    IsCurrentYear = true,
                    StPersonalData = stPersonalData,
                    AcademicYear = academicYear,
                    Batch = studentBatch
                };


                StAcademicDataRepository.Add(stAcademicData);
                //=================================================
                //===================Add CourseGrade to Repository Term.الأول,.==============================

                AddCourseGradeTostAcademicData(stAcademicData);



                //========================Add StAcademicData to Repository  Term.الثاني.================================
                var stAcademicData2 = new StAcademicData()
                {
                    StLevel = Level.الأول,
                    Term = Term.الثاني,
                    StStatus = StStatus.مقيد,
                    Valuation = Valuation.غير_محدد,
                    IsCurrentYear = true,
                    StPersonalData = stPersonalData,
                    AcademicYear = academicYear,
                    Batch = studentBatch
                };
                
                StAcademicDataRepository.Add(stAcademicData2);
                //===================Add CourseGrade to Repository Term.الثاني.==============================


                AddCourseGradeTostAcademicData(stAcademicData2);
                //========================================================

                return RedirectToAction(nameof(Index), new { IsSelectCurrentYear = true }  );
                }
                catch
                {
                    return View();
                }
            //}
            //return View(GetAllStPersonalDatas(model));
           
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
                AcademicID = stPersonalData.AcademicID.ToString(),
                 
                StName = stPersonalData.StName,
                IdentificatioNO = stPersonalData.IdentificatioNO,
                Sex = stPersonalData.Sex,
                BirthDate = stPersonalData.BirthDate,
                BirthPlaceId = birthPlaceId,
                EnrollmentYearH = stPersonalData.EnrollmentYear.AcademicYearNameH,
                EnrollmentYearM = stPersonalData.EnrollmentYear.AcademicYearName,
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

                //=====================================

                int newAcademicID = 0;
                if (model.AcademicID is not null)
                {
                    var cultureInfo = new CultureInfo("en");
                    if (!(int.TryParse(model.AcademicID,
                        NumberStyles.Integer,
                        cultureInfo, out var modelAcademicID)))
                    {
                        ModelState.AddModelError(nameof(model.AcademicID), " الرجاء إدخال  رقم القيد رقماً صحيحا .");

                        //return PartialView("_Create", model);
                    }
                    else { newAcademicID = modelAcademicID; }


                }
                else
                {
                    ModelState.AddModelError(nameof(model.AcademicID), " الرجاء إدخال  رقم القيد رقماً صحيحا .");

                    //return PartialView("_Create", model);

                }

                //==================================================

               

              
               

                if (model.StName == null || (model.StName).Trim() == "")
                {
                    ModelState.AddModelError(nameof(model.StName), " الرجاء إدخال اسم الطالب  .");
                }
                if (model.IdentificatioNO == null || (model.IdentificatioNO).Trim() == "")
                {
                    ModelState.AddModelError(nameof(model.IdentificatioNO), " الرجاء إدخال رقم الهوية  .");
                }
                //===========================================

                if (!TryValidateModel(model, nameof(model)))
                {
                    ViewData["BatchId"] = new SelectList(FillSelectBatchsList("-- أختر --"), "Id", "BatchName");
                    return View(GetAllStPersonalDatas(model));
                }

                //===================================================
                var governorate = GovernorateRepository.Find(model.GovernorateId);
                var nationality = NationalityRepository.Find(model.NationalityId);
                var birthPlace = NationalityRepository.Find(model.BirthPlaceId);
                var enrollmentYear = AcademicYearRepository.Find(model.EnrollmentYearId);
                StPersonalData stPersonalData = new()
                {
                    AcademicID = newAcademicID,
                    StName = model.StName,
                    IdentificatioNO = model.IdentificatioNO,
                    Sex = model.Sex,
                    BirthDate = model.BirthDate,
                    Birthcountry = birthPlace,
                    EnrollmentYear = enrollmentYear,                    
                    Nationality = nationality,
                    BirthGovernorate = governorate,
                };
                StPersonalDataRepository.Update(id, stPersonalData);
                return RedirectToAction(nameof(Details), new { IsSelectCurrentYear = true, id = newAcademicID });
            }
            catch
            {
                return NotFound();
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
        public ActionResult Delete(int AcademicID, StPersonalData model)
        {
            try
            {
                var StAcademicDatasOFStPersonalData = StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == model.AcademicID).ToList();
                if (StAcademicDatasOFStPersonalData != null && StAcademicDatasOFStPersonalData.Count > 0)
                {
                    var department = StPersonalDataRepository.Find(AcademicID);
                    ViewBag.Message = "لا يمكن حذف الطالب بسبب وجود سجلات اكاديمية تابعة له.. الرجاء حذف السجلات التابعة له أولا ";
                    return View(department);
                }
                StHighSchoolDataRepository.Delete(AcademicID);
                StPersonalDataRepository.Delete(AcademicID);
                return RedirectToAction(nameof(Index),new { IsSelectCurrentYear = true });
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
            var Batchs = BatchRepository.List().OrderByDescending(x => x.Id).ToList();
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

        List<AcademicYear> FillSelectAcademicYearesList(string academicYearName)
        {
            var AcademicYeares = AcademicYearRepository.List().ToList();
            AcademicYeares.Insert(0, new AcademicYear { Id = -1, AcademicYearName = academicYearName });

            return AcademicYeares;
        }

        private bool IsAcademicIDExists(int academicID)
        {
            return StPersonalDataRepository.List().Any(e => e.AcademicID == academicID);
        }

       private void AddCourseGradeTostAcademicData(StAcademicData stAcademicData)
        {
            var stAcCourses = CourseRepository.List().Where(x => x.Level == stAcademicData.StLevel).Where(x => x.Term == stAcademicData.Term).Where(x => x.Specialization == stAcademicData.Batch.Specialization).ToList();
            foreach (var course in stAcCourses)
            {

                var courseGrade = new CourseGrade()
                {
                    Course = course,
                    CourseType = true,
                    StAcademicData = stAcademicData,
                    StStatusForCourse = StStatusForCourse.غير_محدد,
                };
                CourseGradeRepository.Add(courseGrade);
            }
        }

    }
}
