using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Implementations;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
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
    public class CollegeController : Controller
    {
        //private readonly GenericService<College> _collegeService;
        private readonly ICollegeService _collegeService;
      

        public CollegeController(ICollegeService collegeService)
        {
            _collegeService = collegeService;       
        }
        // GET: CollegeController
        public async Task<ActionResult> Index()
        {            
            var colleges =await _collegeService.GetAllAsync();

            var vm = colleges.Select(c => new CollegeIndexVM
            {
                Id = c.Id,
                CollegeName = c.CollegeName
            }).ToList();
            return View(vm);
           
        }

       

        // GET: CollegeController/Create
        [Authorize(Policy = "CreateCollegePolicy")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: CollegeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateCollegePolicy")]
        public async Task<IActionResult> Create(CollegeCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _collegeService.CreateAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(nameof(vm.CollegeName), ex.Message);
                return View(vm);
            }
        }

        // GET: CollegeController/Edit/5
        [Authorize(Policy = "EditCollegePolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
                return NotFound();

            try
            {
                var college = await _collegeService.GetByIdAsync(id);
                if (college == null) return NotFound();

                var vm = new CollegeVM
                {
                    Id = college.Id,
                    CollegeName = college.CollegeName
                };
                return View(vm);
            }
            catch (DomainException)
            {
                return NotFound();
            }
        }

        // POST: CollegeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditCollegePolicy")]
        public async Task<IActionResult> Edit(CollegeVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _collegeService.UpdateAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(nameof(vm.CollegeName), ex.Message);
                return View(vm);
            }
        }

        // GET: CollegeController/Delete/5
        [Authorize(Policy = "DeleteCollegePolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return NotFound();

            try
            {
                var college = await _collegeService.GetByIdAsync(id);
                if (college == null) return NotFound();

                var vm = new CollegeVM
                {
                    Id = college.Id,
                    CollegeName = college.CollegeName
                };
                return View(vm);
            }
            catch (DomainException)
            {
                return NotFound();
            }
        }

        // POST: CollegeController/Delete/5
        // POST: StAcademicData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteCollegePolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {        
            try
            {
                await _collegeService.DeleteAsync(id);               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      
    }
}
