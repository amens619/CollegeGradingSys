using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.District;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class DistrictController : Controller
    {
        private readonly IGenericService<District> _districtService;
        private readonly IGenericService<Governorate> _governorateService;

        public DistrictController(
            IGenericService<District> districtService,
            IGenericService<Governorate> governorateService)
        {
            _districtService = districtService;
            _governorateService = governorateService;
        }

        // ================= Index =================
        public async Task<IActionResult> Index()
        {
            var districts = await _districtService.GetAllAsync();

            var model = districts.Select(d => new DistrictListDetailsDeleteVM
            {
                Id = d.Id,
                DistrictName = d.DistrictName,
                GovernorateName = d.Governorate.GovernorateName
            }).ToList();

            return View(model);
           
        }

        // ================= Details =================
        public async Task<IActionResult> Details(int id)
        {
            var district = await _districtService.GetByIdAsync(id);
            if (district == null) return NotFound();

            return View(district);
        }

        // ================= Create =================
        [Authorize(Policy = "CreateUserPolicy")]
        public async Task<IActionResult> Create()
        {
             var model = new DistrictCreateEditVM
             {
             Governorates = await _governorateService.GetSelectItemsAsync(
            g => new SelectItemVM
            {
                Id = g.Id,
                Name = g.GovernorateName
            })
            };

            return View(model);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistrictCreateEditVM model)
        {
            if (model.GovernorateId == -1)
            {
                ModelState.AddModelError("", "الرجاء اختيار المحافظة");
                return await Create();
            }
            if (!ModelState.IsValid)
            {
                model.Governorates = await _governorateService.GetSelectItemsAsync(
                    g => new SelectItemVM
                    {
                        Id = g.Id,
                        Name = g.GovernorateName
                    });

                return View(model);
            }

            var governorate = await _governorateService.GetByIdAsync(model.GovernorateId);
            if (governorate == null) return NotFound();

            var district = new District
            {
                DistrictName = model.DistrictName,
                Governorate = governorate
            };

            await _districtService.CreateAsync(district);
            return RedirectToAction(nameof(Index));
        }

        // ================= Edit =================
        public async Task<IActionResult> Edit(int id)
        {
            var district = await _districtService.GetByIdAsync(id);
            if (district == null) return NotFound();

            var model = new DistrictCreateEditVM
            {
                Id = district.Id,
                DistrictName = district.DistrictName,
                GovernorateId = district.Governorate?.Id ?? 0,
                Governorates = await _governorateService.GetSelectItemsAsync(
                    g => new SelectItemVM
                    {
                        Id = g.Id,
                        Name = g.GovernorateName
                    })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DistrictCreateEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Governorates = await _governorateService.GetSelectItemsAsync(
                    g => new SelectItemVM
                    {
                        Id = g.Id,
                        Name = g.GovernorateName
                    });

                return View(model);
            }
            var governorate = await _governorateService.GetByIdAsync(model.GovernorateId);
            if (governorate == null) return NotFound();

           
            var district = new District
            {
                Id = model.Id,
                DistrictName = model.DistrictName,
                Governorate = governorate
            };

            await _districtService.UpdateAsync(district);
            return RedirectToAction(nameof(Index));
        }

        // ================= Delete =================
        public async Task<IActionResult> Delete(int id)
        {
            var district = await _districtService.GetByIdAsync(id);
            if (district == null) return NotFound();

            var model = new DistrictListDetailsDeleteVM
            {
                Id = district.Id,
                DistrictName = district.DistrictName,
                GovernorateName = district.Governorate.GovernorateName
            };

            
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _districtService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
