using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels.Batch;
using CollegeGradingSys.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult> Index(Term? term, Level? level, int? SpecializationId, bool clear = false)
        {
            // 1. هل طلب المستخدم إلغاء التصفية؟
            if (clear)
            {
                HttpContext.Session.Remove("FilterTerm");
                HttpContext.Session.Remove("FilterLevel");
                HttpContext.Session.Remove("FilterSpecId");

                return RedirectToAction(nameof(Index)); // إعادة تحميل الصفحة بدون فلاتر
            }
            // 2. هل قام المستخدم بالضغط على زر "تصفية" الآن؟
            // نعرف ذلك إذا كان الرابط (URL) يحتوي على أسماء الحقول
            bool hasNewFilters = Request.Query.ContainsKey("Term") ||
                                 Request.Query.ContainsKey("Level") ||
                                 Request.Query.ContainsKey("SpecializationId");

            if (hasNewFilters)
            {
                // المستخدم أرسل فلاتر جديدة -> نحفظها في الجلسة (Session)
                if (term.HasValue) HttpContext.Session.SetInt32("FilterTerm", (int)term.Value);
                else HttpContext.Session.Remove("FilterTerm");

                if (level.HasValue) HttpContext.Session.SetInt32("FilterLevel", (int)level.Value);
                else HttpContext.Session.Remove("FilterLevel");

                if (SpecializationId.HasValue) HttpContext.Session.SetInt32("FilterSpecId", SpecializationId.Value);
                else HttpContext.Session.Remove("FilterSpecId");
            }
            else
            {
                // المستخدم دخل الصفحة بشكل عادي -> نحاول جلب الفلاتر السابقة من الجلسة
                var sessionTerm = HttpContext.Session.GetInt32("FilterTerm");
                if (sessionTerm.HasValue) term = (Term)sessionTerm.Value;

                var sessionLevel = HttpContext.Session.GetInt32("FilterLevel");
                if (sessionLevel.HasValue) level = (Level)sessionLevel.Value;

                SpecializationId = HttpContext.Session.GetInt32("FilterSpecId");
            }

            // 3. جلب البيانات بناءً على الفلاتر (سواء كانت جديدة أو قادمة من الجلسة)
            var vm = await _courseService.GetIndexViewModelAsync(term, level, SpecializationId);

            // تأكد من تمرير القيم للـ ViewModel حتى تظل القوائم المنسدلة محتفظة بالخيارات المختارة
            var specializations = await _courseService.GetSpecializationsAsync();
            vm.SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", SpecializationId);
           
            return View(vm);
            //var vm = await _courseService.GetIndexViewModelAsync(term, level, SpecializationId);
            //var specializations = await _courseService.GetSpecializationsAsync();
            //ViewData["Specializations"] = new SelectList(specializations, "Id", "SpecializationName");
          
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
            var model =  _courseService.GetCreateViewModelAsync();
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
        //[Authorize(Policy = " DeleteCoursePolicy")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var course = await _courseService.GetByIdAsync(id);
        //    if (course is null)
        //    {
        //        return NotFound();
        //    }


        //    return PartialView("_Delete", course);
        //}
        [HttpGet]
        [Authorize(Policy = "DeleteCoursePolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            // إرجاع الشاشة الكاملة
            return View(course);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteCoursePolicy")]
        public async Task<ActionResult> Delete(int id, Course course)
        {
            try
            {
                var (canDelete, errorMessage) = await _courseService.CanDeleteCourseAsync(id);
                
                if (!canDelete)
                {
                    ModelState.AddModelError(nameof(course.CourseName), "");
                    ViewBag.Message = errorMessage;
                    return View("Delete", course);
                }

                await _courseService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Delete", course);
            }
        }

        async Task FullAllListsAsync()
        {   
            ViewData["ParentsCourses"] = new SelectList(await _courseService.GetParentCoursesAsync(), "Id", "CourseName");
            ViewData["Specializations"] = new SelectList(await _courseService.GetSpecializationsAsync(), "Id", "SpecializationName");
        }
    }
}
