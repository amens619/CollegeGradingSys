using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Globalization;
using CollegeGradingSys.Repositories.Interfaces;

namespace CollegeGradingSys.Controllers
{
    public class StHighSchoolDataController : Controller
    {
        private readonly ICollegeGradingSysRepository<StHighSchoolData> _StHighSchoolDataRepository;
        private readonly ICollegeGradingSysRepository<StPersonalData> _StPersonalDataRepository;

        public StHighSchoolDataController(ICollegeGradingSysRepository<StHighSchoolData> StHighSchoolDataRepository,ICollegeGradingSysRepository<StPersonalData> StPersonalDataRepository)
        {
            
            _StHighSchoolDataRepository = StHighSchoolDataRepository;
            _StPersonalDataRepository = StPersonalDataRepository;
        }

        // GET: StHighSchoolData
        public  IActionResult Index()
        {
            var stHighSchoolDatas = _StHighSchoolDataRepository.List();            
            return View(stHighSchoolDatas);
        }

        // GET: StHighSchoolData/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
          
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = new StHighSchoolDataVM();
            model.AcademicID = id ?? 0;
            

            //ViewData["AcademicID"] = new SelectList(_context.StPersonalData, "AcademicID", "EnrollmentYearH");
            return PartialView("_Create", model);
        }

