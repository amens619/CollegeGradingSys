using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StAcademicDataDbRepository : ICollegeGradingSysRepository<StAcademicData>
    {
        private readonly ApplicationDbContext db;

        public StAcademicDataDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public StAcademicData Add(StAcademicData entity)
        {
            db.StAcademicData.Add(entity);
            SaveChange();
            return entity;
        }

        public StAcademicData Delete(int id)
        {
            var stAcademicData = Find(id);
            if (stAcademicData != null)
            {
                db.StAcademicData.Remove(stAcademicData);
                SaveChange();
            }
            return stAcademicData;
        }

        public StAcademicData Find(int id)
        {
            return db.StAcademicData
                 .Include(x => x.Batch)
                    .ThenInclude(y => y.Specialization)
                        .ThenInclude(z => z.Department)
                            .ThenInclude(a => a.College)
                .Include(x => x.AcademicYear)                
                .Include(x => x.StPersonalData)
                  .ThenInclude(y => y.Nationality)

                .Include(x => x.CourseGrades)
                    .ThenInclude(y => y.Course)
                .SingleOrDefault(a => a.Id == id);
        }

        public IList<StAcademicData> List()
        {
            return db.StAcademicData
                .Include(x => x.Batch)
                    .ThenInclude(y => y.Specialization)
                        .ThenInclude(z => z.Department)
                .Include(x => x.AcademicYear)                    
                .Include(x => x.StPersonalData)
                .Include(x => x.CourseGrades)
                    .ThenInclude(y => y.Course)
                .ToList();
        }

        public StAcademicData Update(int id, StAcademicData newStAcademicData)
        {
            var local = db.Set<StAcademicData>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                db.Entry(local).State = EntityState.Detached;
            }
                     
            var college = db.StAcademicData.Attach(newStAcademicData);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newStAcademicData;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }


    //    public static void DetachLocal<T>(this DbContext context, T t, string entryId)
    //where T : class, IIdentifier
    //    {
    //        var local = context.Set<T>()
    //            .Local
    //            .FirstOrDefault(entry => entry.Id.Equals(entryId));
    //        if (!local.IsNull())
    //        {
    //            context.Entry(local).State = EntityState.Detached;
    //        }
    //        context.Entry(t).State = EntityState.Modified;
    //    }
    }
}

    






