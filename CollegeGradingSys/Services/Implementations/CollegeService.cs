using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.AcademicYear;
using CollegeGradingSys.ViewModels.College;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class CollegeService : GenericService<College>, ICollegeService
    {
        private readonly IRepository<College> _repo;
        public CollegeService(IRepository<College> repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task CreateAsync(CollegeCreateVM vm)
        {
            if (await ExistsByNameAsync(vm.CollegeName))
                throw new DomainException("يوجد كلية بنفس الاسم");

            var college = new College
            {
                CollegeName = vm.CollegeName.Trim()
            };

            await _repo.AddAsync(college);
        }

       

        public async Task UpdateAsync(CollegeVM vm)
        {
            var college = await _repo.FindAsync(vm.Id);
            if (college == null)
                throw new DomainException("الكلية غير موجودة");


            // نمرر الـ Id لكي يستثني الكلية الحالية من البحث
            if (await ExistsByNameAsync(vm.CollegeName, vm.Id))
                throw new DomainException("يوجد كلية بنفس الاسم");

            college.CollegeName = vm.CollegeName.Trim();
            await _repo.UpdateAsync(college);
        }

       
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
           

            var query = _repo.Query().Where(x => x.CollegeName == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

      

    }

}
