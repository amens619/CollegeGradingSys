using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.StHighSchoolData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class StHighSchoolDataController : Controller
    {
        private readonly IStHighSchoolDataService _stHighSchoolDataService;
        public StHighSchoolDataController(IStHighSchoolDataService stHighSchoolDataService)
        {
            _stHighSchoolDataService = stHighSchoolDataService;
        }

       
        // GET: StHighSchoolData
        public async Task<IActionResult> Index()
        {
            var stHighSchoolDatas = await _stHighSchoolDataService.GetAllAsync();
            return View(stHighSchoolDatas);
        }

        // GET: StHighSchoolData/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var stHighSchoolData = await _stHighSchoolDataService.GetByIdAsync(id.Value);

            if (stHighSchoolData == null) return NotFound();

            return View(stHighSchoolData);
        }

        // GET: StHighSchoolData/Create
        public IActionResult Create(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var model = new StHighSchoolDataVM
            {
                AcademicID = id.Value
            };

            return PartialView("_Create", model);
        }

     
        // POST: StHighSchoolData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StHighSchoolDataVM model)
        {
            // الآن ModelState سيعمل بشكل تلقائي ومثالي بناءً على خصائص الـ VM
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }

            try
            {
                var stHighSchoolData = new StHighSchoolData
                {
                    AcademicID = model.AcademicID,
                    CertificateType = model.CertificateType,

                    // نستخدم .Value لأن الخصائص في المودل أصبحت Nullable (float? و int?)
                    // ونحن متأكدون أنها تحتوي على قيم لأن ModelState.IsValid قد مر بنجاح
                    Average = model.Average.Value,
                    CertificateYear = model.CertificateYear.Value,
                    SeatNo = model.SeatNo.Value,

                    HighSchoolName = model.HighSchoolName,
                    Source = model.Source,
                    Note = model.Note
                };

                await _stHighSchoolDataService.CreateAsync(stHighSchoolData);

                return PartialView("_Create", model);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return PartialView("_Create", model);
            }
        }
       
        // GET: StHighSchoolData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var stHighSchoolData = await _stHighSchoolDataService.GetByIdAsync(id.Value);

            if (stHighSchoolData == null) return NotFound();

            var model = new StHighSchoolDataVM
            {
                AcademicID = stHighSchoolData.AcademicID,
                CertificateType = stHighSchoolData.CertificateType,

                // التعيين المباشر للأرقام دون الحاجة لـ ToString()
                Average = stHighSchoolData.Average,
                CertificateYear = stHighSchoolData.CertificateYear,
                SeatNo = stHighSchoolData.SeatNo,

                HighSchoolName = stHighSchoolData.HighSchoolName,
                Source = stHighSchoolData.Source,
                Note = stHighSchoolData.Note
            };

            return PartialView("_Edit", model);
        }
       

        // POST: StHighSchoolData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StHighSchoolDataVM model)
        {
            if (id != model.AcademicID) return NotFound();

            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", model);
            }

            try
            {
                var stHighSchoolData = await _stHighSchoolDataService.GetByIdAsync(id);
                if (stHighSchoolData == null) return NotFound();

                // تحديث البيانات باستخدام القيم النظيفة
                stHighSchoolData.CertificateType = model.CertificateType;
                stHighSchoolData.Average = model.Average.Value;
                stHighSchoolData.CertificateYear = model.CertificateYear.Value;
                stHighSchoolData.SeatNo = model.SeatNo.Value;
                stHighSchoolData.HighSchoolName = model.HighSchoolName;
                stHighSchoolData.Source = model.Source;
                stHighSchoolData.Note = model.Note;

                await _stHighSchoolDataService.UpdateAsync(stHighSchoolData);

                return PartialView("_Edit", model);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return PartialView("_Edit", model);
            }
        }
       

        // GET: StHighSchoolData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var stHighSchoolData = await _stHighSchoolDataService.GetByIdAsync(id.Value);

            if (stHighSchoolData == null) return NotFound();

            return PartialView("_Delete", stHighSchoolData);
        }

       

        // POST: StHighSchoolData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _stHighSchoolDataService.DeleteAsync(id);
                return PartialView("_Delete");
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var stHighSchoolData = await _stHighSchoolDataService.GetByIdAsync(id);
                return PartialView("_Delete", stHighSchoolData);
            }
        }

       
    }
}
