using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class DepartmentRepository : ICollegeGradingSysRepository<Department>
    {
        IList<Department> departments;
        public DepartmentRepository()
        {
            departments = new List<Department>()
            {
                new Department{ Id=1, DepartmentName="فقه",  Specializations=new List<Specialization>(){ new Specialization{  Id=1 } }  },
                 new Department{ Id=2, DepartmentName="علوم القرآن", Specializations=new List<Specialization>(){ new Specialization{  Id=1 } }  },
                 new Department{ Id=3, DepartmentName="اللغة العربية", Specializations=new List<Specialization>(){ new Specialization{  Id=1 } }  },
                 new Department{ Id=4, DepartmentName="الاقتصاد", Specializations=new List<Specialization>(){ new Specialization{  Id=1 } }  }
            };
        }
        public void Add(Department entity)
        {
            entity.Id = departments.Max(a => a.Id) + 1;
            departments.Add(entity);
        }

        public void Delete(int id)
        {
            var department = Find(id);
            departments.Remove(department);
        }

        public Department Find(int id)
        {
           return departments.SingleOrDefault(a => a.Id == id);
        }

        public IList<Department> List()
        {
            return departments;
        }

        public void Update(int id, Department newDep)
        {
           var oldDep = Find(id);
            oldDep.DepartmentName = newDep.DepartmentName;
        }
    }
}
