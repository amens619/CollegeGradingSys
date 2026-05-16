using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.AcademicYear;
using CollegeGradingSys.ViewModels.StAcademic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml; // EPPlus for Excel
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CreateStAcademicDataDataViewModel = CollegeGradingSys.ViewModels.StAcademic.CreateStAcademicDataDataViewModel;



namespace CollegeGradingSys.Services.Implementations
{
    public class StAcademicDataService : IStAcademicDataService
    {
        private readonly IStAcademicDataRepository _stAcademicDataRepo;
        private readonly IAcademicYearService _academicYearService;
        private readonly IBatchService _batchService;
        private readonly IStPersonalDataRepository _stPersonalRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly ICourseGradeRepository _courseGradeRepo;
        private readonly IRepository<GeneralInfo> _generalInfoRepo;

        public StAcademicDataService(
            IStAcademicDataRepository stAcademicDataRepo,
            IAcademicYearService academicYearService,
            IBatchService batchService,
            IStPersonalDataRepository stPersonalRepo,
            IRepository<Course> courseRepo,
            ICourseGradeRepository courseGradeRepo,
            IRepository<GeneralInfo> generalInfoRepo)
        {
            _stAcademicDataRepo = stAcademicDataRepo;
            _academicYearService = academicYearService;
            _batchService = batchService;
            _stPersonalRepo = stPersonalRepo;
            _courseRepo = courseRepo;
            _courseGradeRepo = courseGradeRepo;
            _generalInfoRepo = generalInfoRepo;
        }

        // ==================== Create Logic ====================
        public async Task<CreateStAcademicDataDataViewModel> GetCreateFormAsync(int academicId)
        {
            var student = await _stPersonalRepo.FindAsync(academicId)
                          ?? throw new DomainException("بيانات الطالب غير موجودة");

            var currentYear = await _academicYearService.GetCurrentYearAsync();

            // جلب آخر سجل أكاديمي لتعبئة البيانات السابقة
            var lastRecord = await _stAcademicDataRepo.Query()
                .Where(x => x.StPersonalData.AcademicID == academicId)
                .OrderByDescending(x => x.Id) // افتراض أن ID يزداد مع الزمن
                .FirstOrDefaultAsync();

            var model = new CreateStAcademicDataDataViewModel
            {
                AcademicID = student.AcademicID,
                StName = student.StName,
                AcademicYearId = currentYear.Id,
                Term = Term.الأول,
                StStatus = StStatus.مقيد
            };

            // تعبئة البيانات السابقة (Pre-Data)
            if (lastRecord != null)
            {
                model.preAcademicYear = lastRecord.AcademicYear.AcademicYearName;
                model.preStStatus = lastRecord.StStatus.ToString();
                model.preLevel = lastRecord.StLevel.ToString();
                model.preGPA = lastRecord.GPA?.ToString("N2");
                model.preValuation = lastRecord.Valuation.ToString();
                model.preSpecialization = lastRecord.Batch?.Specialization?.SpecializationName;

                // اختيار الدفعة تلقائياً إذا كان ناجحاً
                model.BatchId = lastRecord.StStatus == StStatus.ناجح ? lastRecord.Batch.Id : -1;
            }

            await FillListsForModelAsync(model);
            return model;
        }

        public async Task CreateAsync(CreateStAcademicDataDataViewModel model)
        {
            if (model.BatchId == -1)
                throw new DomainException("الرجاء اختيار الدفعة من القائمة");

            var student = await _stPersonalRepo.FindAsync(model.AcademicID);
            var batch = await _batchService.GetByIdAsync(model.BatchId);
            var academicYear = await _academicYearService.GetByIdAsync(model.AcademicYearId);

            // إضافة الفصل الأول
            var term1 = CreateEntity(model, student, batch, academicYear, Term.الأول);
            await _stAcademicDataRepo.AddAsync(term1);
            await InitializeCourseGradesAsync(term1);

            // إضافة الفصل الثاني
            var term2 = CreateEntity(model, student, batch, academicYear, Term.الثاني);
            await _stAcademicDataRepo.AddAsync(term2);
            await InitializeCourseGradesAsync(term2);
        }

