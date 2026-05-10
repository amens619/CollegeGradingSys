using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Specialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class SpecializationController : Controller
    {
        private readonly ISpecializationService _specializationService;
        private readonly IDepartmentService _departmentService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }
        // GET: SpecializationController
        //public async Task<ActionResult> Index()
        //{
        //    var specializations = await _specializationService.GetAllAsync();
        //    return View(specializations);
        //}

        public async Task<ActionResult> Index()
        {
            // 1. جلب البيانات الأساسية (الكيانات) من قاعدة البيانات
            var specializations = await _specializationService.GetAllAsync();

            // 2. تحويل الكيانات إلى ViewModels
            var vm = specializations.Select(s => new SpecializationIndexVM
            {
                Id = s.Id,
                SpecializationName = s.SpecializationName, 
                
                DepartmentName = s.Department != null ? s.Department.DepartmentName : "غير محدد"
            }).ToList();

            // 3. إرسال قائمة الـ ViewModels إلى الشاشة
            return View(vm);
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
        public async Task<ActionResult> Create(SpecializationVM model)
        {
            if (!ModelState.IsValid)
            {
                model.DepartmentsList = await PopulateDepartmentsDropdownAsync();
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

                model.DepartmentsList = await PopulateDepartmentsDropdownAsync();
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
        public async Task<ActionResult> Edit(int id, SpecializationVM model)
        {
            if (!ModelState.IsValid)
            {
                model.DepartmentsList = await PopulateDepartmentsDropdownAsync();
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

                model.DepartmentsList = await PopulateDepartmentsDropdownAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

       
        // GET: SpecializationController/Delete/5
        [Authorize(Policy = "DeleteSpecializationPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            // 1. التحقق من صحة المعرف أولاً كإجراء أمني
            if (id <= 0)
            {
                return NotFound();
            }

            // 2. جلب التخصص من قاعدة البيانات
            var specialization = await _specializationService.GetByIdAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            // 3. تحويل الكيان (Entity) إلى ViewModel
            var vm = new SpecializationVM
            {
                Id = specialization.Id,
                SpecializationName = specialization.SpecializationName,
                DepartmentId = specialization.Department?.Id ?? 0,

                // جلب اسم القسم وعرضه ليتأكد المستخدم من تفاصيل التخصص قبل حذفه
                DepartmentName = specialization.Department?.DepartmentName
            };

            // 4. إرسال الـ ViewModel إلى الشاشة
            return View(vm);
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

        private async Task<SelectList> PopulateDepartmentsDropdownAsync()
        {
            var departmentsItems = await _departmentService.GetSelectItemsAsync(c => new SelectItemVM
            {
                Id = c.Id,
                Name = c.DepartmentName
            }, "-- أختر --");

            return new SelectList(departmentsItems, "Id", "Name");
        }
    }
}
