using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
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
    public class NationalityController : Controller
    {
        private readonly INationalityService _nationalityService;

        public NationalityController(INationalityService nationalityService)
        {
            _nationalityService = nationalityService;
        }
        // GET: NationalityController
        public async Task<ActionResult> Index()
        {
            // 1. جلب البيانات الأساسية من السيرفس
            var nationalities = await _nationalityService.GetAllAsync();

            // 2. تحويل الكيانات إلى ViewModels
            var vm = nationalities.Select(n => new NationalityIndexVM
            {
                Id = n.Id,
                // افترضت أن اسم الخاصية هو NationalityName، قم بتعديله إذا كان مختلفاً (مثل Name)
                NationalityName = n.NationalityName,
                CountryName = n.CountryName
            }).ToList();

            // 3. إرسال قائمة الـ ViewModels إلى الشاشة
            return View(vm);
        }



        // GET: NationalityController/Create
        [Authorize(Policy = "CreateNationalityPolicy")]
        public ActionResult Create()
        {

            return View(new NationalityVM());
        }

        // POST: NationalityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateNationalityPolicy")]       
        public async Task<IActionResult> Create(NationalityVM vm)
        {
            // 1. التحقق من صحة المدخلات
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                // 2. تحويل الـ VM إلى Entity للحفظ
                var nationality = new Models.Nationality
                {
                    CountryName = vm.CountryName.Trim(),
                    NationalityName = vm.NationalityName.Trim()
                };

                await _nationalityService.CreateAsync(nationality);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: NationalityController/Edit/5
        [Authorize(Policy = "EditNationalityPolicy")]
        public async Task<ActionResult> Edit(int id) 
        {
            if (id <= 0)
                return NotFound();

            
            var nationality = await _nationalityService.GetByIdAsync(id);
            if (nationality == null)
                return NotFound();

           
            var vm = new NationalityVM
            {
                Id = nationality.Id,
                CountryName = nationality.CountryName,
                NationalityName = nationality.NationalityName
            };

            return View(vm);
        }

        // POST: NationalityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditNationalityPolicy")]
        public async Task<IActionResult> Edit(NationalityVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                // جلب السجل القديم من قاعدة البيانات
                var nationality = await _nationalityService.GetByIdAsync(vm.Id);
                if (nationality == null)
                    return NotFound();

                // تحديث البيانات
                nationality.CountryName = vm.CountryName.Trim();
                nationality.NationalityName = vm.NationalityName.Trim();

                await _nationalityService.UpdateAsync(nationality);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: NationalityController/Delete/5
        [Authorize(Policy = "DeleteNationalityPolicy")]
        public async Task<ActionResult> Delete(int id) 
        {
            if (id <= 0) return NotFound();

          
            var nationality = await _nationalityService.GetByIdAsync(id);
            if (nationality == null) return NotFound(); // التحقق من القيمة الفارغة

            var vm = new NationalityVM
            {
                Id = nationality.Id,
                CountryName = nationality.CountryName,
                NationalityName = nationality.NationalityName
            };

            return View(vm);
        }

        // POST: NationalityController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteNationalityPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _nationalityService.DeleteWithValidationAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ViewBag.Message = ex.Message;

                // عند حدوث خطأ أثناء الحذف، نعيد تحميل البيانات للشاشة عبر الـ VM
                var nationality = await _nationalityService.GetByIdAsync(id);
                var vm = new NationalityVM
                {
                    Id = nationality.Id,
                    CountryName = nationality.CountryName,
                    NationalityName = nationality.NationalityName
                };

                return View(vm);
            }
        }



    }
}
