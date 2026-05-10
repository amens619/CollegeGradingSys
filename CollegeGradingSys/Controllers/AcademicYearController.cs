using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.AcademicYear;
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
