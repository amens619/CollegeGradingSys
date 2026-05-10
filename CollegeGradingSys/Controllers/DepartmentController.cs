using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICollegeService _collegeService;

        public DepartmentController(IDepartmentService departmentService, ICollegeService collegeService)
        {
            _departmentService = departmentService;
            _collegeService = collegeService;
        }
        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {

            // 1. جلب البيانات الأساسية من السيرفس
            var departments = await _departmentService.GetAllAsync();

          
            var vm = departments.Select(d => new DepartmentIndexVM
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName ,
                CollegeName = d.College != null ? d.College.CollegeName : "غير محدد"

            }).ToList();

            
            return View(vm);
            
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
        public async Task<ActionResult> Create(DepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
                
                model.CollegesList = await PopulateCollegesDropdownAsync();
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

              
                model.CollegesList = await PopulateCollegesDropdownAsync();
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
        public async Task<ActionResult> Edit(int id, DepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
              
                model.CollegesList = await PopulateCollegesDropdownAsync();
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
               
                model.CollegesList = await PopulateCollegesDropdownAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentController/Delete/5
        [Authorize(Policy = "DeleteDepartmentPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            // 1. التحقق من صحة المعرف
            if (id <= 0)
            {
                return NotFound();
            }

            // 2. جلب البيانات من قاعدة البيانات
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            // 3. تحويل الكيان (Entity) إلى ViewModel
            var vm = new DepartmentVM
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName, // افترضت أن الخاصية اسمها هكذا
               
                 CollegeName = department.College?.CollegeName
            };

            // 4. إرسال الـ ViewModel إلى الشاشة
            return View(vm);
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

        private async Task<SelectList> PopulateCollegesDropdownAsync()
        {
            var collegesItems = await _collegeService.GetSelectItemsAsync(c => new SelectItemVM
            {
                Id = c.Id,
                Name = c.CollegeName
            }, "-- أختر --");

            return new SelectList(collegesItems, "Id", "Name");
        }
    }
}
