using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICollegeGradingSysRepository<Course> CourseRepository;
        private readonly ICollegeGradingSysRepository<Specialization> _specializationRepository;
        public CourseController(ICollegeGradingSysRepository<Course> CourseRepository, ICollegeGradingSysRepository<Specialization> specializationRepository)
        {
            this.CourseRepository = CourseRepository;
            this._specializationRepository = specializationRepository;
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


            FullAllLists();
            return PartialView("_Create", model);
        }

       

       


        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
            {

                    if (model.IsSubCourse)
                    {
                        if (model.ParentId == -1 || model.ParentId == null)
                        {
                        FullAllLists();
                        ModelState.AddModelError(nameof(model.ParentId), "الرجاء اختيار المادة الاساسية من القائمة");
                       
                            return PartialView("_Create", model);
                        }
                    }
                    if (string.IsNullOrEmpty(model.CourseName))
                    {
                        FullAllLists();
                        ModelState.AddModelError(nameof(model.CourseName), " يجب إدخال اسم المادة بطول  40 حرفًا على الاكثر.");

                        return PartialView("_Create", model);
                    }
                    var specialization = _specializationRepository.Find(model.SpecializationId);
                    Course course = new()
                    {
                        Id = model.Id,
                        CourseName = model.CourseName,
                        BigGrade = model.BigGrade,
                        SmallGrade = model.SmallGrade,
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
            else
            {
                FullAllLists();
                return PartialView("_Create", model);
            }
           

        }

        // GET: CourseController/Edit/5
        public ActionResult Edit(int id)
        {
            var course = CourseRepository.Find(id);

            //var governorateId = city.District == null ? city.District.Id = 0 : city.District.Id;
            var model = new EditCourseViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName,
                BigGrade = course.BigGrade,
                SmallGrade = course.SmallGrade,
                Level = course.Level,
                Term = course.Term,
                IsSubCourse = course.IsSubCourse,
                Note = course.Note,
                ParentId = course.ParentId,
                SpecializationId = course.Specialization.Id,
                Specialization = course.Specialization
            };
            FullAllLists();
            return PartialView("_Edit", model);
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditCourseViewModel model)
        {
            try
            {
                var specialization = _specializationRepository.Find(model.SpecializationId);
                Course course = new()
                {
                    Id = model.Id,
                    CourseName = model.CourseName,
                    BigGrade = model.BigGrade,
                    SmallGrade = model.SmallGrade,
                    Level = model.Level,
                    Term = model.Term,
                    IsSubCourse = model.IsSubCourse,
                    Note = model.Note,
                    ParentId = model.ParentId,                   
                    Specialization = specialization,
                     
                };
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
            var course = CourseRepository.Find(id);

            return PartialView("_Delete", course);
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,Course course)
        {
            try
            {

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
    }
}
