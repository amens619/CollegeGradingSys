using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CollegeRepository : ICollegeGradingSysRepository<College>
    {
        IList<College> colleges;
        public CollegeRepository()
        {
            colleges = new List<College>()
            {
                new College{  Id =1,CollegeName="الشريعة"}
            };
        }

        public void Add(College entity)
        {
            var college = colleges.FirstOrDefault();

            entity.Id = college != null ? colleges.Max(b => b.Id) + 1 : 1;


            colleges.Add(entity);
        }

        public void Delete(int id)
        {
            var college = Find(id);
            colleges.Remove(college);
        }

        public College Find(int id)
        {
            return colleges.SingleOrDefault(a => a.Id == id);
        }

        public IList<College> List()
        {
            return colleges;
        }

        public void Update(int id, College newAuthor)
        {
            var oldCollege = Find(id);
            oldCollege.CollegeName = newAuthor.CollegeName;
        }
    }
}
