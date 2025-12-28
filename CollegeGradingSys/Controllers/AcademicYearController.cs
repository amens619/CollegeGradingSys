//using CollegeGradingSys.Models;
//using CollegeGradingSys.Models.Enums;
//using CollegeGradingSys.Repositories.Interfaces;
//using CollegeGradingSys.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Linq;

//namespace CollegeGradingSys.Controllers
//{
//    [Authorize(Roles = "Admin,Owner")]
//    public class AcademicYearController : Controller
//    {
//        private readonly IAcademicYearRepository _academicYearRepo;
//        private readonly ICollegeGradingSysRepository<StAcademicData> _stAcademicRepo;
//        private readonly ICollegeGradingSysRepository<StPersonalData> _stPersonalRepo;

//        public AcademicYearController(
//            IAcademicYearRepository academicYearRepo,
//            ICollegeGradingSysRepository<StAcademicData> stAcademicRepo,
//            ICollegeGradingSysRepository<StPersonalData> stPersonalRepo)
//        {
//            _academicYearRepo = academicYearRepo;
//            _stAcademicRepo = stAcademicRepo;
//            _stPersonalRepo = stPersonalRepo;
//        }

//        // GET: AcademicYearController
//        public ActionResult Index()
//        {
//            var model = new AcademicYearVM
//            {
//                AcademicYears = _academicYearRepo.List()
//                                .OrderByDescending(x => x.AcademicYearStart)
//                                .ToList(),
//                IsCurrentYearClosed = IsCurrentYearClosed()
//            };

//            return View(model);
//        }

//        // GET: Details
//        [Authorize(Policy = "DetailsAcademicYearPolicy")]
//        public ActionResult Details(int id)
//        {
//            var year = _academicYearRepo.Find(id);
//            return year == null ? NotFound() : View(year);
//        }

//        // GET: Create
//        [Authorize(Policy = "CreateAcademicYearPolicy")]
//        public ActionResult Create()
//        {
//            return !IsCurrentYearClosed() ? NotFound() : View();
//        }

//        // POST: Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Policy = "CreateAcademicYearPolicy")]
//        public ActionResult Create(AcademicYear academicYear)
//        {
//            if (!IsCurrentYearClosed())
//                return NotFound();

//            if (!ModelState.IsValid)
//                return View(academicYear);

//            if (string.IsNullOrWhiteSpace(academicYear.AcademicYearName))
//            {
//                ModelState.AddModelError(nameof(academicYear.AcademicYearName), "الرجاء كتابة العام الاكاديمي");
//                return View(academicYear);
//            }

//            if (isAcademicYearNameExists(academicYear.AcademicYearName.Trim()))
//            {
//                ModelState.AddModelError(nameof(academicYear.AcademicYearName), "يوجد عام سابق بنفس الاسم، الرجاء كتابة اسم آخر");
//                return View(academicYear);
//            }

//            if (academicYear.AcademicYearStart >= academicYear.AcademicYearEnd)
//            {
//                ModelState.AddModelError(nameof(academicYear.AcademicYearEnd), "يجب أن يكون تاريخ النهاية بعد البداية");
//                return View(academicYear);
//            }

//            try
//            {
//                // إلغاء التعيين السابق للسنة الحالية
//                var previousYears = _academicYearRepo.List().Where(x => x.IsCurrentYear);
//                foreach (var p in previousYears)
//                {
//                    p.IsCurrentYear = false;
//                    _academicYearRepo.Update(p.Id, p);
//                }

//                academicYear.IsCurrentYear = true;
//                _academicYearRepo.Add(academicYear);

//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View(academicYear);
//            }
//        }

//        // GET: Edit
//        [Authorize(Policy = "EditAcademicYearPolicy")]
//        public ActionResult Edit(int id)
//        {
//            if (!IsCurrentYearClosed())
//                return NotFound();

//            if (id <= 0)
//                return NotFound();

//            var year = _academicYearRepo.Find(id);
//            return year == null ? NotFound() : View(year);
//        }

//        // POST: Edit
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Policy = "EditAcademicYearPolicy")]
//        public ActionResult Edit(int id, AcademicYear academicYear)
//        {
//            if (!IsCurrentYearClosed())
//                return NotFound();

//            if (academicYear.AcademicYearStart >= academicYear.AcademicYearEnd)
//            {
//                ModelState.AddModelError(nameof(academicYear.AcademicYearEnd), "يجب أن يكون تاريخ النهاية بعد البداية");
//                return View(academicYear);
//            }