        // ==================== Edit Logic ====================
        public async Task<CreateStAcademicDataDataViewModel> GetEditFormAsync(int id)
        {
            var entity = await _stAcademicDataRepo.FindAsync(id)
                         ?? throw new DomainException("السجل غير موجود");

            var model = new CreateStAcademicDataDataViewModel
            {
                Id = entity.Id,
                AcademicID = entity.StPersonalData.AcademicID,
                StName = entity.StPersonalData.StName,
                StLevel = entity.StLevel,
                Term = entity.Term,
                StStatus = entity.StStatus,
                StudyType = entity.StudyType,
                AcademicYearId = entity.AcademicYear.Id,
                BatchId = entity.Batch.Id,
                Valuation = entity.Valuation,
                Average = entity.Average?.ToString("N2", CultureInfo.InvariantCulture),
                GPA = entity.GPA?.ToString("N2", CultureInfo.InvariantCulture)
            };

            await FillListsForModelAsync(model);
            return model;
        }

        public async Task EditAsync(CreateStAcademicDataDataViewModel model)
        {
            var entity = await _stAcademicDataRepo.FindAsync(model.Id);
            if (entity == null) throw new DomainException("السجل غير موجود");

            // قواعد العمل (Business Rules)
            if (model.StStatus == StStatus.متخرج)
            {
                if (model.StLevel != Level.الرابع || model.Term != Term.الثاني)
                    throw new DomainException("يمكن اختيار 'متخرج' فقط في المستوى الرابع - الفصل الثاني.");

                if (!await IsAllCoursesPassedAsync(model.AcademicID))
                    throw new DomainException("لا يمكن التخرج، يوجد مواد رسوب.");
            }

            if (model.StStatus == StStatus.ناجح || model.StStatus == StStatus.متخرج)
            {
                if (!await IsCourseGradesEnteredAsync(model.Id))
                    throw new DomainException("يجب إدخال جميع الدرجات قبل الحفظ.");

                if (!float.TryParse(model.Average, NumberStyles.Any, CultureInfo.InvariantCulture, out float avg) || avg < 0 || avg > 100)
                    throw new DomainException("المعدل يجب أن يكون رقماً بين 0 و 100");

                if (!float.TryParse(model.GPA, NumberStyles.Any, CultureInfo.InvariantCulture, out float gpa) || gpa < 0 || gpa > 100)
                    throw new DomainException("المعدل التراكمي يجب أن يكون رقماً بين 0 و 100");

                if (model.Valuation == Valuation.غير_محدد)
                    throw new DomainException("يجب تحديد التقدير.");

                entity.Average = avg;
                entity.GPA = gpa;
                entity.Valuation = model.Valuation;
            }

            // تحديث القيم
            entity.StudyType = model.StudyType;
            entity.StStatus = model.StStatus;

            // إذا نجح، أضف مواد الرسوب كمواد تكميلية
            if (model.StStatus == StStatus.ناجح)
            {
                await AddSupplementaryCoursesAsync(entity, model.AcademicID);
            }

            await _stAcademicDataRepo.UpdateAsync(entity);
        }

        // ==================== Helpers & Logic ====================
        public async Task FillListsForModelAsync(CreateStAcademicDataDataViewModel model)
        {
            var years = await _academicYearService.GetSelectItemsAsync(x => new SelectItemVM { Id = x.Id, Name = x.AcademicYearName });
            var batches = await _batchService.GetSelectItemsAsync(x => new SelectItemVM { Id = x.Id, Name = x.BatchName });

            model.AcademicYearsList = new SelectList(years, "Id", "Name", model.AcademicYearId);
            model.BatchesList = new SelectList(batches, "Id", "Name", model.BatchId);
        }

        private StAcademicData CreateEntity(CreateStAcademicDataDataViewModel model, StPersonalData student, Batch batch, AcademicYear year, Term term)
        {
            return new StAcademicData
            {
                StLevel = model.StLevel,
                Term = term,
                StStatus = StStatus.مقيد,
                Valuation = Valuation.غير_محدد,
                IsTerm = true,
                StudyType = model.StudyType,
                StPersonalData = student,
                AcademicYear = year,
                Batch = batch
            };
        }

        private async Task InitializeCourseGradesAsync(StAcademicData stData)
        {
            // منطق جلب المواد حسب الجنس والمستوى
            var courses = await _courseRepo.Query()
               .Where(x => x.Level == stData.StLevel && x.Term == stData.Term && x.Specialization.Id == stData.Batch.Specialization.Id)
               .ToListAsync();

            var finalCourses = courses.Where(c => c.Course_sGender == Course_sGender.كلاالجنسين ||
                                                 (stData.StPersonalData.Sex == Sex.ذكر && c.Course_sGender == Course_sGender.ذكور) ||
                                                 (stData.StPersonalData.Sex == Sex.انثى && c.Course_sGender == Course_sGender.اناث));

            foreach (var c in finalCourses)
            {
                await _courseGradeRepo.AddAsync(new CourseGrade
                {
                    Course = c,
                    CourseType = true,
                    StAcademicData = stData,
                    StStatusForCourse = StStatusForCourse.غير_محدد
                });
            }
        }

