using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICollegeGradingSysRepository<Course> CourseRepository;
        private readonly ICollegeGradingSysRepository<Specialization> _specializationRepository;
        private readonly ICollegeGradingSysRepository<CourseGrade> _CourseGradeRepository;

        public CourseController(ICollegeGradingSysRepository<Course> CourseRepository,
            ICollegeGradingSysRepository<Specialization> specializationRepository,
            ICollegeGradingSysRepository<CourseGrade> CourseGradeRepository)
        {
            this.CourseRepository = CourseRepository;
            this._specializationRepository = specializationRepository;
            _CourseGradeRepository = CourseGradeRepository;
        }
        // GET: CourseController
        public ActionResult Index(Term? term, Level? level,int? SpecializationId)
        {
            var Courses = CourseRepository.List()
                    .OrderBy(x => x.Level)
                    .ThenBy(x => x.Term)
                    .ToList();
            var model = new CourseIndexViewModel();
            if (SpecializationId != null && SpecializationId != -1)
            {
                model.SpecializationId = SpecializationId;
                Courses = Courses.Where(x => x.Specialization.Id == SpecializationId).ToList();
            }
            if (level != null)
            {
                model.Level = level;
                Courses = Courses.Where(x => x.Level == level).ToList();              
            }
            if (term != null)
            {
                model.Term = term;
                Courses = Courses.Where(x => x.Term == term).ToList();              
            }
            model.Courses = Courses;
            ViewData["Specializations"] = new SelectList(FillSelectSpecializationsList(), "Id", "SpecializationName");
            return View(model);
        }

        // GET: CourseController/Details/5
        public ActionResult Details(int id)
        {
            var course = CourseRepository.Find(id);
            return View(course);
        }

        // GET: CourseController/Create
        public ActionResult Create()
        {

            var model = new CreateCourseViewModel();

            model.BigGrade = "100";
            model.SmallGrade = "60";
            FullAllLists();
            return PartialView("_Create", model);
        }

       

       


        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCourseViewModel model)
        {
            ModelState.ClearValidationState(nameof(model));
            int newBigGrade = 0;
            if (model.BigGrade is not null)
            {
                var cultureInfo = new CultureInfo("en");
                if (!(int.TryParse(model.BigGrade.ToString(),
                    NumberStyles.Integer,
                    cultureInfo, out var modelBigGrade)) || !(modelBigGrade >= 0 && modelBigGrade <= 100))
                {
                    ModelState.AddModelError(nameof(model.BigGrade), " الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");

                    //return PartialView("_Create", model);
                }else { newBigGrade = modelBigGrade; }
                

            }
            else
            {
                ModelState.AddModelError(nameof(model.BigGrade), "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");

                //return PartialView("_Create", model);

            }

            int newSmallGrade = 0;
            if (model.SmallGrade is not null)
            {
                var cultureInfo = new CultureInfo("en");
                if (!(int.TryParse(model.SmallGrade.ToString(),
                    NumberStyles.Integer,
                    cultureInfo, out var modelSmallGrade)) || !(modelSmallGrade >= 0 && modelSmallGrade <= 100))
                {
                    ModelState.AddModelError(nameof(model.SmallGrade), " الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");

                    //return PartialView("_Create", model);
                }
                else { newSmallGrade = modelSmallGrade; }
                

            }
            else
            {
                ModelState.AddModelError(nameof(model.SmallGrade), "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");

                return PartialView("_Create", model);

            }



            if (string.IsNullOrEmpty(model.CourseName))
            {
                //FullAllLists();
                ModelState.AddModelError(nameof(model.CourseName), " الرجاء إدخال اسم المادة بطول  40 حرفًا على الاكثر.");

                //return PartialView("_Create", model);
            }
            else if (isCourseNameExists((model.CourseName).Trim()))
            {
                ModelState.AddModelError(nameof(model.CourseName), "لقد تم إيجاد مادة بنفس اسم .. الرجاء كتابة اسم آخر ");
                //return PartialView("_Create", model);
            }
            if (model.IsSubCourse)
            {
                if (model.ParentId == -1 || model.ParentId == null)
                {
                    //FullAllLists();
                    ModelState.AddModelError(nameof(model.ParentId), "الرجاء اختيار المادة الاساسية من القائمة");

                    //return PartialView("_Create", model);
                }
            }

            try
            {
               
                  

                if (!TryValidateModel(model, nameof(model)))
                {
                    FullAllLists();
                    return PartialView("_Create", model);
                }
                var specialization = _specializationRepository.Find(model.SpecializationId);
                    Course course = new()
                    {
                        Id = model.Id,
                        CourseName = model.CourseName,
                        BigGrade = newBigGrade,
                        SmallGrade = newSmallGrade,
                        IsSubCourse = model.IsSubCourse,
                        Level = model.Level,
                        ParentId = model.ParentId,
                        Term = model.Term,
                        Note = model.Note,
                         Specialization = specialization

                    };
                    CourseRepository.Add(course);
                    return PartialView("_Create", model);
                    //return RedirectToAction(nameof(Index));
                }
                catch
                {
                   
                FullAllLists();
                return PartialView("_Create", model);
                }

           
           

        }

        // GET: CourseController/Edit/5
        public ActionResult Edit(int id)
        {
            var course = CourseRepository.Find(id);
            FullAllLists();
            //var governorateId = city.District == null ? city.District.Id = 0 : city.District.Id;
            var model = new EditCourseViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName,
                BigGrade = course.BigGrade.ToString(),
                SmallGrade = course.SmallGrade.ToString(),
                Level = course.Level,
                Term = course.Term,
                IsSubCourse = course.IsSubCourse,
                Note = course.Note,
                ParentId = course.ParentId,
                SpecializationId = course.Specialization.Id,
                Specialization = course.Specialization
            };
           
            return PartialView("_Edit", model);
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditCourseViewModel model)
        {
            ModelState.ClearValidationState(nameof(model));
            int newBigGrade = 0;
            if (model.BigGrade is not null)
            {
                var cultureInfo = new CultureInfo("en");
                if (!(int.TryParse(model.BigGrade.ToString(),
                    NumberStyles.Integer,
                    cultureInfo, out var modelBigGrade)) || !(modelBigGrade >= 0 && modelBigGrade <= 100))
                {
                    ModelState.AddModelError(nameof(model.BigGrade), " الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");

                    //return PartialView("_Create", model);
                }
                else { newBigGrade = modelBigGrade; }


            }
            else
            {
                ModelState.AddModelError(nameof(model.BigGrade), "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");

                //return PartialView("_Create", model);

            }

            int newSmallGrade = 0;
            if (model.SmallGrade is not null)
            {
                var cultureInfo = new CultureInfo("en");
                if (!(int.TryParse(model.SmallGrade.ToString(),
                    NumberStyles.Integer,
                    cultureInfo, out var modelSmallGrade)) || !(modelSmallGrade >= 0 && modelSmallGrade <= 100))
                {
                    ModelState.AddModelError(nameof(model.SmallGrade), " الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");

                    //return PartialView("_Create", model);
                }
                else { newSmallGrade = modelSmallGrade; }


            }
            else
            {
                ModelState.AddModelError(nameof(model.SmallGrade), "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");

                return PartialView("_Create", model);

            }

            if (string.IsNullOrEmpty(model.CourseName))
            {
                //FullAllLists();
                ModelState.AddModelError(nameof(model.CourseName), " الرجاء إدخال اسم المادة بطول  40 حرفًا على الاكثر.");

                //return PartialView("_Create", model);
            }
            else
            {
                var course1 = CourseRepository.List().SingleOrDefault(x => x.CourseName == model.CourseName);
                if (course1 != null && course1.Id != model.Id)
                {
                    ModelState.AddModelError(nameof(model.CourseName), "لقد تم إيجاد مادة بنفس اسم .. الرجاء كتابة اسم آخر ");                   
                }
            }
            
           
            if (model.IsSubCourse)
            {
                if (model.ParentId == -1 || model.ParentId == null)
                {
                    //FullAllLists();
                    ModelState.AddModelError(nameof(model.ParentId), "الرجاء اختيار المادة الاساسية من القائمة");

                    //return PartialView("_Create", model);
                }
            }



            if (!TryValidateModel(model, nameof(model)))
            {
                FullAllLists();
                return PartialView("_Edit", model);
            }

            try
            {
                var specialization = _specializationRepository.Find(model.SpecializationId);
                Course course = CourseRepository.Find(model.Id);

                course.CourseName = model.CourseName;
                course.BigGrade = newBigGrade;
                course.SmallGrade = newSmallGrade;
                course.Level = model.Level;
                course.Term = model.Term;
                course.IsSubCourse = model.IsSubCourse;
                course.Note = model.Note;
                course.ParentId = model.ParentId;
                course.Specialization = specialization;


                CourseRepository.Update(id, course);
                //return RedirectToAction(nameof(Index));
                return PartialView("_Edit", model);
            }
            catch
            {
                return PartialView("_Edit", model);
            }
        }

        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var course = CourseRepository.Find(id);
            if (course is null)
            {
                return NotFound();
            }
           

            return PartialView("_Delete", course);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,Course course)
        {
            //ModelState.ClearValidationState(nameof(course));
            try
            {
                var SubCoursesOFCourse = CourseRepository.List().Where(x => x.ParentId == id).ToList();
                if (SubCoursesOFCourse != null && SubCoursesOFCourse.Count > 0)
                {
                    ModelState.AddModelError(nameof(course.CourseName), "");
                    ViewBag.Message = "لا يمكن حذف المادة بسبب وجود مواد فرعية تابعة لها";
                    return PartialView("_Delete", course);
                }
                var CourseGradesOFCourse = _CourseGradeRepository.List().Where(x => x.Course.Id == id).ToList();
                if (CourseGradesOFCourse != null && CourseGradesOFCourse.Count > 0)
                {
                    ModelState.AddModelError(nameof(course.CourseName), "");
                    ViewBag.Message = "لا يمكن حذف المادة بسبب وجود درجات مرصودة للطلاب في هذه المادة";
                    return PartialView("_Delete", course);
                }

                CourseRepository.Delete(id);
                return PartialView("_Delete", course);
            }
            catch
            {
                return PartialView("_Delete", course);
            }
        }

        List<Specialization> FillSelectSpecializationsList()
        {
            var specializations = _specializationRepository.List().ToList();
           

            return specializations;
        }

        List<Course> FillSelectParentsCoursesList()
        {
            var Courses = CourseRepository.List()
                .Where(x => x.IsSubCourse == false)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .ToList();
            Courses.Insert(0, new Course { Id = -1,  CourseName = "-- أختر --" });
            return Courses;
        }

        void FullAllLists()
        {   
            ViewData["ParentsCourses"] = new SelectList(FillSelectParentsCoursesList(), "Id", "CourseName");
            ViewData["Specializations"] = new SelectList(FillSelectSpecializationsList(), "Id", "SpecializationName");
            
        }

        private bool isCourseNameExists(string courseName)
        {
            return CourseRepository.List().Any(e => e.CourseName == courseName);
        }
    }
}
