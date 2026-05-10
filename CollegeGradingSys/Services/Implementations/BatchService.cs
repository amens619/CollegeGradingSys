using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Batch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class BatchService : GenericService<Batch>, IBatchService
    {
        private readonly IRepository<Batch> _batchRepo;
        private readonly IRepository<Specialization> _specializationRepo;
        private readonly IStAcademicDataRepository _stAcademicRepo;

        public BatchService(IRepository<Batch> batchRepo,
            IRepository<Specialization> specializationRepo,
            IStAcademicDataRepository stAcademicRepo) : base(batchRepo)
        {
            _batchRepo = batchRepo;
            _specializationRepo = specializationRepo;
            _stAcademicRepo = stAcademicRepo;
        }





        // =======================
        // منطق خاص بـ Batch
        // =======================       
        public async Task<Batch> CreateAsync(BatchCreateDataVM dto)
        {
            if (string.IsNullOrWhiteSpace(dto.BatchName))
                throw new DomainException("اسم الدفعة مطلوب");

            if (await IsBatchNameExistsAsync(dto.BatchName.Trim()))
                throw new DomainException("اسم الدفعة موجود مسبقًا");

            if (dto.SpecializationId <= 0)
                throw new DomainException("يجب اختيار التخصص");

            var specialization = await _specializationRepo.FindAsync(dto.SpecializationId)
                ?? throw new DomainException("التخصص غير موجود");

            var batch = new Batch
            {
                BatchName = dto.BatchName.Trim(),
                Note = dto.Note,
                Specialization = specialization
            };

            return await _batchRepo.AddAsync(batch);
        }
               

        public async Task UpdateBatchAsync(BatchEditVM vm)
        {
            var batch = await _batchRepo.FindAsync(vm.Id)
                ?? throw new DomainException("الدفعة غير موجودة");

            bool exists = await _batchRepo.Query()
                .AnyAsync(x => x.BatchName == vm.BatchName && x.Id != vm.Id);

            if (exists)
                throw new DomainException("يوجد دفعة أخرى بنفس الاسم");

            var specialization = await _specializationRepo.FindAsync(vm.SpecializationId)
                ?? throw new DomainException("التخصص غير موجود");

            batch.BatchName = vm.BatchName.Trim();
            batch.Specialization = specialization;
            batch.Note = vm.Note;

            await _batchRepo.UpdateAsync(batch);
        }


        public async Task DeleteAsync(int id)
        {
            var batch = await _batchRepo.FindAsync(id)
                ?? throw new DomainException("الدفعة غير موجودة");

            bool isUsed = await _stAcademicRepo.Query()
                .AnyAsync(x => x.Batch.Id == id);

            if (isUsed)
                throw new DomainException("لا يمكن حذف دفعة مرتبطة بطلاب مقيدين");

            await _batchRepo.DeleteAsync(id);
        }


        public async Task<IList<Batch>> GetBatchesAsync(int? specializationId)
        {
            var query = _batchRepo.Query()
                .Include(x => x.Specialization)
                .AsQueryable();

            if (specializationId.HasValue && specializationId != -1)
                query = query.Where(x => x.Specialization.Id == specializationId);

            return await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
              

        public async Task<bool> IsBatchNameExistsAsync(string batchName)
        {
            return await _batchRepo.Query()
                .AnyAsync(x => x.BatchName == batchName);
        }

       
    }

}