        private async Task<bool> IsCourseGradesEnteredAsync(int stAcademicDataId)
        {
            var pendingGrades = await _courseGradeRepo.Query()
               .Where(x => x.StAcademicData.Id == stAcademicDataId && x.CourseType == true && x.StStatusForCourse == StStatusForCourse.غير_محدد)
               .AnyAsync();
            return !pendingGrades;
        }

       
        private async Task<bool> IsAllCoursesPassedAsync(int academicId)
        {
            if (!await _stAcademicDataRepo.Query().AnyAsync(x => x.StPersonalData.AcademicID == academicId))
                return false;
            // استعلام واحد فعال يتم ترجمته إلى SQL
            // قمنا بإضافة Select قبل AllAsync لضمان سهولة الترجمة في EF Core
            var allCoursesPassed = await _stAcademicDataRepo.Query()
                .Where(x => x.StPersonalData.AcademicID == academicId)
                .SelectMany(x => x.CourseGrades)
                .Where(g => g.CourseType == false) // المواد الأساسية فقط
                .GroupBy(g => g.Course.Id)          // التجميع حسب معرف المادة الدراسي
                .Select(group => group.Any(g => g.StStatusForCourse == StStatusForCourse.ناجح))
                .AllAsync(isPassed => isPassed);

            return allCoursesPassed;
        }

       
        private async Task AddSupplementaryCoursesAsync(StAcademicData currentEntity, int academicId)
        {
            // 1. جلب كافة المواد التي رسب فيها الطالب في تاريخه الأكاديمي ولم ينجح فيها حتى الآن
            var failedCourses = await _stAcademicDataRepo.Query()
                .Where(x => x.StPersonalData.AcademicID == academicId)
                .SelectMany(x => x.CourseGrades)
                .Where(g => g.CourseType == false) // المواد الأساسية
                .GroupBy(g => g.Course.Id)
                // الفلترة: نأخذ المواد التي لا يوجد لها أي سجل "ناجح"
                .Where(group => !group.Any(g => g.StStatusForCourse == StStatusForCourse.ناجح))
                .Select(group => group.OrderByDescending(g => g.Id).FirstOrDefault().Course)
                .ToListAsync();

            if (!failedCourses.Any()) return;

            // 2. إضافة هذه المواد إلى السجل الحالي (currentEntity) كمواد تكميلية/معادة
            foreach (var course in failedCourses)
            {
                // نتحقق أولاً أن المادة ليست مضافة مسبقاً في السجل الحالي لتجنب التكرار
                if (!currentEntity.CourseGrades.Any(g => g.Course.Id == course.Id))
                {
                    var newSupplementaryGrade = new CourseGrade
                    {
                        Course = course,
                        CourseType = false, // مادة أساسية لكنها معادة
                        StAcademicData = currentEntity,
                        StStatusForCourse = StStatusForCourse.غير_محدد,
                        // يمكنك هنا إضافة ملاحظة أو وسم بأنها مادة محملة من مستويات سابقة
                    };

                    await _courseGradeRepo.AddAsync(newSupplementaryGrade);
                }
            }
        }
        // ==================== Other Methods (Index, History, Delete) ====================
    
        public async Task<StAcademicDataUnifiedVM> GetIndexDataAsync(StAcademicDataUnifiedVM filter, int pageNumber, int pageSize)
        {
            // 1. التحقق من سنة الالتحاق الحالية
            if (filter.IsSelectCurrentYear)
            {
                var current = await _academicYearService.GetCurrentYearAsync();
                if (current != null)
                {
                    filter.AcademicYearId = current.Id;
                    filter.IsCurrentYear = true;
                }
            }

            // 2. جلب الاستعلام (Query) من المستودع بدون تنفيذه (لاحظ: بدون await)
            var query = _stAcademicDataRepo.GetFilteredQuery(
                filter.AcademicYearId,
                filter.BatchId,
                filter.StNameSearch,
                filter.StStatus,
                filter.Term,
                filter.StudyType);

            // 3. حساب العدد الإجمالي للطلاب المتوافقين مع البحث
            int totalCount = await query.CountAsync();

            // 4. تقطيع البيانات وجلبها من SQL Server (هنا يحدث التنفيذ الفعلي)
            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize) // استخدمنا البارامترات الممررة للدالة
                .Take(pageSize)
                .ToListAsync();

