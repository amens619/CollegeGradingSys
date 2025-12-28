using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.EntityFrameworkCore;
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

        // Use Cases Implementation
        public async Task<IList<Course>> GetFilteredCoursesAsync(Term? term, Level? level, int? specializationId)
        {
            var courses = (await GetAllAsync())
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .ToList();

            if (specializationId != null && specializationId != -1)
            {
                courses = courses.Where(x => x.Specialization?.Id == specializationId).ToList();
            }

            if (level != null)
            {
                courses = courses.Where(x => x.Level == level).ToList();
            }

            if (term != null)
            {
                courses = courses.Where(x => x.Term == term).ToList();
            }

            return courses;
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

        public async Task ValidateCourseNameAsync(string courseName, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(courseName))
            {
                throw new DomainException("الرجاء إدخال اسم المادة بطول 40 حرفًا على الاكثر.");
            }

            if (await IsCourseNameExistsAsync(courseName.Trim(), excludeId))
            {
                throw new DomainException("لقد تم إيجاد مادة بنفس اسم .. الرجاء كتابة اسم آخر");
            }
        }

        public async Task ValidateGradesAsync(string bigGrade, string smallGrade)
        {
            var cultureInfo = new CultureInfo("en");
            
            if (string.IsNullOrEmpty(bigGrade))
            {
                throw new DomainException("الرجاء إدخال الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            if (!int.TryParse(bigGrade, NumberStyles.Integer, cultureInfo, out var bigGradeValue) || 
                !(bigGradeValue >= 0 && bigGradeValue <= 100))
            {
                throw new DomainException("الرجاء إدخال الدرجة الكبرى رقماً صحيحا بين 0 - 100.");
            }

            if (string.IsNullOrEmpty(smallGrade))
            {
                throw new DomainException("الرجاء إدخال الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }

            if (!int.TryParse(smallGrade, NumberStyles.Integer, cultureInfo, out var smallGradeValue) || 
                !(smallGradeValue >= 0 && smallGradeValue <= 100))
            {
                throw new DomainException("الرجاء إدخال الدرجة الصغرى رقماً صحيحا بين 0 - 100.");
            }
        }

        public async Task<(int bigGrade, int smallGrade)> ParseGradesAsync(string bigGrade, string smallGrade)
        {
            await ValidateGradesAsync(bigGrade, smallGrade);
            
            var cultureInfo = new CultureInfo("en");
            int.TryParse(bigGrade, NumberStyles.Integer, cultureInfo, out var bigGradeValue);
            int.TryParse(smallGrade, NumberStyles.Integer, cultureInfo, out var smallGradeValue);
            
            return (bigGradeValue, smallGradeValue);
        }

        public async Task ValidateSubCourseAsync(bool isSubCourse, int? parentId)
        {
            if (isSubCourse && (parentId == -1 || parentId == null))
            {
                throw new DomainException("الرجاء اختيار المادة الاساسية من القائمة");
            }
        }

        public async Task<CourseDetailsViewModel> GetCourseDetailsAsync(int id)
        {
            var course = await GetByIdWithRelationsAsync(id);
            if (course == null)
                return null;

            return new CourseDetailsViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Level = course.Level,
                Term = course.Term,
                BigGrade = course.BigGrade,
                SmallGrade = course.SmallGrade,
                Course_sGender = course.Course_sGender,
                IsSubCourse = course.IsSubCourse,
                ParentCourseName = course.Parent?.CourseName,
                SpecializationName = course.Specialization?.SpecializationName,
                Note = course.Note
            };
        }

        public async Task<CreateCourseViewModel> PrepareCreateCourseViewModelAsync()
        {
            return new CreateCourseViewModel
            {
                BigGrade = "100",
                SmallGrade = "60",
                Course_sGender = Course_sGender.كلاالجنسين
            };
        }

        public async Task<EditCourseViewModel> PrepareEditCourseViewModelAsync(int id)
        {
            var course = await GetByIdWithRelationsAsync(id);
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
                SpecializationId = course.Specialization?.Id ?? 0,
                Specialization = course.Specialization
            };
        }

        public async Task CreateCourseAsync(CreateCourseViewModel viewModel)
        {
            // Validate
            await ValidateCourseNameAsync(viewModel.CourseName?.Trim());
            var (bigGrade, smallGrade) = await ParseGradesAsync(viewModel.BigGrade, viewModel.SmallGrade);
            await ValidateSubCourseAsync(viewModel.IsSubCourse, viewModel.ParentId);

            // Get specialization
            var specialization = await _specializationService.GetByIdAsync(viewModel.SpecializationId);
            if (specialization == null)
                throw new DomainException("التخصص المحدد غير موجود");

            // Create course
            var course = new Course
            {
                Id = viewModel.Id,
                CourseName = viewModel.CourseName.Trim(),
                BigGrade = bigGrade,
                SmallGrade = smallGrade,
                IsSubCourse = viewModel.IsSubCourse,
                Level = viewModel.Level,
                Course_sGender = viewModel.Course_sGender,
                ParentId = viewModel.ParentId,
                Term = viewModel.Term,
                Note = viewModel.Note,
                Specialization = specialization
            };

            await CreateAsync(course);
        }

        public async Task UpdateCourseAsync(EditCourseViewModel viewModel)
        {
            // Validate
            await ValidateCourseNameAsync(viewModel.CourseName?.Trim(), viewModel.Id);
            var (bigGrade, smallGrade) = await ParseGradesAsync(viewModel.BigGrade, viewModel.SmallGrade);
            await ValidateSubCourseAsync(viewModel.IsSubCourse, viewModel.ParentId);

            // Get specialization
            var specialization = await _specializationService.GetByIdAsync(viewModel.SpecializationId);
            if (specialization == null)
                throw new DomainException("التخصص المحدد غير موجود");

            // Get existing course
            var course = await GetByIdAsync(viewModel.Id);
            if (course == null)
                throw new DomainException("المادة غير موجودة");

            // Update course
            course.CourseName = viewModel.CourseName.Trim();
            course.BigGrade = bigGrade;
            course.SmallGrade = smallGrade;
            course.Level = viewModel.Level;
            course.Term = viewModel.Term;
            course.IsSubCourse = viewModel.IsSubCourse;
            course.Course_sGender = viewModel.Course_sGender;
            course.Note = viewModel.Note;
            course.ParentId = viewModel.ParentId;
            course.Specialization = specialization;

            await UpdateAsync(course);
        }

        public async Task<CourseDeleteViewModel> PrepareDeleteCourseViewModelAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course == null)
                return null;

            return new CourseDeleteViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName
            };
        }

        public async Task<CourseDeleteResult> CanDeleteCourseAsync(int id)
        {
            var allCourses = await GetAllAsync();
            var subCourses = allCourses.Where(x => x.ParentId == id).ToList();
            
            if (subCourses != null && subCourses.Count > 0)
            {
                return new CourseDeleteResult
                {
                    CanDelete = false,
                    ErrorMessage = "لا يمكن حذف المادة بسبب وجود مواد فرعية تابعة لها"
                };
            }

            var allCourseGrades = await _courseGradeService.GetAllAsync();
            var courseGrades = allCourseGrades.Where(x => x.Course?.Id == id).ToList();
            
            if (courseGrades != null && courseGrades.Count > 0)
            {
                return new CourseDeleteResult
                {
                    CanDelete = false,
                    ErrorMessage = "لا يمكن حذف المادة بسبب وجود درجات مرصودة للطلاب في هذه المادة"
                };
            }

            return new CourseDeleteResult { CanDelete = true };
        }

        public async Task DeleteCourseAsync(int id)
        {
            var deleteResult = await CanDeleteCourseAsync(id);
            if (!deleteResult.CanDelete)
            {
                throw new DomainException(deleteResult.ErrorMessage);
            }

            await DeleteAsync(id);
        }

        public async Task<List<SelectItemVM>> GetSpecializationsSelectItemsAsync()
        {
            var specializations = await _specializationService.GetAllAsync();
            return specializations.Select(s => new SelectItemVM
            {
                Id = s.Id,
                Name = s.SpecializationName
            }).ToList();
        }

        public async Task<List<SelectItemVM>> GetParentCoursesSelectItemsAsync()
        {
            var allCourses = await GetAllAsync();
            var parentCourses = allCourses
                .Where(x => !x.IsSubCourse)
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Term)
                .Select(c => new SelectItemVM
                {
                    Id = c.Id,
                    Name = c.CourseName
                })
                .ToList();

            parentCourses.Insert(0, new SelectItemVM { Id = -1, Name = "-- أختر --" });
            return parentCourses;
        }
    }
}
