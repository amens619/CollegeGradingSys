using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class GovernorateRepository : ICollegeGradingSysRepository<Governorate>
    {
        IList<Governorate> governorates;
        public GovernorateRepository()
        {
            governorates = new List<Governorate>()
            {
                new Governorate{ Id=1, GovernorateName="حضرموت" , Nationality= new Nationality{  Id =1,CountryName="اليمن"} },
                 new Governorate{ Id=2, GovernorateName="المهرة" , Nationality =new Nationality{  Id =1,CountryName="اليمن"} },
                 new Governorate{ Id=3, GovernorateName="صنعاء", Nationality =new Nationality{  Id =1,CountryName="اليمن"}},
                 new Governorate{ Id=4, GovernorateName="جدة",  Nationality =new Nationality{  Id =2,CountryName="السعودية"} },
                   new Governorate{ Id=5, GovernorateName="الرياض",  Nationality =new Nationality{  Id =2,CountryName="السعودية"} }
            };
        }
        public Governorate Add(Governorate entity)
        {
            var college = governorates.FirstOrDefault();
            entity.Id = college != null ? governorates.Max(b => b.Id) + 1 : 1;           
            governorates.Add(entity);
            return entity;
        }

        public Governorate Delete(int id)
        {
            var governorate = Find(id);
            if (governorate != null)
            {
                governorates.Remove(governorate);
            }
            return governorate;
        }

        public Governorate Find(int id)
        {
           return governorates.SingleOrDefault(a => a.Id == id);
        }

        public IList<Governorate> List()
        {
            return governorates;
        }

        public Governorate Update(int id, Governorate newDep)
        {
           var oldDep = Find(id);
            if (oldDep != null)
            {
                oldDep.GovernorateName = newDep.GovernorateName;
                oldDep.Nationality = newDep.Nationality;
            }
            return newDep;
            
        }
    }
}
