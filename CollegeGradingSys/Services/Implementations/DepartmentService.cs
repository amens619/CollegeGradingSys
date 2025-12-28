using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class DepartmentService : GenericService<Department>, IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<College> _collegeRepository;
        private readonly IRepository<Specialization> _specializationRepository;

        public DepartmentService(
            IRepository<Department> departmentRepository,
            IRepository<College> collegeRepository,
            IRepository<Specialization> specializationRepository)
            : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _collegeRepository = collegeRepository;
            _specializationRepository = specializationRepository;
        }

        // Use Cases
        public async Task<CollegeDepartmentViewModel> GetCreateViewModelAsync()
        {
            var colleges = await GetCollegesAsync();
            return new CollegeDepartmentViewModel
            {
                Colleges = colleges
            };
        }

        public async Task<(bool Success, string ErrorMessage)> CreateDepartmentAsync(CollegeDepartmentViewModel model)
        {
            if (string.IsNullOrEmpty(model.DepartmentName))
            {
                return (false, "الرجاء كتابة اسم القسم");
            }

            if (await IsDepartmentNameExistsAsync(model.DepartmentName.Trim()))
            {
                return (false, "لقد تم إيجاد قسم سابقة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            if (model.CollegeId == -1)
            {
                return (false, "الرجاء اختيار الكلية من القائمة");
            }

            var college = await _collegeRepository.FindAsync(model.CollegeId);
            if (college == null)
            {
                return (false, "الكلية المحددة غير موجودة");
            }

            var department = new Department
            {
                Id = model.Id,
                DepartmentName = model.DepartmentName.Trim(),
                College = college
            };

            await _departmentRepository.AddAsync(department);
            return (true, string.Empty);
        }

        public async Task<CollegeDepartmentViewModel> GetEditViewModelAsync(int id)
        {
            var department = await _departmentRepository.FindAsync(id);
            if (department == null)
                return null;

            var colleges = await GetCollegesAsync();
            return new CollegeDepartmentViewModel
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                CollegeId = department.College.Id,
                Colleges = colleges
            };
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateDepartmentAsync(int id, CollegeDepartmentViewModel model)
        {
            if (string.IsNullOrEmpty(model.DepartmentName))
            {
                return (false, "الرجاء كتابة اسم القسم");
            }

            var allDepartments = await _departmentRepository.ListAsync();
            var existingDepartment = allDepartments
                .SingleOrDefault(x => x.DepartmentName == model.DepartmentName);
            if (existingDepartment != null && existingDepartment.Id != model.Id)
            {
                return (false, "لقد تم إيجاد قسم سابقة بنفس اسم .. الرجاء كتابة اسم آخر");
            }

            var college = await _collegeRepository.FindAsync(model.CollegeId);
            if (college == null)
            {
                return (false, "الكلية المحددة غير موجودة");
            }

            var department = await _departmentRepository.FindAsync(model.Id);
            if (department == null)
            {
                return (false, "القسم غير موجود");
            }

            department.DepartmentName = model.DepartmentName.Trim();
            department.College = college;

            await _departmentRepository.UpdateAsync(department);
            return (true, string.Empty);
        }

        public async Task<(bool CanDelete, string ErrorMessage)> CanDeleteDepartmentAsync(int id)
        {
            var allSpecializations = await _specializationRepository.ListAsync();
            var specializationsOfDepartment = allSpecializations
                .Where(x => x.Department.Id == id).ToList();
            if (specializationsOfDepartment != null && specializationsOfDepartment.Count > 0)
            {
                return (false, "لا يمكن حذف القسم بسبب وجود تخصصات تابعة له.. الرجاء حذف التخصصات التابعة له أولا");
            }

            return (true, string.Empty);
        }

        public async Task<bool> IsDepartmentNameExistsAsync(string departmentName, int? excludeId = null)
        {
            var departments = await _departmentRepository.ListAsync();
            if (excludeId.HasValue)
            {
                return departments.Any(e => e.DepartmentName == departmentName && e.Id != excludeId.Value);
            }
            return departments.Any(e => e.DepartmentName == departmentName);
        }

        public async Task<List<College>> GetCollegesAsync()
        {
            var colleges = (await _collegeRepository.ListAsync()).ToList();
            colleges.Insert(0, new College { Id = -1, CollegeName = "-- أختر --" });
            return colleges;
        }
    }
}

