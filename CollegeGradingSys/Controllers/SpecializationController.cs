using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class SpecializationController : Controller
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }
        // GET: SpecializationController
        public async Task<ActionResult> Index()
        {
            var specializations = await _specializationService.GetAllAsync();
            return View(specializations);
        }

        // GET: SpecializationController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var specialization = await _specializationService.GetByIdAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }
            return View(specialization);
        }

        // GET: SpecializationController/Create
        [Authorize(Policy = "CreateSpecializationPolicy")]
        public async Task<ActionResult> Create()
        {
            var model = await _specializationService.GetCreateViewModelAsync();
            return View(model);
        }

        // POST: SpecializationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateSpecializationPolicy")]
        public async Task<ActionResult> Create(DepartmentSpecializationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await _specializationService.GetDepartmentsAsync();
                return View(model);
            }

            var (success, errorMessage) = await _specializationService.CreateSpecializationAsync(model);
            
            if (!success)
            {
                if (errorMessage.Contains("اسم التخصص"))
                {
                    ModelState.AddModelError(nameof(model.SpecializationName), errorMessage);
                }
                else if (errorMessage.Contains("القسم"))
                {
                    ViewBag.Message = errorMessage;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                model.Departments = await _specializationService.GetDepartmentsAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SpecializationController/Edit/5
        [Authorize(Policy = "EditSpecializationPolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = await _specializationService.GetEditViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: SpecializationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditSpecializationPolicy")]
        public async Task<ActionResult> Edit(int id, DepartmentSpecializationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await _specializationService.GetDepartmentsAsync();
                return View(model);
            }

            var (success, errorMessage) = await _specializationService.UpdateSpecializationAsync(id, model);
            
            if (!success)
            {
                if (errorMessage.Contains("اسم التخصص"))
                {
                    ModelState.AddModelError(nameof(model.SpecializationName), errorMessage);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                model.Departments = await _specializationService.GetDepartmentsAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SpecializationController/Delete/5
        [Authorize(Policy = "DeleteSpecializationPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            var specialization = await _specializationService.GetByIdAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }
            
            return View(specialization);       
        }

        // POST: SpecializationController/Delete/5       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteSpecializationPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (canDelete, errorMessage) = await _specializationService.CanDeleteSpecializationAsync(id);
                
                if (!canDelete)
                {
                    var specialization = await _specializationService.GetByIdAsync(id);
                    ViewBag.Message = errorMessage;
                    return View(specialization);
                }

                await _specializationService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var specialization = await _specializationService.GetByIdAsync(id);
                return View(specialization);
            }
        }
    }
}
