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
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

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
                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;
                }
                             
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
           if(currentYear == null)
            {
                ViewBag.Message = "الرجاء ادخال السنة الاكاديمية أولا ";
                return RedirectToAction(nameof(Index));
            }
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
                    IsTerm = true,
                     StudyType =StudyType.انتظام,
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
                    IsTerm = true,
                    StudyType = StudyType.انتظام,
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
            var StAcademicDatasOFStPersonalData = StAcademicDataRepository.List().Where(x => x.StPersonalData.AcademicID == id).ToList();
            if (StAcademicDatasOFStPersonalData != null && StAcademicDatasOFStPersonalData.Count > 0)
            {
               
                ViewBag.Message = "يوجد سجلات اكاديمية تابعة للطالب.. سيتم حذف الطالب وجميع بيانات ودرجاته";
                
            }
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
                    foreach (var StAcademicDataOFStPersonalData in StAcademicDatasOFStPersonalData)
                    {
                        foreach (var CourseGrade in StAcademicDataOFStPersonalData.CourseGrades)
                        {
                            CourseGradeRepository.Delete(CourseGrade.Id);
                        }
                        StAcademicDataRepository.Delete(StAcademicDataOFStPersonalData.Id);
                    }            

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


        
        //==============================================
        public ActionResult ExportAcceptedStToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        {
            var StPersonalDatasR = StPersonalDataRepository.List();

            if (IsSelectCurrentYear == true)
            {
                var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;
                }

            }
            ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            if (AcademicYearId != null)
            {
                StPersonalDatasR = StPersonalDatasR.Where(x => x.EnrollmentYear.Id == AcademicYearId).ToList();
            }
            // Get the user list 
            //var users = GetlistOfUsers();

            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("المقبولين");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 8;
                var row = startRow;

                worksheet.Column(1).Width = 4.71;
                worksheet.Column(2).Width = 38.29;
                worksheet.Column(3).Width = 6.71;
                worksheet.Column(4).Width = 9.29;
                worksheet.Column(5).Width = 17.43;
                worksheet.Column(6).Width = 17.71;
                worksheet.Column(7).Width = 23.86;
                worksheet.Column(8).Width = 21.14;
                worksheet.Column(9).Width = 11;
                worksheet.Column(10).Width = 11.43;
                worksheet.Column(11).Width = 14.57;
                worksheet.Column(12).Width = 11;
                worksheet.Column(13).Width = 11.71;
                worksheet.Column(14).Width = 12.57;
                worksheet.Column(15).Width = 14.14;
                worksheet.Column(16).Width = 24.57;
                //==========================
                worksheet.Row(1).Height = 25;
                worksheet.Row(2).Height = 25;
                worksheet.Row(3).Height = 25;
                worksheet.Row(4).Height = 25;
                worksheet.Row(5).Height = 30;
                worksheet.Row(6).Height = 30;
                worksheet.Row(7).Height = 80;



                //Create Headers and format them
                worksheet.Cells["B1"].Value = "الجمهورية اليمنية";
                worksheet.Cells["B2"].Value = "وزارة التعليم العالي والبحث العلمي";
                worksheet.Cells["B3"].Value = "قطاع الشؤون التعليمية";
                //=============================
                worksheet.Cells["A4:P7"].Style.Font.Size = 18;
                //=============================
                worksheet.Cells["E2"].Value = "كشف الطلاب المقبولين للعام الدراسي:";
                using (var r = worksheet.Cells["E2:H2"])
                {
                    r.Merge = true;                    
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //============================
                var academicYear =  GetCurrentYear();
                worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");
                using (var r = worksheet.Cells["I2:J2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["K2"].Value = "برنامج الدراسة";
                using (var r = worksheet.Cells["K2:L2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //===============================================
                worksheet.Cells["K3"].Value = "برنامج الدراسة";
                using (var r = worksheet.Cells["K3:L3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //===============================================
                using (var r = worksheet.Cells["M2:N3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                }
                //================================
                worksheet.Cells["A4"].Value = "اسم الجامعة";
                using (var r = worksheet.Cells["A4:D4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["E4"].Value = "للعام الدراسي";
                using (var r = worksheet.Cells["E4:J4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["K4"].Value = "Name Of University";
                using (var r = worksheet.Cells["K4:p4"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["A5"].Value = "جامعة الإيمان فرع حضرموت";
                using (var r = worksheet.Cells["A5:D5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["E5"].Value = academicYear.AcademicYearName +"  " + academicYear.AcademicYearNameH;
                using (var r = worksheet.Cells["E5:J5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }
                //================================
                worksheet.Cells["K5"].Value = "";
                using (var r = worksheet.Cells["K5:p5"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                }

                //================================
                worksheet.Cells["A6"].Value = "م";
                using (var r = worksheet.Cells["A6:A7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);

                   
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);
                   
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);
                    
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["B6"].Value = "اسم الطالب";
                using (var r = worksheet.Cells["B6:B7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["C6"].Value = "الجنس";
                using (var r = worksheet.Cells["C6:C7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.TextRotation = 90;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["D6"].Value = "الجنسية";
                using (var r = worksheet.Cells["D6:D7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["E6"].Value = "محل للميلاد";
                using (var r = worksheet.Cells["E6:E7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["F6"].Value = "تاريخ الميلاد";
                using (var r = worksheet.Cells["F6:F7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
 //================================
                worksheet.Cells["G6"].Value = "الرقم الوطني (الهوية)";
                using (var r = worksheet.Cells["G6:G7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
 //================================
                worksheet.Cells["H6"].Value = "رقم القيد";
                using (var r = worksheet.Cells["H6:H7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["I6"].Value = "الكلية";
                using (var r = worksheet.Cells["I6:I7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["J6"].Value = "القسم";
                using (var r = worksheet.Cells["J6:J7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["K6"].Value = "التخصص";
                using (var r = worksheet.Cells["K6:K7"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["L6"].Value = "نظام الدراسة";
                using (var r = worksheet.Cells["L6:L7"])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }
                //================================
                worksheet.Cells["M6"].Value = "بيانات المؤهل السابق";
                using (var r = worksheet.Cells["M6:P6"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(BrownFromHex);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(BrownFromHex);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(BrownFromHex);
                }

                //================================
                worksheet.Cells["M7"].Value = "نوعه";

                worksheet.Cells["M7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["M7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["M7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["M7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["M7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["M7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["M7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["M7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["M7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["M7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

                //================================
                worksheet.Cells["N7"].Value = "تاريخ الحصول عليه";
                worksheet.Cells["N7"].Style.WrapText = true;
                worksheet.Cells["N7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["N7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["N7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["N7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["N7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["N7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["N7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["N7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["N7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["N7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);


                //================================
                worksheet.Cells["O7"].Value = "جهة الحصول عليه";
                worksheet.Cells["O7"].Style.WrapText = true;
                worksheet.Cells["O7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["O7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["O7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["O7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["O7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["O7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["O7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["O7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["O7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);

               //================================
                worksheet.Cells["P7"].Value = "النسبة المئوية %";

                worksheet.Cells["P7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells["P7"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells["P7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["P7"].Style.Fill.BackgroundColor.SetColor(LightYellowFromHex);


                worksheet.Cells["P7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["P7"].Style.Border.Left.Color.SetColor(BrownFromHex);

                worksheet.Cells["P7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["P7"].Style.Border.Right.Color.SetColor(BrownFromHex);

                worksheet.Cells["P7"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["P7"].Style.Border.Bottom.Color.SetColor(BrownFromHex);
               

                row = 8;
                var no = 1;
                foreach (var StPersonalData in StPersonalDatasR)
                {
                    var StAcademicData = StAcademicDataRepository.Find(StPersonalData.StAcademicDatas.FirstOrDefault().Id);

                    worksheet.Cells[row, 1].Value = no;
                    worksheet.Cells[row, 2].Value = StPersonalData.StName;
                    worksheet.Cells[row, 3].Value = StPersonalData.Sex;
                    worksheet.Cells[row, 4].Value = StPersonalData.Nationality.CountryName;
                    worksheet.Cells[row, 5].Value = StPersonalData.BirthGovernorate.GovernorateName;
                    worksheet.Cells[row, 6].Value = StPersonalData.BirthDate.Date.ToString("d");
                    worksheet.Cells[row, 7].Value = StPersonalData.IdentificatioNO;
                    worksheet.Cells[row, 8].Value = StPersonalData.AcademicID;
                    worksheet.Cells[row, 9].Value = StAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 10].Value = StAcademicData.Batch.Specialization.Department.DepartmentName;
                    worksheet.Cells[row, 11].Value = StAcademicData.Batch.Specialization.SpecializationName;
                    worksheet.Cells[row, 12].Value = StAcademicData.StudyType;
                    if (StPersonalData.StHighSchoolData != null)
                    {
                        worksheet.Cells[row, 13].Value = StPersonalData.StHighSchoolData.CertificateType;
                        worksheet.Cells[row, 14].Value = StPersonalData.StHighSchoolData.CertificateYear;
                        worksheet.Cells[row, 15].Value = StPersonalData.StHighSchoolData.Source;
                        worksheet.Cells[row, 16].Value = StPersonalData.StHighSchoolData.Average;
                    }
                    else
                    {
                        worksheet.Cells[row, 13].Value = "";
                        worksheet.Cells[row, 14].Value = "";
                        worksheet.Cells[row, 15].Value = "";
                        worksheet.Cells[row, 16].Value = "";
                    }

                    
                    string modelRange = "A"+ row.ToString()+ ":P" + row.ToString();
                    var modelTable = worksheet.Cells[modelRange];
                    worksheet.Row(row).Height = 25;
                    modelTable.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    modelTable.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Color.SetColor(Color.Black);

                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Color.SetColor(Color.Black);

                    

                    //worksheet.Cells[row, 17].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, 17].Style.Border.Left.Color.SetColor(Color.Black);


                    row++;
                    no++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "Ameen Bashaaib";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "المقبولين.xlsx");
        }
        //==============================================

        public ActionResult ExportSthighSchoolToExcel(bool IsSelectCurrentYear, int? AcademicYearId)
        {
            var StPersonalDatasR = StPersonalDataRepository.List();

            if (IsSelectCurrentYear == true)
            {
                var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
                if (currentYear != null)
                {
                    AcademicYearId = currentYear.Id;
                }

            }
            ViewData["IsSelectCurrentYear"] = IsSelectCurrentYear;
            ViewData["AcademicYearId"] = new SelectList(FillSelectAcademicYearesList("-- الكل --"), "Id", "AcademicYearName", AcademicYearId ?? -1);
            if (AcademicYearId != null)
            {
                StPersonalDatasR = StPersonalDatasR.Where(x => x.EnrollmentYear.Id == AcademicYearId).ToList();
            }
            //================================           
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add(" كشف الطلاب");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 2;                
                var row = startRow;

                worksheet.Column(1).Width = 6;
                worksheet.Column(2).Width = 46.57;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 20;
                worksheet.Column(8).Width = 20;
                worksheet.Column(9).Width = 20;
                worksheet.Column(10).Width = 20;
                worksheet.Column(11).Width = 20;
                worksheet.Column(12).Width = 31.71;
               


                //==========================
                worksheet.Row(1).Height = 58.75;
                worksheet.Row(2).Height = 80;
               




                //Create Headers and format them
                worksheet.Cells["A1:L1"].Value = "بيانات طلاب حضرموت دفعة 1439هـ من واقع شهادة الثانوية";
                using (var r = worksheet.Cells["A1:L1"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Font.Size = 36;
                    r.Style.Font.Name = "Calibri";

                }
               
                //=============================

                worksheet.Cells["A2:L2"].Style.Font.Size = 24;
                
                //=============================
               
               
                //=====================================
                //////var academicYear = GetCurrentYear();
                //////worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");

                //================================

                int rowIndex = 1;
                int colIndex = 7;
                int PixelTop = 17;
                int PixelLeft = 1721;
                int Height = 153;
                int Width = 100;
               
                using (var r = worksheet.Cells["A2:L2"])
                {
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                row = startRow;
                //================================
                worksheet.Cells["A2"].Value = "م";
                //================================
                worksheet.Cells["B2"].Value = "الاسم";
                //================================
                worksheet.Cells["C2"].Value = "محل الميلاد";
                //================================
                worksheet.Cells["D2"].Value = "تاريخ الميلاد";
                //================================
                worksheet.Cells["E2"].Value = "الجنسية";
                //================================
                worksheet.Cells["F2"].Value = "نوع الشهادة";
                //================================
                worksheet.Cells["G2"].Value = "المعدل";
                //================================
                worksheet.Cells["H2"].Value = "مصدرها";
                //================================
                worksheet.Cells["I2"].Value = "رقم الجلوس";
                //================================
                worksheet.Cells["J2"].Value = "تاريخ الالتحاق هـ";
                //================================
                worksheet.Cells["K2"].Value = "تاريخ الالتحاق م";
                //================================
                worksheet.Cells["L2"].Value = "ملاحظة";
                //================================              
                

                var no = 1;
                foreach (var OneSt in StPersonalDatasR)
                {
                    row++;
                    worksheet.Row(row).Height = 50;
                    worksheet.Cells[row, 1].Value = no;

                    //=================================================
                    worksheet.Cells[row, 2].Value = OneSt.StName;
                    
                    //================================================
                    worksheet.Cells[row, 3].Value = OneSt.Birthcountry.CountryName;

                    //================================================
                    worksheet.Cells[row, 4].Value = OneSt.BirthDate.ToShortDateString();

                    //================================================

                    worksheet.Cells[row, 5].Value = OneSt.Nationality.NationalityName;
                    if (OneSt.StHighSchoolData is not null)
                    {
                        //================================================
                        worksheet.Cells[row, 6].Value= OneSt.StHighSchoolData.CertificateType.ToString() ;

                        //================================================
                        worksheet.Cells[row, 7].Value =  OneSt.StHighSchoolData.Average.ToString() ;

                        //================================================
                        worksheet.Cells[row, 8].Value = (OneSt.StHighSchoolData.Source is not null) ? OneSt.StHighSchoolData.Source : "";

                        //================================================
                        worksheet.Cells[row, 9].Value =  OneSt.StHighSchoolData.SeatNo.ToString() ;

                        //================================================
                        worksheet.Cells[row, 12].Value = (OneSt.StHighSchoolData.Note is not null) ? OneSt.StHighSchoolData.Note:"";

                    }
                    //================================================
                    worksheet.Cells[row, 10].Value = OneSt.EnrollmentYear.AcademicYearNameH;

                    //================================================
                    worksheet.Cells[row, 11].Value = OneSt.EnrollmentYear.AcademicYearName;
                   


                    //================================================
                   

                    using (var r = worksheet.Cells[row,1, row, 12])
                    {
                        r.Style.WrapText = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.White);

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Top.Color.SetColor(Color.Black);

                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Color.SetColor(Color.Black);

                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Color.SetColor(Color.Black);

                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Color.SetColor(Color.Black);
                        r.Style.Font.Size = 18;
                    }


                    no++;
                }
               
                    //================================================
                  

                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "بيان درجات من واقع شهادة الثانوية.xlsx");

        }

        List<Governorate> FillSelectGovernoratesList()
        {
            var Governorates = GovernorateRepository.List().ToList();
            Governorates.Insert(0, new Governorate { Id = -1, GovernorateName = "-- أختر --" });

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

        private AcademicYear GetCurrentYear()
        {
            var currentYear = AcademicYearRepository.List().SingleOrDefault(x => x.IsCurrentYear == true);
            return (currentYear);
        }
    }
}