            // 5. تحويل البيانات (Mapping)
            var mappedData = data.Select(x => new StAcademicDataVM
            {
                Id = x.Id,
                StPersonalData = x.StPersonalData,
                AcademicYear = x.AcademicYear,
                Batch = x.Batch,
                StLevel = x.StLevel,
                StStatus = x.StStatus,
                Term = x.Term,
                IsCurrentYear = x.IsTerm,
                Average = x.Average,
                GPA = x.GPA,
                Valuation = x.Valuation
            }).ToList();

            filter.AcademicYearsList = new SelectList(await FillSelectAcademicYearesList("-- الكل --"), "Id", "Name", filter.AcademicYearId ?? -1);
            filter.BatchesList = new SelectList(await FillSelectBatchsList("-- الكل --"), "Id", "BatchName", -1);
            filter.StAcademicDataVMs = mappedData;

            // 6. 💡 بناء كائن التقسيم (PagedResult) لكي تظهر أزرار الصفحات في الـ View
            filter.pagedResult = new PagedResult<StAcademicDataVM>
            {
                Data = mappedData,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            // 7. إرجاع الـ ViewModel الجاهز تماماً
            return filter;
        }
        public async Task<StAcademicDataDataViewModel> GetStudentAcademicHistoryAsync(int academicID)
        {
            var student = await _stPersonalRepo.FindAsync(academicID);
            if (student == null) throw new DomainException("الطالب غير موجود");

            var history = await _stAcademicDataRepo.Query()
                .Where(x => x.StPersonalData.AcademicID == academicID)
                .OrderBy(x => x.AcademicYear.AcademicYearStart.Year)
                .ThenBy(x => x.StLevel).ThenBy(x => x.Term)
                .ToListAsync();

            var currentYear = await _academicYearService.GetCurrentYearAsync();
            bool canRegister = !history.Any(x => x.AcademicYear.Id == currentYear.Id)
                               && !history.Any(x => x.StStatus == StStatus.منحسب);
            
            return new StAcademicDataDataViewModel
            {
                Id = academicID,
                AcademicID = student.AcademicID,
                StName = student.StName,
                IsCanRegisterInCurrentYear = canRegister,
                StAcademicDatas = history
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _stAcademicDataRepo.DeleteAsync(id);
        }

        public async Task PromoteAllStudentsToNextYearAsync()
        {
            // منطق الترحيل الجماعي (كما تم شرحه سابقاً)
            throw new NotImplementedException("يجب تطبيق منطق الترحيل هنا");
        }

        // ==================== Printing & Exporting ====================
        public async Task<PrintConfEnrollVM> GetPrintConfEnrollAsync(int id)
        {
            var entity = await _stAcademicDataRepo.FindAsync(id) ?? throw new DomainException("السجل غير موجود");
            var general = _generalInfoRepo.Query().FirstOrDefault();
            var currentYear = await _academicYearService.GetCurrentYearAsync();

            return new PrintConfEnrollVM
            {
                StName = entity.StPersonalData.StName,
                AcademicID = entity.StPersonalData.AcademicID.ToString(),
                AcademicYearName = currentYear.AcademicYearName,
                StDepartmentHead = general?.StDepartmentHead,
                StLevel = entity.StLevel.ToString(),
                Nationality = entity.StPersonalData.Nationality?.NationalityName,
                PrintConfEnrollDate = DateTime.Now.ToString("yyyy/MM/dd")
            };
        }

        public Task<PrintConfEnrollVM> GetPrintGradeReportAsync(int id) => GetPrintConfEnrollAsync(id);
        public async Task<PrintGraduatStatementVM> GetPrintGraduatStatementAsync(int id)
        {
            var entity = await _stAcademicDataRepo.FindAsync(id) ?? throw new DomainException("السجل غير موجود");
            var general = _generalInfoRepo.Query().FirstOrDefault();
            var currentYear = await _academicYearService.GetCurrentYearAsync();

            return new PrintGraduatStatementVM
            {
                StName = entity.StPersonalData.StName,
                AcademicID = entity.StPersonalData.AcademicID.ToString(),
                AcademicYearName = currentYear.AcademicYearName,
                StDepartmentHead = general?.StDepartmentHead,
                StLevel = entity.StLevel.ToString(),
                Nationality = entity.StPersonalData.Nationality?.NationalityName,
                 
                PrintConfEnrollDate = DateTime.Now.ToString("yyyy/MM/dd")
            };
        }
        public Task<PrintAlmushayakhaStatementVM> GetPrintAlmushayakhaStatementAsync(int id) => throw new NotImplementedException();

        public async Task<MemoryStream> ExportStAcademicDataToExcelAsync(StAcademicDataFilterVM filter)
        {
            // منطق EPPlus للتصدير
            return new MemoryStream(); // Placeholder
        }
        public async Task<StudentGradesFilterVM> GetStudentGradesReportAsync(int academicId, int? level, int? term, int? yearId)
        {
           

            var student = await _stPersonalRepo.FindFullAsync(academicId);
            if (student == null) return null;

            // 2. تطبيق الفلاتر (التصفية) على السجلات الأكاديمية
            var query = student.StAcademicDatas.AsQueryable();

            if (level.HasValue)
                query = query.Where(a => (int)a.StLevel == level.Value);

            if (term.HasValue)
                query = query.Where(a => (int)a.Term == term.Value);

            if (yearId.HasValue)
                query = query.Where(a => a.AcademicYear != null && a.AcademicYear.Id == yearId.Value);

            // 3. جلب جميع السنوات الأكاديمية للقائمة المنسدلة من قاعدة البيانات
            var allYears = await _academicYearService.GetAllAsync();

            // 4. تعبئة الـ ViewModel
            var viewModel = new StudentGradesFilterVM
            {
                AcademicID = student.AcademicID,
                StudentName = student.StName,
                SelectedLevel = level,
                SelectedTerm = term,
                SelectedYearId = yearId,

                // 💡 دمج درجات السجلات المفلترة في قائمة واحدة مع الحماية من الـ Null
                Grades = query.SelectMany(a => a.CourseGrades ?? new List<CourseGrade>()).ToList(),

                // تعبئة قوائم الفلاتر (Dropdowns)
                Levels = Enum.GetValues(typeof(Level)).Cast<Level>()
                    .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() }).ToList(),

                Terms = Enum.GetValues(typeof(Term)).Cast<Term>()
                    .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() }).ToList(),

