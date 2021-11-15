using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class AcademicYearRepository : ICollegeGradingSysRepository<AcademicYear>
    {
        IList<AcademicYear> academicYears;
        public AcademicYearRepository()
        {
            academicYears = new List<AcademicYear>()
            {
                new AcademicYear { Id= 1, AcademicYearStart = new DateTime(2021, 8, 1), AcademicYearEnd = new DateTime(2022, 4, 20) , AcademicYearName ="2021-2022"}
            };
        }
        public void Add(AcademicYear entity)
        {
            entity.Id = academicYears.Max(a => a.Id) + 1;
            academicYears.Add(entity);
        }

        public void Delete(int id)
        {
            var academicYear = Find(id);
            academicYears.Remove(academicYear);
        }

        public AcademicYear Find(int id)
        {
            return academicYears.SingleOrDefault(a => a.Id == id);
        }

        public IList<AcademicYear> List()
        {
            return academicYears;
        }

        public void Update(int id, AcademicYear newAcademicYear)
        {
            var oldAcademicYear = Find(id);
            oldAcademicYear.AcademicYearStart = newAcademicYear.AcademicYearStart;
            oldAcademicYear.AcademicYearEnd = newAcademicYear.AcademicYearEnd;
            oldAcademicYear.AcademicYearName = newAcademicYear.AcademicYearName;
        }
    }
}
