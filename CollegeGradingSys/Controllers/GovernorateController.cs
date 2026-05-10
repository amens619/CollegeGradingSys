using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Governorate;
using CollegeGradingSys.ViewModels.Nationality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class GovernorateController : Controller
    {
        private readonly IGovernorateService _governorateService;
        private readonly IGenericService<Nationality> _nationalityService;

        public GovernorateController(
            IGovernorateService governorateService,
            IGenericService<Nationality> nationalityService)
        {
            _governorateService = governorateService;
            _nationalityService = nationalityService;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            // 1. جلب كل البيانات من قاعدة البيانات
            var all = await _governorateService.GetAllAsync();

            // 2. تحويل البيانات الأساسية إلى ViewModel (مع جلب اسم الدولة)
            var mappedList = all.Select(g => new GovernorateIndexVM
            {
                Id = g.Id,
                GovernorateName = g.GovernorateName,

                // التحقق من أن العلاقة ليست null قبل محاولة قراءة اسم الدولة
                NationalityName = g.Nationality != null ? g.Nationality.CountryName : "غير محدد"
            })
            .OrderBy(x => x.GovernorateName) // الترتيب الأبجدي يتم هنا
            .ToList();

            // 3. تطبيق التقسيم للصفحات (Pagination)
            var items = mappedList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4. إرسال النتيجة للشاشة باستخدام PagedResult ولكن بالنوع الجديد (GovernorateIndexVM)
            return View(new PagedResult<GovernorateIndexVM>
            {
                Data = items,
                TotalItems = mappedList.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }
        // ================= CREATE =================
        public async Task<IActionResult> Create()
        {
            return View(new NationalityGovernorateViewModel
            {
                Nationalities = await _nationalityService.GetSelectItemsAsync(
                    n => new SelectItemVM { Id = n.Id, Name = n.CountryName })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NationalityGovernorateViewModel model)
        {
            try
            {
                var nationality = await _nationalityService.GetByIdAsync(model.NationalityId);

                await _governorateService.CreateWithEnsureGovernorateNameAsync(new Governorate
                {
                    GovernorateName = model.GovernorateName,
                    Nationality = nationality
                });

                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Nationalities = await _nationalityService.GetSelectItemsAsync(
                    n => new SelectItemVM { Id = n.Id, Name = n.CountryName });
                return View(model);
            }
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int id)
        {
            var g = await _governorateService.GetByIdAsync(id);
            if (g == null) return NotFound();

            return View(new NationalityGovernorateViewModel
            {
                Id = g.Id,
                GovernorateName = g.GovernorateName,
                NationalityId = g.Nationality.Id,
                Nationalities = await _nationalityService.GetSelectItemsAsync(
                    n => new SelectItemVM { Id = n.Id, Name = n.CountryName })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NationalityGovernorateViewModel model)
        {
            try
            {
                var nationality = await _nationalityService.GetByIdAsync(model.NationalityId);

                await _governorateService.UpdateEnsureGovernorateNameAsync(new Governorate
                {
                    Id = model.Id,
                    GovernorateName = model.GovernorateName,
                    Nationality = nationality
                });

                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Nationalities = await _nationalityService.GetSelectItemsAsync(
                    n => new SelectItemVM { Id = n.Id, Name = n.CountryName });
                return View(model);
            }
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int id)
        {
            var g = await _governorateService.GetByIdAsync(id);
            if (g == null) return NotFound();
            return View(g);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _governorateService.DeleteWithValidationAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ViewBag.Message = ex.Message;
                return View(await _governorateService.GetByIdAsync(id));
            }
        }
    }
}