                AcademicYears = allYears
                    .Select(y => new SelectListItem { Value = y.Id.ToString(), Text = y.AcademicYearName }).ToList()
            };

            return viewModel;
        }
        public Task<MemoryStream> ExportGraduateStToExcelAsync(StAcademicDataFilterVM filter) => ExportStAcademicDataToExcelAsync(filter);

        public async Task<SingleTermGradesVM> GetSingleTermGradesAsync(int stAcademicDataId)
        {
            // جلب السجل الأكاديمي المحدد مع الطالب والدرجات والمواد التابعة له
            var academicData = await _stAcademicDataRepo.FindAsync(stAcademicDataId);
               

            if (academicData == null) return null;

            return new SingleTermGradesVM
            {
                StAcademicDataId = academicData.Id,
                AcademicID = academicData.StPersonalData.AcademicID,
                StudentName = academicData.StPersonalData.StName,
                Level = academicData.StLevel,
                Term = academicData.Term ?? Term.الأول, // التعامل مع القيم المجهولة بأمان
                AcademicYearName = academicData.AcademicYear?.AcademicYearName,

                // 💡 تحديد إذا كانت السنة مفتوحة بناءً على خاصية IsCurrentYear التي أعددتها سابقاً
                IsYearOpen = academicData.AcademicYear?.IsCurrentYear ?? false,

                Grades = academicData.CourseGrades?.ToList() ?? new List<CourseGrade>()
            };
        }

        private async Task<List<AcademicYearSelectItemVM>> FillSelectAcademicYearesList(string placeholder)
        {
            var years = (await _academicYearService.GetAllAsync())
                .Select(x => new AcademicYearSelectItemVM { Id = x.Id, Name = x.AcademicYearName })
                .ToList();
            years.Insert(0, new AcademicYearSelectItemVM { Id = -1, Name = placeholder });
            return years;
        }

        private async Task<List<Batch>> FillSelectBatchsList(string placeholder)
        {
            var list = (await _batchService.GetAllAsync()).OrderByDescending(x => x.Id).ToList();
            list.Insert(0, new Batch { Id = -1, BatchName = placeholder });
            return list;
        }

    }
}