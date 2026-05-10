using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Course;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IGenericService<Specialization> _specializationService;
        private readonly ICourseGradeRepository _courseGradeRepo;       
        private readonly IRepository<Batch> _batchRepo;

        public CourseService(
            ICourseRepository courseRepo,
            IGenericService<Specialization> specializationService,
                IRepository<Batch> batchRepo,
                ICourseGradeRepository courseGradeRepo
           )
                : base(courseRepo)
        {
            _courseRepo = courseRepo;
            _specializationService = specializationService;
            _courseGradeRepo = courseGradeRepo;           
            _batchRepo = batchRepo;
        }

        //public async Task<IList<Course>> GetAllAsync()
        //{
        //    return await _courseRepository.ListAsync();
        //}

        public async Task<IList<Course>> GetAllWithRelationsAsync()
        {
            return await _courseRepo.ListWithRelationsAsync();
        }

        //public async Task<Course> GetByIdAsync(int id)
        //{
        //    return await _courseRepository.FindAsync(id);
        //}

        public async Task<Course> GetByIdWithRelationsAsync(int id)
        {
            return await _courseRepo.FindWithRelationsAsync(id);
        }

        //public async Task<Course> CreateAsync(Course course)
        //{
        //    return await _courseRepository.AddAsync(course);
        //}

        //public async Task<Course> UpdateAsync(Course course)
        //{
        //    return await _courseRepository.UpdateAsync(course);
        //}

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    if (!await _courseRepository.ExistsAsync(id))
        //        return false;

        //    await _courseRepository.DeleteAsync(id);
        //    return true;
        //}

        //public async Task<bool> ExistsAsync(int id)
        //{
        //    return await _courseRepository.ExistsAsync(id);
        //}

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
            var vm = new CourseIndexViewModel();

            // 1. نبدأ بناء الاستعلام (IQueryable) بدون تنفيذه
            // لاحظ أننا ألغينا Include(Level) و Include(Term) لأنها أعمدة عادية
            var query = _courseRepo.Query()
                .Include(x => x.Specialization)
                .AsQueryable();

            // 2. نطبق الفلاتر على مستوى قاعدة البيانات (SQL)
            if (specializationId != null && specializationId != -1)
            {
                vm.SpecializationId = specializationId;
                query = query.Where(x => x.Specialization != null && x.Specialization.Id == specializationId);
            }

            if (level != null)
            {
                vm.Level = level;
                query = query.Where(x => x.Level == level);
            }

            if (term != null)
            {
                vm.Term = term;
                query = query.Where(x => x.Term == term);
            }

            // 3. أخيراً: نرتب البيانات وننفذ الاستعلام (نجلب البيانات المتوافقة فقط من الداتابيز)
            var courses = await query
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .ToListAsync(); // نستخدم ToListAsync لأن الدالة async

            vm.Courses = courses;
            return vm;
        }

        public CreateCourseViewModel GetCreateViewModelAsync()
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
            bool hasSubCourses = await _courseRepo.Query()
                .AnyAsync(x => x.ParentId == id);

            if (hasSubCourses)
            {
                return (false, "لا يمكن حذف المادة بسبب وجود مواد فرعية تابعة لها");
            }                       
            bool hasGrades = await _courseGradeRepo.Query()
                .AnyAsync(x => x.Course.Id == id);

            if (hasGrades)
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

        public async Task<List<SelectItemVM>> GetCoursesByBatchAsync(int batchId, Level level, Term term)
        {
            // 1. جلب بيانات الدفعة لمعرفة التخصص الخاص بها
            var batch = await _batchRepo.Query()
                .Include(x => x.Specialization)
                .FirstOrDefaultAsync(x => x.Id == batchId);

            if (batch == null || batch.Specialization == null)
                return new List<SelectItemVM>();

            // 2. جلب المواد بناءً على تخصص الدفعة والمستوى والترم
            var courses = await _courseRepo.Query()
                .Where(x => x.Specialization.Id == batch.Specialization.Id
                         && x.Level == level
                         && x.Term == term)
                .Select(x => new SelectItemVM { Id = x.Id, Name = x.CourseName }) // تحويل سريع
                .ToListAsync();

            return courses;
        }

        public async Task<List<SelectItemVM>> GetCoursesBySpecializationAsync(int specializationId, Level? level, Term? term)
        {
            var query = _courseRepo.Query().Where(x => x.Specialization.Id == specializationId);

            if (level.HasValue)
                query = query.Where(x => x.Level == level.Value);

            if (term.HasValue)
                query = query.Where(x => x.Term == term.Value);

            var courses = await query
                .Select(x => new SelectItemVM { Id = x.Id, Name = x.CourseName })
                .ToListAsync();

            return courses;
        }
    }
}
