using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
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

        public College Add(College entity)
        {
            var college = colleges.FirstOrDefault();
            entity.Id = college != null ? colleges.Max(b => b.Id) + 1 : 1;
            colleges.Add(entity);
            return entity;
        }

        public College Delete(int id)
        {
            College college = Find(id);
            if( college != null)
            {
                colleges.Remove(college);
            }

            return college;
        }

        public College Find(int id)
        {
            return colleges.SingleOrDefault(a => a.Id == id);
        }

        public IList<College> List()
        {
            return colleges;
        }

        public College Update(int id, College newAuthor)
        {
            var oldCollege = Find(id);
            if (oldCollege != null)
            {
                oldCollege.CollegeName = newAuthor.CollegeName;
            }
            return newAuthor;
        }
    }
}
