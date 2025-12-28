using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class CollegeService : ICollegeService
    {
        private readonly IRepository<College> _repo;

        public CollegeService(IRepository<College> repo)
        {
            _repo = repo;
        }

        public async Task<IList<College>> GetAllAsync()
            => await _repo.ListAsync();

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

        public async Task<College?> GetByIdAsync(int id)
        {
            return await _repo.FindAsync(id);
        }

        public async Task UpdateAsync(CollegeVM vm)
        {
            var college = await _repo.FindAsync(vm.Id);
            if (college == null)
                throw new DomainException("الكلية غير موجودة");

            var exists = await _repo.Query()
                .AnyAsync(x => x.CollegeName == vm.CollegeName && x.Id != vm.Id);

            if (exists)
                throw new DomainException("يوجد كلية بنفس الاسم");

            college.CollegeName = vm.CollegeName.Trim();
            await _repo.UpdateAsync(college);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            if (!await _repo.ExistsAsync(id))
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repo.Query()
                .AnyAsync(x => x.CollegeName == name);
        }
    }

}
