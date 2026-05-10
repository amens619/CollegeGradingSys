using AutoMapper;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class StPersonalDataService : GenericService<StPersonalData>, IStPersonalDataService
    {
        private readonly IStPersonalDataRepository _studentRepo;
        private readonly IStAcademicDataRepository _academicDataRepo;
        private readonly IStHighSchoolDataRepository _highSchoolRepo;
        private readonly ICourseGradeRepository _courseGradeRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IRepository<Batch> _batchRepo;
        private readonly IMapper _mapper;
        public StPersonalDataService(
             IStPersonalDataRepository studentRepo,
             IStAcademicDataRepository academicDataRepo,
             IStHighSchoolDataRepository highSchoolRepo,
             ICourseGradeRepository courseGradeRepo,
             ICourseRepository courseRepo,
             IRepository<Batch> batchRepo)
             : base(studentRepo)
        {
            _studentRepo = studentRepo;
            _academicDataRepo = academicDataRepo;
            _highSchoolRepo = highSchoolRepo;
            _courseGradeRepo = courseGradeRepo;
            _courseRepo = courseRepo;
            _batchRepo = batchRepo;
        }

        public Task<StPersonalData> GetFullAsync(int academicId) => _studentRepo.FindFullAsync(academicId);

        public Task<IList<StPersonalData>> GetAllFullAsync() => _studentRepo.ListFullAsync();
        public async Task<IEnumerable<StPersonalData>> GetByEnrollmentYearAsync(int enrollmentYearId)
        {
            var allStudents = await GetAllFullAsync();
            return allStudents.Where(x => x.EnrollmentYear?.Id == enrollmentYearId);
        }

        public async Task EnsureCountryCanBeDeletedAsync(int countryId)
        {
            if (await _studentRepo.Query().AnyAsync(x => x.Nationality.Id == countryId || x.Birthcountry.Id == countryId))
                throw new DomainException("لا يمكن حذف الدولة بسبب وجود بيانات طلاب تابعة لها");
        }

        public async Task<bool> IsAcademicIdAvailableAsync(int academicId)
        {
            return !await _studentRepo.ExistsAsync(academicId);
        }


        // ✨ نقلنا منطق الكنترولر المعقد إلى هنا!
        public async Task RegisterNewStudentAsync(StPersonalData student, int batchId)
        {
            // 1. إضافة بيانات الطالب الأساسية
            await _studentRepo.AddAsync(student);

            // 2. جلب الدفعة المرتبطة
            var batch = await _batchRepo.FindAsync(batchId);

            // 3. إنشاء السجل الأكاديمي (الترم الأول) ومواده
            var term1 = CreateAcademicRecord(student, batch, Term.الأول);
            await _academicDataRepo.AddAsync(term1);
            await GenerateCourseGradesAsync(term1);

            // 4. إنشاء السجل الأكاديمي (الترم الثاني) ومواده
            var term2 = CreateAcademicRecord(student, batch, Term.الثاني);
            await _academicDataRepo.AddAsync(term2);
            await GenerateCourseGradesAsync(term2);
        }
        // تخصيص عملية الحذف (Override) لتطبيق الـ Cascade Delete يدوياً
        public override async Task<bool> DeleteAsync(int academicID)
        {
            var student = await GetFullAsync(academicID);
            if (student == null) return false;

            // 1. حذف السجلات الأكاديمية التابعة (Terms)
            if (student.StAcademicDatas != null && student.StAcademicDatas.Any())
            {
                // نستخدم ToList() لتجنب خطأ (Collection was modified) أثناء الحذف
                foreach (var record in student.StAcademicDatas.ToList())
                {
                    // 1. حذف جميع الدرجات (CourseGrades) المرتبطة بهذا السجل الأكاديمي أولاً
                    if (record.CourseGrades != null && record.CourseGrades.Any())
                    {
                        foreach (var grade in record.CourseGrades.ToList())
                        {
                            await _courseGradeRepo.DeleteAsync(grade.Id);
                        }
                    }

                    // 2. بعد تنظيف الدرجات، نقوم بحذف السجل الأكاديمي نفسه بأمان
                    await _academicDataRepo.DeleteAsync(record.Id);
                }
            }

            // 2. حذف بيانات الثانوية
            if (student.StHighSchoolData != null)
            {

                await _highSchoolRepo.DeleteAsync(academicID);
            }

            // 3. حذف سجل الطالب الأساسي
            return await base.DeleteAsync(academicID);
        }
        // --- دوال مساعدة (Private Helpers) لترتيب الكود ---
        private StAcademicData CreateAcademicRecord(StPersonalData student, Batch batch, Term term)
        {
            return new StAcademicData
            {
                StLevel = Level.الأول,
                Term = term,
                StStatus = StStatus.مقيد,
                Valuation = Valuation.غير_محدد,
                IsTerm = true,
                StudyType = StudyType.انتظام,
                StPersonalData = student,
                AcademicYear = student.EnrollmentYear,
                Batch = batch
            };
        }

        private async Task GenerateCourseGradesAsync(StAcademicData stAcademicData)
        {
            var courses = await _courseRepo.Query()
                .Where(x => x.Level == stAcademicData.StLevel &&
                            x.Term == stAcademicData.Term &&
                            x.Specialization == stAcademicData.Batch.Specialization)
                .ToListAsync();

            foreach (var course in courses)
            {
                var grade = new CourseGrade
                {
                    Course = course,
                    CourseType = true,
                    StAcademicData = stAcademicData,
                    StStatusForCourse = StStatusForCourse.غير_محدد
                };
                await _courseGradeRepo.AddAsync(grade);
            }
        }
    }
}
