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
        public Specialization Add(Specialization entity)
        {
            var specialization = specializations.FirstOrDefault();
            entity.Id = specialization != null ? specializations.Max(b => b.Id) + 1 : 1;            
            specializations.Add(entity);
            return entity;
        }

        public Specialization Delete(int id)
        {
            var specialization = Find(id);
            if (specialization != null)
            {
                specializations.Remove(specialization);
            }
            return specialization;
        }

        public Specialization Find(int id)
        {
           return specializations.SingleOrDefault(a => a.Id == id);
        }

        public IList<Specialization> List()
        {
            return specializations;
        }

        public Specialization Update(int id, Specialization newSpec)
        {
           var oldspec = Find(id);
            if (oldspec != null)
            {
                oldspec.SpecializationName = newSpec.SpecializationName;
                oldspec.Department = newSpec.Department;
            }
            return newSpec;
        }
    }
}
