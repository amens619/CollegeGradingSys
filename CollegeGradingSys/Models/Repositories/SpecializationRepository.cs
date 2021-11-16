using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class SpecializationRepository : ICollegeGradingSysRepository<Specialization>
    {
        IList<Specialization> specializations;
        public SpecializationRepository()
        {
            specializations = new List<Specialization>()
            {
                new  Specialization{ Id=1, SpecializationName="عام", Department = new Department{ Id=1, DepartmentName="فقه" }  }
            };
        }
        public void Add(Specialization entity)
        {
            entity.Id = specializations.Max(a => a.Id) + 1; 
            specializations.Add(entity);
        }

        public void Delete(int id)
        {
            var specialization = Find(id);
            specializations.Remove(specialization);
        }

        public Specialization Find(int id)
        {
           return specializations.SingleOrDefault(a => a.Id == id);
        }

        public IList<Specialization> List()
        {
            return specializations;
        }

        public void Update(int id, Specialization newSpec)
        {
           var oldspec = Find(id);
            oldspec.SpecializationName = newSpec.SpecializationName;
            oldspec.Department = newSpec.Department;
        }
    }
}
