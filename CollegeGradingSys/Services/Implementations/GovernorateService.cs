using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class GovernorateService
    : GenericService<Governorate>, IGovernorateService 
    {
        private readonly IRepository<Governorate> _repo;
        private readonly IStPersonalDataRepository _stPersonalRepo;

        public GovernorateService(
            IRepository<Governorate> repo,
            IStPersonalDataRepository stPersonalRepo)
            : base(repo)
        {
            _repo = repo;
            _stPersonalRepo = stPersonalRepo;
        }

        public async Task EnsureCountryCanBeDeletedAsync(int countryId)
        {
            if (await _repo
                .Query()
                .AnyAsync(x =>
                    x.Nationality.Id == countryId ))
                throw new DomainException(
                    "لا يمكن حذف الدولة بسبب وجود محافظات تابعة لها.. الرجاء حذف المحافظات التابعة لها أولا ");
             
        }

        // ================= CREATE =================
        public async Task CreateWithEnsureGovernorateNameAsync(Governorate governorate)
        {
            if (string.IsNullOrWhiteSpace(governorate.GovernorateName))
                throw new DomainException("الرجاء كتابة اسم المحافظة");

            if (await IsGovernorateNameExistsAsync(governorate.GovernorateName))
                throw new DomainException("تم إيجاد محافظة بنفس الاسم مسبقاً");

            await _repo.AddAsync(governorate);
        }

        // ================= UPDATE =================
        public async Task UpdateEnsureGovernorateNameAsync(Governorate governorate)
        {
            if (string.IsNullOrWhiteSpace(governorate.GovernorateName))
                throw new DomainException("الرجاء كتابة اسم المحافظة");

            if (await IsGovernorateNameExistsAsync(
                governorate.GovernorateName,
                governorate.Id))
                throw new DomainException("اسم المحافظة مستخدم مسبقاً");

            await _repo.UpdateAsync(governorate);
        }

        // ================= DELETE =================
        public async Task DeleteWithValidationAsync(int id)
        {
            await EnsureGovernorateCanBeDeletedAsync(id);
            await _repo.DeleteAsync(id);
        }

        public async Task EnsureGovernorateCanBeDeletedAsync(int governorateId)
        {
            if (await _stPersonalRepo
                .Query()
                .AnyAsync(x => x.BirthGovernorate.Id == governorateId))
                throw new DomainException(
                    "لا يمكن حذف المحافظة بسبب وجود بيانات طلاب تابعة لها");
        }

        // ================= HELPERS =================
        public async Task<bool> IsGovernorateNameExistsAsync(string name, int? excludeId = null)
        {
            return await _repo.Query().AnyAsync(x =>
                x.GovernorateName == name &&
                (!excludeId.HasValue || x.Id != excludeId.Value));
        }


    }

}
