using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class NationalityService : GenericService<Nationality>, INationalityService
    {
        private readonly IRepository<Nationality> _repository;
        private readonly IGovernorateService _governorateService;
        private readonly IStPersonalDataService _stPersonalDataService;

        public NationalityService(
            IRepository<Nationality> repository,
            IGovernorateService governorateService,
            IStPersonalDataService stPersonalDataService)
            : base(repository)
        {
            _repository = repository;
            _governorateService = governorateService;
            _stPersonalDataService = stPersonalDataService;
        }

        // ================= CREATE =================
        public async Task CreateAsync(Nationality nationality)
        {
            if (string.IsNullOrWhiteSpace(nationality.CountryName))
                throw new DomainException("الرجاء كتابة اسم الدولة");

            if (string.IsNullOrWhiteSpace(nationality.NationalityName))
                throw new DomainException("الرجاء كتابة اسم الجنسية");

            if (await _repository.Query()
                .AnyAsync(x => x.CountryName == nationality.CountryName))
                throw new DomainException("لقد تم إيجاد دولة سابقة بنفس الاسم");

            if (await _repository.Query()
                .AnyAsync(x => x.NationalityName == nationality.NationalityName))
                throw new DomainException("لقد تم إيجاد جنسية سابقة بنفس الاسم");

            await _repository.AddAsync(nationality);
        }

        // ================= UPDATE =================
        public async Task UpdateAsync(Nationality nationality)
        {
            if (string.IsNullOrWhiteSpace(nationality.CountryName))
                throw new DomainException("الرجاء كتابة اسم الدولة");

            if (string.IsNullOrWhiteSpace(nationality.NationalityName))
                throw new DomainException("الرجاء كتابة اسم الجنسية");

            if (await _repository.Query()
                .AnyAsync(x =>
                    x.CountryName == nationality.CountryName &&
                    x.Id != nationality.Id))
                throw new DomainException("اسم الدولة مستخدم مسبقاً");

            if (await _repository.Query()
                .AnyAsync(x =>
                    x.NationalityName == nationality.NationalityName &&
                    x.Id != nationality.Id))
                throw new DomainException("اسم الجنسية مستخدم مسبقاً");

            await _repository.UpdateAsync(nationality);
        }

        // ================= DELETE =================
        public async Task DeleteWithValidationAsync(int id)
        {
            await _governorateService.EnsureCountryCanBeDeletedAsync(id);
            await _stPersonalDataService.EnsureCountryCanBeDeletedAsync(id);

            await _repository.DeleteAsync(id);
        }
    }
}
