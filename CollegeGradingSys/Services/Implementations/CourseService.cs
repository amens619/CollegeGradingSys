using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IGenericService<Specialization> _specializationService;
        private readonly ICourseGradeService _courseGradeService;

        public CourseService(
            ICourseRepository courseRepository,
            IGenericService<Specialization> specializationService,
            ICourseGradeService courseGradeService)
                : base(courseRepository)
        {
            _courseRepository = courseRepository;
            _specializationService = specializationService;
            _courseGradeService = courseGradeService;
        }

        public async Task<IList<Course>> GetAllAsync()
        {
            return await _courseRepository.ListAsync();
        }

        public async Task<IList<Course>> GetAllWithRelationsAsync()
        {
            return await _courseRepository.ListWithRelationsAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _courseRepository.FindAsync(id);
        }

        public async Task<Course> GetByIdWithRelationsAsync(int id)
        {
            return await _courseRepository.FindWithRelationsAsync(id);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            return await _courseRepository.AddAsync(course);
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            return await _courseRepository.UpdateAsync(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _courseRepository.ExistsAsync(id))
                return false;

            await _courseRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _courseRepository.ExistsAsync(id);
        }

        // Use Cases
        public async Task<CourseDetailsViewModel> GetDetailsViewModelAsync(int id)
        {
            var course = await GetByIdWithRelationsAsync(id);
            if (course == null)
                return null;

            return new CourseDetailsViewModel
            {
                Id = course.Id,
                Level = course.Level,
                Term = course.Term,
                CourseName = course.CourseName,
                BigGrade = course.BigGrade,
                SmallGrade = course.SmallGrade,
                Note = course.Note,
                Course_sGender = course.Course_sGender,
                ParentId = course.ParentId,
                IsSubCourse = course.IsSubCourse,
                SpecializationName = course.Specialization?.SpecializationName,
                ParentCourseName = course.Parent?.CourseName
            };
        }

        public async Task<CourseIndexViewModel> GetIndexViewModelAsync(Term? term, Level? level, int? specializationId)
        {
            var courses = (await GetAllAsync())
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .ToList();

            var vm = new CourseIndexViewModel();

            if (specializationId != null && specializationId != -1)
            {
                vm.SpecializationId = specializationId;
                courses = courses.Where(x => x.Specialization.Id == specializationId).ToList();
            }

            if (level != null)
            {
                vm.Level = level;
                courses = courses.Where(x => x.Level == level).ToList();
            }

            if (term != null)
            {
                vm.Term = term;
                courses = courses.Where(x => x.Term == term).ToList();
            }

            vm.Courses = courses;
            return vm;
        }

        public async Task<CreateCourseViewModel> GetCreateViewModelAsync()
        {
            return new CreateCourseViewModel
            {
                BigGrade = "100",
                SmallGrade = "60",
                Course_sGender = Course_sGender.كلاالجنسين
            };
        }

        public async Task<(bool Success, string ErrorMessage)> CreateCourseAsync(CreateCourseViewModel model)
        {
            // Validate BigGrade
            if (string.IsNullOrEmpty(model.BigGrade))
            {
                return (false, "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            var cultureInfo = new CultureInfo("en");
            if (!int.TryParse(model.BigGrade, NumberStyles.Integer, cultureInfo, out var bigGrade) || 
                !(bigGrade >= 0 && bigGrade <= 100))
            {
                return (false, "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            // Validate SmallGrade
            if (string.IsNullOrEmpty(model.SmallGrade))
            {
                return (false, "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }

            if (!int.TryParse(model.SmallGrade, NumberStyles.Integer, cultureInfo, out var smallGrade) || 
                !(smallGrade >= 0 && smallGrade <= 100))
            {
                return (false, "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }

            // Validate CourseName
            if (string.IsNullOrEmpty(model.CourseName))
            {
                return (false, "الرجاء إدخال اسم المادة بطول  40 حرفًا على الاكثر.");
            }

            if (await IsCourseNameExistsAsync(model.CourseName.Trim()))
            {
                return (false, "لقد تم إيجاد مادة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            // Validate ParentId if IsSubCourse
            if (model.IsSubCourse && (model.ParentId == -1 || model.ParentId == null))
            {
                return (false, "الرجاء اختيار المادة الاساسية من القائمة");
            }

            var specialization = await _specializationService.GetByIdAsync(model.SpecializationId);
            if (specialization == null)
            {
                return (false, "التخصص المحدد غير موجود");
            }

            var course = new Course
            {
                Id = model.Id,
                CourseName = model.CourseName.Trim(),
                BigGrade = bigGrade,
                SmallGrade = smallGrade,
                IsSubCourse = model.IsSubCourse,
                Level = model.Level,
                Course_sGender = model.Course_sGender,
                ParentId = model.ParentId,
                Term = model.Term,
                Note = model.Note,
                Specialization = specialization
            };

            await CreateAsync(course);
            return (true, string.Empty);
        }

        public async Task<EditCourseViewModel> GetEditViewModelAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course == null)
                return null;

            return new EditCourseViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName,
                BigGrade = course.BigGrade.ToString(),
                SmallGrade = course.SmallGrade.ToString(),
                Level = course.Level,
                Term = course.Term,
                IsSubCourse = course.IsSubCourse,
                Course_sGender = course.Course_sGender,
                Note = course.Note,
                ParentId = course.ParentId,
                SpecializationId = course.Specialization.Id,
                Specialization = course.Specialization
            };
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateCourseAsync(EditCourseViewModel model)
        {
            // Validate BigGrade
            if (string.IsNullOrEmpty(model.BigGrade))
            {
                return (false, "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            var cultureInfo = new CultureInfo("en");
            if (!int.TryParse(model.BigGrade, NumberStyles.Integer, cultureInfo, out var bigGrade) || 
                !(bigGrade >= 0 && bigGrade <= 100))
            {
                return (false, "الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            // Validate SmallGrade
            if (string.IsNullOrEmpty(model.SmallGrade))
            {
                return (false, "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }

            if (!int.TryParse(model.SmallGrade, NumberStyles.Integer, cultureInfo, out var smallGrade) || 
                !(smallGrade >= 0 && smallGrade <= 100))
            {
                return (false, "الرجاء إدخال  الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }

            // Validate CourseName
            if (string.IsNullOrEmpty(model.CourseName))
            {
                return (false, "الرجاء إدخال اسم المادة بطول  40 حرفًا على الاكثر.");
            }

            if (await IsCourseNameExistsAsync(model.CourseName.Trim(), model.Id))
            {
                return (false, "لقد تم إيجاد مادة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            // Validate ParentId if IsSubCourse
            if (model.IsSubCourse && (model.ParentId == -1 || model.ParentId == null))
            {
                return (false, "الرجاء اختيار المادة الاساسية من القائمة");
            }

            var specialization = await _specializationService.GetByIdAsync(model.SpecializationId);
            if (specialization == null)
            {
                return (false, "التخصص المحدد غير موجود");
            }

            var course = await GetByIdAsync(model.Id);
            if (course == null)
            {
                return (false, "المادة غير موجودة");
            }

            course.CourseName = model.CourseName.Trim();
            course.BigGrade = bigGrade;
            course.SmallGrade = smallGrade;
            course.Level = model.Level;
            course.Term = model.Term;
            course.IsSubCourse = model.IsSubCourse;
            course.Course_sGender = model.Course_sGender;
            course.Note = model.Note;
            course.ParentId = model.ParentId;
            course.Specialization = specialization;

            await UpdateAsync(course);
            return (true, string.Empty);
        }

        public async Task<(bool CanDelete, string ErrorMessage)> CanDeleteCourseAsync(int id)
        {
            var allCourses = await GetAllAsync();
            var subCourses = allCourses.Where(x => x.ParentId == id).ToList();
            if (subCourses != null && subCourses.Count > 0)
            {
                return (false, "لا يمكن حذف المادة بسبب وجود مواد فرعية تابعة لها");
            }

            var allCourseGrades = await _courseGradeService.GetAllAsync();
            var courseGrades = allCourseGrades.Where(x => x.Course.Id == id).ToList();
            if (courseGrades != null && courseGrades.Count > 0)
            {
                return (false, "لا يمكن حذف المادة بسبب وجود درجات مرصودة للطلاب في هذه المادة");
            }

            return (true, string.Empty);
        }

        public async Task<bool> IsCourseNameExistsAsync(string courseName, int? excludeId = null)
        {
            var courses = await GetAllAsync();
            if (excludeId.HasValue)
            {
                return courses.Any(e => e.CourseName == courseName && e.Id != excludeId.Value);
            }
            return courses.Any(e => e.CourseName == courseName);
        }

        public async Task<List<Course>> GetParentCoursesAsync()
        {
            var allCourses = await GetAllAsync();
            var courses = allCourses
                .Where(x => x.IsSubCourse == false)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .ToList();
            courses.Insert(0, new Course { Id = -1, CourseName = "-- أختر --" });
            return courses;
        }

        public async Task<List<Specialization>> GetSpecializationsAsync()
        {
            return (await _specializationService.GetAllAsync()).ToList();
        }
    }
}
