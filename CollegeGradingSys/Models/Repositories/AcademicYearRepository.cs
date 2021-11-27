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
                new AcademicYear { Id= 1, AcademicYearStart = new DateTime(2021, 8, 1), AcademicYearEnd = new DateTime(2022, 4, 20) , AcademicYearName ="2021-2022"},
                 new AcademicYear { Id= 2, AcademicYearStart = new DateTime(2022, 8, 1), AcademicYearEnd = new DateTime(2023, 4, 20) , AcademicYearName ="2022-2023"}
            };
        }
        public AcademicYear Add(AcademicYear entity)
        {
            var college = academicYears.FirstOrDefault();
            entity.Id = college != null ? academicYears.Max(b => b.Id) + 1 : 1;            
            academicYears.Add(entity);
            return entity;
        }

        public AcademicYear Delete(int id)
        {
            AcademicYear academicYear = Find(id);
            if (academicYear != null)
            {
                academicYears.Remove(academicYear);
            }
            return academicYear;
        }

        public AcademicYear Find(int id)
        {
            return academicYears.SingleOrDefault(a => a.Id == id);
        }

        public IList<AcademicYear> List()
        {
            return academicYears;
        }

        public AcademicYear Update(int id, AcademicYear newAcademicYear)
        {
            var oldAcademicYear = Find(id);
            if (oldAcademicYear != null)
            {
                oldAcademicYear.AcademicYearStart = newAcademicYear.AcademicYearStart;
                oldAcademicYear.AcademicYearEnd = newAcademicYear.AcademicYearEnd;
                oldAcademicYear.AcademicYearName = newAcademicYear.AcademicYearName;
                oldAcademicYear.Note = newAcademicYear.Note;
            }
            return newAcademicYear;
        }
    }
}
