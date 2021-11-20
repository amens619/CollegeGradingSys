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
                new Department{ Id=1, DepartmentName="فقه" , College= new College{  Id =1,CollegeName="الشريعة"} },
                 new Department{ Id=2, DepartmentName="علوم القرآن" , College =new College{  Id =1,CollegeName="الشريعة"} },
                 new Department{ Id=3, DepartmentName="اللغة العربية", College =new College{  Id =1,CollegeName="الشريعة"}},
                 new Department{ Id=4, DepartmentName="الاقتصاد",  College =new College{  Id =1,CollegeName="الشريعة"} }
            };
        }
        public Department Add(Department entity)
        {
            var college = departments.FirstOrDefault();
            entity.Id = college != null ? departments.Max(b => b.Id) + 1 : 1;            
            departments.Add(entity);
            return entity;
        }

        public Department Delete(int id)
        {
            var department = Find(id);
            if (department != null)
            {
                departments.Remove(department);
            }
            return department;
        }

        public Department Find(int id)
        {
           return departments.SingleOrDefault(a => a.Id == id);
        }

        public IList<Department> List()
        {
            return departments;
        }

        public Department Update(int id, Department newDep)
        {
                var oldDep = Find(id);
                if (oldDep != null)
                {
                    oldDep.DepartmentName = newDep.DepartmentName;
                    oldDep.College = newDep.College;
                }
                return newDep;
        }
    }
}
