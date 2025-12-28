using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: CourseController
        public async Task<ActionResult> Index(Term? term, Level? level, int? SpecializationId)
        {
            var courses = await _courseService.GetFilteredCoursesAsync(term, level, SpecializationId);
            var vm = new CourseIndexViewModel
            {
                Term = term,
                Level = level,
                SpecializationId = SpecializationId,
                Courses = courses
            };
            
            var specializations = await _courseService.GetSpecializationsSelectItemsAsync();
            ViewData["Specializations"] = new SelectList(specializations, "Id", "Name");
            
            return View(vm);
        }

        // GET: CourseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var vm = await _courseService.GetCourseDetailsAsync(id);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // GET: CourseController/Create
        //[Authorize(Policy = " CreateCoursePolicy")]
        public async Task<ActionResult> Create()
        {
            var model = await _courseService.PrepareCreateCourseViewModelAsync();
            await FillViewDataAsync();
            return PartialView("_Create", model);
        }

        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = " CreateCoursePolicy")]
        public async Task<ActionResult> Create(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillViewDataAsync();
                return PartialView("_Create", model);
            }

            try
            {
                await _courseService.CreateCourseAsync(model);
                return PartialView("_Create", model);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await FillViewDataAsync();
                return PartialView("_Create", model);
            }
        }

        // GET: CourseController/Edit/5
        //[Authorize(Policy = " EditCoursePolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
                return NotFound();

            var model = await _courseService.PrepareEditCourseViewModelAsync(id);
            if (model == null)
                return NotFound();

            await FillViewDataAsync();
            return PartialView("_Edit", model);
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = " EditCoursePolicy")]
        public async Task<ActionResult> Edit(int id, EditCourseViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await FillViewDataAsync();
                return PartialView("_Edit", model);
            }

            try
            {
                await _courseService.UpdateCourseAsync(model);
                return PartialView("_Edit", model);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await FillViewDataAsync();
                return PartialView("_Edit", model);
            }
        }

        // GET: CourseController/Delete/5
        [Authorize(Policy = " DeleteCoursePolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return NotFound();

            var vm = await _courseService.PrepareDeleteCourseViewModelAsync(id);
            if (vm == null)
                return NotFound();

            return PartialView("_Delete", vm);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = " DeleteCoursePolicy")]
        public async Task<ActionResult> Delete(int id, CourseDeleteViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            try
            {
                await _courseService.DeleteCourseAsync(id);
                return PartialView("_Delete", vm);
            }
            catch (DomainException ex)
            {
                vm.ErrorMessage = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return PartialView("_Delete", vm);
            }
        }

        private async Task FillViewDataAsync()
        {
            var parentCourses = await _courseService.GetParentCoursesSelectItemsAsync();
            var specializations = await _courseService.GetSpecializationsSelectItemsAsync();
            
            ViewData["ParentsCourses"] = new SelectList(parentCourses, "Id", "Name");
            ViewData["Specializations"] = new SelectList(specializations, "Id", "Name");
        }
    }
}
