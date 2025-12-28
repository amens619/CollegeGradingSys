//using CollegeGradingSys.Repositories.Interfaces;
//using CollegeGradingSys.ViewModels;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CollegeGradingSys.Services
//{
//    public class AcademicYearService
//    {
//        private readonly IAcademicYearRepository _repository;

//        public AcademicYearService(IAcademicYearRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<List<AcademicYearSelectItemViewModel>> GetAcademicYearsAsync(string placeholder)
//        {
//            var years = await _repository.ListAsync();

//            var result = years
//                .Select(y => new AcademicYearSelectItemViewModel
//                {
//                    Id = y.Id,
//                    Name = y.AcademicYearName
//                })
//                .ToList();

//            // إضافة خيار placeholder
//            result.Insert(0, new AcademicYearSelectItemViewModel
//            {
//                Id = -1,
//                Name = placeholder
//            });

//            return result;
//        }
//    }
//}
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AcademicYearService : IAcademicYearService
{
    private readonly IAcademicYearRepository _yearRepo;
    private readonly IStAcademicDataService _stAcademicRepo;
    private readonly IStPersonalDataService _stPersonalRepo;

    public AcademicYearService(
        IAcademicYearRepository yearRepo,
        IStAcademicDataService stAcademicRepo,
        IStPersonalDataService stPersonalRepo
        )
    {
        _yearRepo = yearRepo;
        _stAcademicRepo = stAcademicRepo;
        _stPersonalRepo = stPersonalRepo;
    }

    public async Task<AcademicYearIndexVM> GetIndexViewAsync()
    {
        var years = (await _yearRepo.ListAsync())
            .OrderByDescending(x => x.AcademicYearStart)
            .ToList();

        return new AcademicYearIndexVM
        {
            AcademicYears = years,
            IsCurrentYearClosed = await IsCurrentYearClosedAsync()
        };
    }

    public async Task<AcademicYearDetailsVM> GetDetailsAsync(int id)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null) return null;

        return new AcademicYearDetailsVM
        {
            Id = year.Id,
            AcademicYearName = year.AcademicYearName,
            AcademicYearStart = year.AcademicYearStart,
            AcademicYearEnd = year.AcademicYearEnd,
            IsClosed = year.IsClosed,
            IsCurrentYear = year.IsCurrentYear
        };
    }
    public async Task<List<AcademicYearSelectItemVM>> GetAcademicYearsAsync(string placeholder = "— اختر العام الدراسي —")
    {
        var years = await _yearRepo.ListAsync();
        var list = years
            .OrderByDescending(y => y.AcademicYearStart)
            .Select(y => new AcademicYearSelectItemVM { Id = y.Id, Name = y.AcademicYearName })
            .ToList();

        list.Insert(0, new AcademicYearSelectItemVM { Id = -1, Name = placeholder });
        return list;
    }
   


    public async Task<bool> IsAcademicYearNameExistsAsync(string name, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        var all = await _yearRepo.ListAsync();
        return all.Any(y => y.AcademicYearName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)
                            && (!excludeId.HasValue || y.Id != excludeId.Value));
    }

    public async Task<bool> IsCurrentYearClosedAsync()
    {
        var current = (await _yearRepo.ListAsync()).SingleOrDefault(x => x.IsCurrentYear);
        if (current == null) return true; // إذا ما فيه سنة حالية اعتبرها مغلقة
        return (current.IsClosed);
        //    var stAcademic = (await _stAcademicRepo.GetAllAsync()).Where(x => x.AcademicYear.Id == current.Id);
        //return !stAcademic.Any(x => x.StStatus == CollegeGradingSys.Models.Enums.StStatus.مقيد);
    }

    public async Task<AcademicYear?> CreateAsync(AcademicYear newYear)
    {
        // ❗ قاعدة: لا يمكن إنشاء سنة جديدة إذا الحالية غير مغلقة
        if (!await IsCurrentYearClosedAsync())
            throw new DomainException("لا يمكن إنشاء عام دراسي جديد قبل إغلاق العام الحالي.");

        // ❗ قاعدة: الاسم يجب أن يكون فريد
        if (await IsAcademicYearNameExistsAsync(newYear.AcademicYearName))
            throw new DomainException("يوجد عام دراسي بنفس الاسم.");

        // ❗ قاعدة: التاريخ
        if (newYear.AcademicYearStart >= newYear.AcademicYearEnd)
            throw new DomainException("تاريخ نهاية العام يجب أن يكون بعد تاريخ البداية.");

        // ❗ قاعدة: سنة واحدة فقط Current
        if (newYear.IsCurrentYear)
        {
            var prevCurrent = (await _yearRepo.ListAsync())
                .Where(x => x.IsCurrentYear)
                .ToList();

            foreach (var p in prevCurrent)
            {
                p.IsCurrentYear = false;
                await _yearRepo.UpdateAsync(p);
            }
        }

        return await _yearRepo.AddAsync(newYear);       
    }

    public async Task<AcademicYearEditVM> GetEditViewAsync(int id)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null)
            return null;

        if (year.IsClosed)
            throw new DomainException("لا يمكن تعديل سنة دراسية مغلقة.");

        return new AcademicYearEditVM
        {
            Id = year.Id,
            AcademicYearName = year.AcademicYearName,
            AcademicYearStart = year.AcademicYearStart,
            AcademicYearEnd = year.AcademicYearEnd,
            IsCurrentYear = year.IsCurrentYear,
            AcademicYearNameH = year.AcademicYearNameH,
            IsClosed = year.IsClosed,
            Note = year.Note
        };
    }


    public async Task UpdateAsync(int id, AcademicYearEditVM vm)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null)
            throw new DomainException("العام الدراسي غير موجود.");

        if (year.IsClosed)
            throw new DomainException("لا يمكن تعديل عام دراسي مغلق.");

        if (vm.AcademicYearStart >= vm.AcademicYearEnd)
            throw new DomainException("يجب أن يكون تاريخ نهاية العام بعد تاريخ البداية.");

        if (await IsAcademicYearNameExistsAsync(vm.AcademicYearName, id))
            throw new DomainException("يوجد عام آخر بنفس الاسم.");

        // ========= Apply Changes =========
        year.AcademicYearName = vm.AcademicYearName.Trim();
        year.AcademicYearNameH = vm.AcademicYearNameH?.Trim() ?? string.Empty;
        year.AcademicYearStart = vm.AcademicYearStart;
        year.AcademicYearEnd = vm.AcademicYearEnd;
        year.Note = vm.Note;

        if (vm.IsCurrentYear && !year.IsCurrentYear)
            await MakeCurrentYearAsync(year);

        await _yearRepo.UpdateAsync(year);       
    }

    public async Task<AcademicYearDeleteVM?> GetDeleteViewAsync(int id)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null)
            throw new DomainException("العام الدراسي غير موجود.");

        if (year.IsClosed)
            throw new DomainException("لا يمكن حذف عام دراسي مغلق.");

        if (year.IsCurrentYear)
            throw new DomainException("لا يمكن حذف العام الدراسي الحالي.");

        if (await HasStudentsAsync(year.Id))
            throw new DomainException("لا يمكن حذف عام يحتوي على بيانات طلاب.");

        return new AcademicYearDeleteVM
        {
            Id = year.Id,
            AcademicYearName = year.AcademicYearName,
            IsClosed = year.IsClosed
        };
    }

    public async Task DeleteAsync(int id)
    {
       
        // منع الحذف إن كانت هناك سجلات شخصية / أكاديمية مرتبطة
        var personalList = await _stPersonalRepo.GetAllAsync();
        var hasPersonal = personalList.Any(x => x.EnrollmentYear != null && x.EnrollmentYear.Id == id);
        if (hasPersonal)
            throw new DomainException("لا يمكن حذف العام الجامعي لأن هناك طلابًا مسجلين فيه. احذف الطلاب أولاً.");
       

        var academicList = await _stAcademicRepo.GetAllAsync();
        var hasAcademic = academicList.Any(x => x.AcademicYear != null && x.AcademicYear.Id == id);
        if (hasAcademic)
            throw new DomainException("لا يمكن حذف العام الجامعي بسبب وجود سجلات أكاديمية مرتبطة.");

        var deleted = await _yearRepo.DeleteAsync(id);
        if (deleted == null) throw new DomainException("لم يتم العثور على السنة المراد حذفها.");

        // بعد الحذف: اجعل آخر سنة متاحة هي الحالية إن وجدت
        var last = (await _yearRepo.ListAsync()).OrderByDescending(x => x.AcademicYearStart).FirstOrDefault();
        if (last != null)
        {
            last.IsCurrentYear = true;
            await _yearRepo.UpdateAsync(last);
        }
       
    }

    private async Task MakeCurrentYearAsync(AcademicYear year)
    {
        var currentYears = (await _yearRepo.ListAsync())
            .Where(x => x.IsCurrentYear && x.Id != year.Id)
            .ToList();

        foreach (var y in currentYears)
        {
            y.IsCurrentYear = false;
            await _yearRepo.UpdateAsync(y);
        }

        year.IsCurrentYear = true;
    }

    public async Task CloseYearAsync(int id)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null)
            throw new DomainException("العام الدراسي غير موجود.");

           var stAcademic = (await _stAcademicRepo.GetAllAsync()).Where(x => x.AcademicYear.Id == year.Id);
        if(!stAcademic.Any(x => x.StStatus == CollegeGradingSys.Models.Enums.StStatus.مقيد))
            throw new DomainException("لا يمكن إغلاق العام الدراسي لوجود طلاب ما زالوا مقيدين.");

        year.Close(); // Domain Rule
        year.IsCurrentYear = false;
        await _yearRepo.UpdateAsync(year);
    }

    public async Task ReOpenYearAsync(int id)
    {
        var year = await _yearRepo.FindAsync(id);
        if (year == null)
            throw new DomainException("العام الدراسي غير موجود.");

        if (!year.IsClosed)
            throw new DomainException("العام الدراسي غير مغلق.");

        if (await ExistsNewerYearAsync(year))
            throw new DomainException("لا يمكن إعادة فتح عام بعد إنشاء عام أحدث.");

        //if (await StudentsAlreadyPromoted(year.Id))
        //    throw new DomainException("لا يمكن إعادة فتح عام بعد ترقية الطلاب.");

        year.ReOpen();
        await _yearRepo.UpdateAsync(year);
    }

    private async Task<bool> ExistsNewerYearAsync(AcademicYear year)
    {
        return await _yearRepo.Query()
            .AnyAsync(x =>
                x.Id != year.Id &&
                x.AcademicYearStart > year.AcademicYearStart
            );
    }

    private async Task<bool> HasStudentsAsync(int id)
    {
        return (await  _stPersonalRepo.GetAllAsync()).Any(x => x.EnrollmentYear != null && x.EnrollmentYear.Id == id);
         
    }


}