//            try
//            {
//                _academicYearRepo.Update(id, academicYear);
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View(academicYear);
//            }
//        }

//        // GET: Delete
//        [Authorize(Policy = "DeleteAcademicYearPolicy")]
//        public ActionResult Delete(int id)
//        {
//            if (id <= 0)
//                return NotFound();

//            var year = _academicYearRepo.Find(id);
//            return year == null ? NotFound() : View(year);
//        }

//        // POST: Delete
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Policy = "DeleteAcademicYearPolicy")]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            try
//            {
//                // التحقق من وجود طلاب سجلوا هذه السنة
//                var hasPersonal = _stPersonalRepo.List().Any(x => x.EnrollmentYear.Id == id);
//                if (hasPersonal)
//                {
//                    ViewBag.Message = "لا يمكن حذف العام الجامعي بسبب تسجيل بعض الطلاب فيه.";
//                    return View(_academicYearRepo.Find(id));
//                }

//                var hasAcademic = _stAcademicRepo.List().Any(x => x.AcademicYear.Id == id);
//                if (hasAcademic)
//                {
//                    ViewBag.Message = "لا يمكن الحذف بسبب وجود سجلات أكاديمية مرتبطة.";
//                    return View(_academicYearRepo.Find(id));
//                }

//                _academicYearRepo.Delete(id);

//                // إعادة تعيين آخر سنة كسنة حالية
//                var lastYear = _academicYearRepo.List().LastOrDefault();
//                if (lastYear != null)
//                {
//                    lastYear.IsCurrentYear = true;
//                    _academicYearRepo.Update(lastYear.Id, lastYear);
//                }

//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        private bool IsCurrentYearClosed()
//        {
//            var currentYear = _academicYearRepo.List().SingleOrDefault(x => x.IsCurrentYear);
//            if (currentYear == null)
//                return true;

//            var activeStudents = _stAcademicRepo.List()
//                .Where(x => x.AcademicYear.Id == currentYear.Id && x.StStatus == StStatus.مقيد)
//                .Any();

//            return !activeStudents;
//        }

//        private bool isAcademicYearNameExists(string name)
//        {
//            return _academicYearRepo.List().Any(e => e.AcademicYearName == name);
//        }
//    }
//}
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin,Owner")]
public class AcademicYearController : Controller
{
    private readonly IAcademicYearService _yearService;
  
    public AcademicYearController(IAcademicYearService academicYearService)
    {
        _yearService = academicYearService;       
    }

    // GET: Index
    public async Task<IActionResult> Index()
    {
        var vm = await _yearService.GetIndexViewAsync();    

        return View(vm);
    }

    // GET: Details
    [Authorize(Policy = "DetailsAcademicYearPolicy")]
    public async Task<IActionResult> Details(int id)
    {
        var year = await _yearService.GetDetailsAsync(id);
        if (year == null) return NotFound();
        return View(year);
    }

    // GET: Create
    [Authorize(Policy = "CreateAcademicYearPolicy")]
    public async Task<IActionResult> Create()
    {
        if (!await _yearService.IsCurrentYearClosedAsync()) return NotFound();
        return View();
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "CreateAcademicYearPolicy")]
    public async Task<IActionResult> Create(AcademicYearEditVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var entity = new AcademicYear
        {
            AcademicYearName = vm.AcademicYearName.Trim(),
            AcademicYearNameH = vm.AcademicYearNameH?.Trim() ?? string.Empty,
            AcademicYearStart = vm.AcademicYearStart,
            AcademicYearEnd = vm.AcademicYearEnd,
            IsCurrentYear = vm.IsCurrentYear,           
            Note = vm.Note
        };

        try
        {
            await _yearService.CreateAsync(entity);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
    }

    // GET: Edit
    [Authorize(Policy = "EditAcademicYearPolicy")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var vm = await _yearService.GetEditViewAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "EditAcademicYearPolicy")]
    public async Task<IActionResult> Edit(int id, AcademicYearEditVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            await _yearService.UpdateAsync(id, vm);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
       
    }

    // GET: Delete
    [Authorize(Policy = "DeleteAcademicYearPolicy")]
    public async Task<IActionResult> Delete(int id)
    {

        try
        {
            var vm = await _yearService.GetDeleteViewAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }
        catch (DomainException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }

      
    }

    // POST: DeleteConfirmed
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "DeleteAcademicYearPolicy")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _yearService.DeleteAsync(id);
            TempData["Success"] = "تم حذف العام الدراسي.";
        }
        catch (DomainException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));

       
    }
}
