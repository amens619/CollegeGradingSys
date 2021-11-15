using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class SubCourseRepository : ICollegeGradingSysRepository<SubCourse>
    {
        IList<SubCourse> subCourses;
        public SubCourseRepository()
        {
            subCourses = new List<SubCourse>()
            {
                new SubCourse { Id=1 , }
            };
        }
        public void Add(SubCourse entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SubCourse Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<SubCourse> List()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, SubCourse entity)
        {
            throw new NotImplementedException();
        }
    }
}
