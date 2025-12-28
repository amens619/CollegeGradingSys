using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Interfaces;
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
            var vm = await _courseService.GetIndexViewModelAsync(term, level, SpecializationId);
            var specializations = await _courseService.GetSpecializationsAsync();
            ViewData["Specializations"] = new SelectList(specializations, "Id", "SpecializationName");
            return View(vm);
        }

        // GET: CourseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var vm = await _courseService.GetDetailsViewModelAsync(id);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // GET: CourseController/Create
        //[Authorize(Policy = " CreateCoursePolicy")]
        public async Task<ActionResult> Create()
        {
            var model = await _courseService.GetCreateViewModelAsync();
            await FullAllListsAsync();
            return PartialView("_Create", model);
        }

       

       


        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = " CreateCoursePolicy")]
        public async Task<ActionResult> Create(CreateCourseViewModel model)
        {
            ModelState.ClearValidationState(nameof(model));

            if (!TryValidateModel(model, nameof(model)))
            {
                await FullAllListsAsync();
                return PartialView("_Create", model);
            }

            var (success, errorMessage) = await _courseService.CreateCourseAsync(model);
            
            if (!success)
            {
                if (errorMessage.Contains("الدرجة الكبرى"))
                {
                    ModelState.AddModelError(nameof(model.BigGrade), errorMessage);
                }
                else if (errorMessage.Contains("الدرجة الصغرى"))
                {
                    ModelState.AddModelError(nameof(model.SmallGrade), errorMessage);
                }
                else if (errorMessage.Contains("اسم المادة"))
                {
                    ModelState.AddModelError(nameof(model.CourseName), errorMessage);
                }
                else if (errorMessage.Contains("المادة الاساسية"))
                {
                    ModelState.AddModelError(nameof(model.ParentId), errorMessage);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                await FullAllListsAsync();
                return PartialView("_Create", model);
            }

            await FullAllListsAsync();
            return PartialView("_Create", model);
        }

        // GET: CourseController/Edit/5
        //[Authorize(Policy = " EditCoursePolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _courseService.GetEditViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            await FullAllListsAsync();
            return PartialView("_Edit", model);
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = " EditCoursePolicy")]
        public async Task<ActionResult> Edit(int id, EditCourseViewModel model)
        {
            ModelState.ClearValidationState(nameof(model));

            if (!TryValidateModel(model, nameof(model)))
            {
                await FullAllListsAsync();
                return PartialView("_Edit", model);
            }

            var (success, errorMessage) = await _courseService.UpdateCourseAsync(model);
            
            if (!success)
            {
                if (errorMessage.Contains("الدرجة الكبرى"))
                {
                    ModelState.AddModelError(nameof(model.BigGrade), errorMessage);
                }
                else if (errorMessage.Contains("الدرجة الصغرى"))
                {
                    ModelState.AddModelError(nameof(model.SmallGrade), errorMessage);
                }
                else if (errorMessage.Contains("اسم المادة"))
                {
                    ModelState.AddModelError(nameof(model.CourseName), errorMessage);
                }
                else if (errorMessage.Contains("المادة الاساسية"))
                {
                    ModelState.AddModelError(nameof(model.ParentId), errorMessage);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }

                await FullAllListsAsync();
                return PartialView("_Edit", model);
            }

            await FullAllListsAsync();
            return PartialView("_Edit", model);
        }

        // GET: CourseController/Delete/5
        [Authorize(Policy = " DeleteCoursePolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var course = await _courseService.GetByIdAsync(id);
            if (course is null)
            {
                return NotFound();
            }
           

            return PartialView("_Delete", course);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = " DeleteCoursePolicy")]
        public async Task<ActionResult> Delete(int id, Course course)
        {
            try
            {
                var (canDelete, errorMessage) = await _courseService.CanDeleteCourseAsync(id);
                
                if (!canDelete)
                {
                    ModelState.AddModelError(nameof(course.CourseName), "");
                    ViewBag.Message = errorMessage;
                    return PartialView("_Delete", course);
                }

                await _courseService.DeleteAsync(id);
                return PartialView("_Delete", course);
            }
            catch
            {
                return PartialView("_Delete", course);
            }
        }

        async Task FullAllListsAsync()
        {   
            ViewData["ParentsCourses"] = new SelectList(await _courseService.GetParentCoursesAsync(), "Id", "CourseName");
            ViewData["Specializations"] = new SelectList(await _courseService.GetSpecializationsAsync(), "Id", "SpecializationName");
        }
    }
}
