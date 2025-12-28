using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class SpecializationService : GenericService<Specialization>, ISpecializationService
    {
        private readonly IRepository<Specialization> _specializationRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Batch> _batchRepository;

        public SpecializationService(
            IRepository<Specialization> specializationRepository,
            IRepository<Department> departmentRepository,
            IRepository<Course> courseRepository,
            IRepository<Batch> batchRepository)
            : base(specializationRepository)
        {
            _specializationRepository = specializationRepository;
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _batchRepository = batchRepository;
        }

        // Use Cases
        public async Task<DepartmentSpecializationViewModel> GetCreateViewModelAsync()
        {
            var departments = await GetDepartmentsAsync();
            return new DepartmentSpecializationViewModel
            {
                Departments = departments
            };
        }

        public async Task<(bool Success, string ErrorMessage)> CreateSpecializationAsync(DepartmentSpecializationViewModel model)
        {
            if (string.IsNullOrEmpty(model.SpecializationName))
            {
                return (false, "الرجاء كتابة اسم التخصص");
            }

            if (await IsSpecializationNameExistsAsync(model.SpecializationName.Trim()))
            {
                return (false, "لقد تم إيجاد تخصص سابقة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            if (model.DepartmentId == -1)
            {
                return (false, "الرجاء اختيار القسم من القائمة");
            }

            var department = await _departmentRepository.FindAsync(model.DepartmentId);
            if (department == null)
            {
                return (false, "القسم المحدد غير موجود");
            }

            var specialization = new Specialization
            {
                Id = model.Id,
                SpecializationName = model.SpecializationName.Trim(),
                Department = department
            };

            await _specializationRepository.AddAsync(specialization);
            return (true, string.Empty);
        }

        public async Task<DepartmentSpecializationViewModel> GetEditViewModelAsync(int id)
        {
            var specialization = await _specializationRepository.FindAsync(id);
            if (specialization == null)
                return null;

            var departments = await GetDepartmentsAsync();
            return new DepartmentSpecializationViewModel
            {
                Id = specialization.Id,
                SpecializationName = specialization.SpecializationName,
                DepartmentId = specialization.Department.Id,
                Departments = departments
            };
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateSpecializationAsync(int id, DepartmentSpecializationViewModel model)
        {
            if (string.IsNullOrEmpty(model.SpecializationName))
            {
                return (false, "الرجاء كتابة اسم التخصص");
            }

            var allSpecializations = await _specializationRepository.ListAsync();
            var existingSpecialization = allSpecializations
                .SingleOrDefault(x => x.SpecializationName == model.SpecializationName);
            if (existingSpecialization != null && existingSpecialization.Id != model.Id)
            {
                return (false, "لقد تم إيجاد تخصص سابقة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            var department = await _departmentRepository.FindAsync(model.DepartmentId);
            if (department == null)
            {
                return (false, "القسم المحدد غير موجود");
            }

            var specialization = await _specializationRepository.FindAsync(model.Id);
            if (specialization == null)
            {
                return (false, "التخصص غير موجود");
            }

            specialization.SpecializationName = model.SpecializationName.Trim();
            specialization.Department = department;

            await _specializationRepository.UpdateAsync(specialization);
            return (true, string.Empty);
        }

        public async Task<(bool CanDelete, string ErrorMessage)> CanDeleteSpecializationAsync(int id)
        {
            var allCourses = await _courseRepository.ListAsync();
            var coursesOfSpecialization = allCourses
                .Where(x => x.Specialization.Id == id).ToList();
            if (coursesOfSpecialization != null && coursesOfSpecialization.Count > 0)
            {
                return (false, "لا يمكن حذف التخصص بسبب وجود مواد تابعة له.. الرجاء حذف المواد التابعة له أولا");
            }

            var allBatches = await _batchRepository.ListAsync();
            var batchesOfSpecialization = allBatches
                .Where(x => x.Specialization.Id == id).ToList();
            if (batchesOfSpecialization != null && batchesOfSpecialization.Count > 0)
            {
                return (false, "لا يمكن حذف التخصص بسبب وجود دفعات تابعة له.. الرجاء حذف الدفعات التابعة له أولا");
            }

            return (true, string.Empty);
        }

        public async Task<bool> IsSpecializationNameExistsAsync(string specializationName, int? excludeId = null)
        {
            var specializations = await _specializationRepository.ListAsync();
            if (excludeId.HasValue)
            {
                return specializations.Any(e => e.SpecializationName == specializationName && e.Id != excludeId.Value);
            }
            return specializations.Any(e => e.SpecializationName == specializationName);
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            var departments = (await _departmentRepository.ListAsync()).ToList();
            departments.Insert(0, new Department { Id = -1, DepartmentName = "-- أختر --" });
            return departments;
        }
    }
}