        // POST: StHighSchoolData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolDataVM model)
        {
            ModelState.ClearValidationState(nameof(model));
            float newAverage = 0;
                if (model.Average is not null)
                {                    
                    var cultureInfo = new CultureInfo("en");
                    if (!(decimal.TryParse(model.Average,
                        NumberStyles.AllowDecimalPoint,
                        cultureInfo, out var modelAverage)) || !(modelAverage >= 0 && modelAverage <= 100))
                    {
                        ModelState.AddModelError(nameof(model.Average), " الرجاء إدخال المعدل رقماً  بين 0 - 100.");

                        //return PartialView("_Edit", model);
                    }
                    newAverage = (float)modelAverage;
                   
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Average), " الرجاء إدخال المعدل رقماً  بين 0 - 100.");
                }

                
                int newCertificateYear = 0;
                if (model.CertificateYear is not null)
                {
                    var cultureInfo = new CultureInfo("en");
                    if (!(int.TryParse(model.CertificateYear,
                        NumberStyles.AllowDecimalPoint,
                        cultureInfo, out var modelCertificateYear)))
                    {
                        ModelState.AddModelError(nameof(model.CertificateYear), " الرجاء إدخال سنة الشهادة رقماً .");

                        //return PartialView("_Edit", model);
                    }
                    newCertificateYear = (int)modelCertificateYear;

                }
                else
                {
                    ModelState.AddModelError(nameof(model.CertificateYear), " الرجاء إدخال سنة الشهادة .");
                }

                int newSeatNo = 0;
                if (model.SeatNo is not null)
                {
                    var cultureInfo = new CultureInfo("en");
                    if (!(int.TryParse(model.SeatNo,
                        NumberStyles.AllowDecimalPoint,
                        cultureInfo, out var modelSeatNo)))
                    {
                        ModelState.AddModelError(nameof(model.SeatNo), " الرجاء ادخال رقم الجلوس رقماً .");

                        //return PartialView("_Edit", model);
                    }
                    newSeatNo = (int)modelSeatNo;

                }
                else
                {
                    ModelState.AddModelError(nameof(model.SeatNo), " الرجاء ادخال رقم الجلوس .");
                }

                if (!TryValidateModel(model, nameof(model)))
                {                   
                    return PartialView("_Create", model);
                }


                var stHighSchoolData = new StHighSchoolData()
                {
                    AcademicID = model.AcademicID,
                    Average = newAverage,
                    CertificateType = model.CertificateType,
                    CertificateYear = newCertificateYear,
                    HighSchoolName = model.HighSchoolName,
                    SeatNo = newSeatNo,
                    Source = model.Source,
                    Note = model.Note,
                    StPersonalData = model.StPersonalData
                };
            try
            {
                _StHighSchoolDataRepository.Add(stHighSchoolData);
                return PartialView("_Create", model);
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!StHighSchoolDataExists(stHighSchoolData.AcademicID))
                //{
                //    return NotFound();
                //}
                //else
                //{
                throw;
                //}
            }

        }

        // GET: StHighSchoolData/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StHighSchoolData stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
            if (stHighSchoolData == null)
            {
                return NotFound();
            }
            var model = new StHighSchoolDataVM()
            {
                AcademicID = stHighSchoolData.AcademicID,
                Average = stHighSchoolData.Average.ToString(),
                CertificateType = stHighSchoolData.CertificateType,
                CertificateYear = stHighSchoolData.CertificateYear.ToString(),
                HighSchoolName = stHighSchoolData.HighSchoolName,
                SeatNo = stHighSchoolData.SeatNo.ToString(),
                Source = stHighSchoolData.Source,
                StPersonalData = stHighSchoolData.StPersonalData,
                Note = stHighSchoolData.Note
            };

            return PartialView("_Edit", model); 
        }

        // POST: StHighSchoolData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("AcademicID,CertificateType,Average,Source,SeatNo,CertificateYear,HighSchoolName,Note")] StHighSchoolDataVM model)
        {
            if (id != model.AcademicID)
            {
                return NotFound();
            }

            
                
                    ModelState.ClearValidationState(nameof(model));
                    float newAverage = 0;
                    if (model.Average is not null)
                    {
                        var cultureInfo = new CultureInfo("en");
                        if (!(decimal.TryParse(model.Average,
                            NumberStyles.AllowDecimalPoint,
                            cultureInfo, out var modelAverage)) || !(modelAverage >= 0 && modelAverage <= 100))
                        {
                            ModelState.AddModelError(nameof(model.Average), " الرجاء إدخال المعدل رقماً  بين 0 - 100.");

                            //return PartialView("_Edit", model);
                        }
                        newAverage = (float)modelAverage;

                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Average), " الرجاء إدخال المعدل رقماً  بين 0 - 100.");
                    }


                    int newCertificateYear = 0;
                    if (model.CertificateYear is not null)
                    {
                        var cultureInfo = new CultureInfo("en");
                        if (!(int.TryParse(model.CertificateYear,
                            NumberStyles.AllowDecimalPoint,
                            cultureInfo, out var modelCertificateYear)))
                        {
                            ModelState.AddModelError(nameof(model.CertificateYear), " الرجاء إدخال سنة الشهادة رقماً .");

                            //return PartialView("_Edit", model);
                        }
                        newCertificateYear = (int)modelCertificateYear;

                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.CertificateYear), " الرجاء إدخال سنة الشهادة .");
                    }

                    int newSeatNo = 0;
                    if (model.SeatNo is not null)
                    {
                        var cultureInfo = new CultureInfo("en");
                        if (!(int.TryParse(model.SeatNo,
                            NumberStyles.AllowDecimalPoint,
                            cultureInfo, out var modelSeatNo)))
                        {
                            ModelState.AddModelError(nameof(model.SeatNo), " الرجاء ادخال رقم الجلوس رقماً .");

                            //return PartialView("_Edit", model);
                        }
                        newSeatNo = (int)modelSeatNo;

                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.SeatNo), " الرجاء ادخال رقم الجلوس .");
                    }

                    if (!TryValidateModel(model, nameof(model)))
                    {
                    return PartialView("_Edit", model);
                }


                    var stHighSchoolData = new StHighSchoolData()
                    {
                        AcademicID = model.AcademicID,
                        Average = newAverage,
                        CertificateType = model.CertificateType,
                        CertificateYear = newCertificateYear,
                        HighSchoolName = model.HighSchoolName,
                        SeatNo = newSeatNo,
                        Source = model.Source,
                        Note = model.Note,
                        StPersonalData = model.StPersonalData
                    };
                try
                {
                    _StHighSchoolDataRepository.Update(id, stHighSchoolData);
                return PartialView("_Edit", model);
            }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!StHighSchoolDataExists(stHighSchoolData.AcademicID))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                        throw;
                    //}
                }

                
                      
            
        }

        // GET: StHighSchoolData/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stHighSchoolData = _StHighSchoolDataRepository.Find(id ?? 1);
                
            if (stHighSchoolData == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", stHighSchoolData);
        }

        // POST: StHighSchoolData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            
            _StHighSchoolDataRepository.Delete(id);

            return PartialView("_Delete");

        }

        //private bool StHighSchoolDataExists(int id)
        //{
        //    return _context.StHighSchoolData.Any(e => e.AcademicID == id);
        //}
    }
}
