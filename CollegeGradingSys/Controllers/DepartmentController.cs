using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }

        // GET: DepartmentController/Details/5
        //public ActionResult Details(int id)
        //{
        //    var department = DepartmentRepository.Find(id);
        //    return View(department);
        //}

        // GET: DepartmentController/Create
        [Authorize(Policy = "CreateDepartmentPolicy")]
        public async Task<ActionResult> Create()
        {
            var model = await _departmentService.GetCreateViewModelAsync();
            return View(model);
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateDepartmentPolicy")]
        public async Task<ActionResult> Create(CollegeDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Colleges = await _departmentService.GetCollegesAsync();
                return View(model);
            }

            var (success, errorMessage) = await _departmentService.CreateDepartmentAsync(model);
            
            if (!success)
            {
                if (errorMessage.Contains("اسم القسم"))
                {
                    ModelState.AddModelError(nameof(model.DepartmentName), errorMessage);
                }
                else if (errorMessage.Contains("الكلية"))
                {
                    ViewBag.Message = errorMessage;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                model.Colleges = await _departmentService.GetCollegesAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentController/Edit/5
        [Authorize(Policy = "EditDepartmentPolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = await _departmentService.GetEditViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDepartmentPolicy")]
        public async Task<ActionResult> Edit(int id, CollegeDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Colleges = await _departmentService.GetCollegesAsync();
                return View(model);
            }

            var (success, errorMessage) = await _departmentService.UpdateDepartmentAsync(id, model);
            
            if (!success)
            {
                if (errorMessage.Contains("اسم القسم"))
                {
                    ModelState.AddModelError(nameof(model.DepartmentName), errorMessage);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                model.Colleges = await _departmentService.GetCollegesAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentController/Delete/5
        [Authorize(Policy = "DeleteDepartmentPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            
            return View(department);       
        }

        // POST: DepartmentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteDepartmentPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (canDelete, errorMessage) = await _departmentService.CanDeleteDepartmentAsync(id);
                
                if (!canDelete)
                {
                    var department = await _departmentService.GetByIdAsync(id);
                    ViewBag.Message = errorMessage;
                    return View(department);
                }

                await _departmentService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var department = await _departmentService.GetByIdAsync(id);
                return View(department);
            }
        }
    }
}
