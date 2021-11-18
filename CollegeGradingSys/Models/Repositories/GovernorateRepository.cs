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
                 new Governorate{ Id=4, GovernorateName="جدة",  Nationality =new Nationality{  Id =2,CountryName="السعودية"} }
            };
        }
        public void Add(Governorate entity)
        {
            entity.Id = governorates.Max(a => a.Id) + 1;
            governorates.Add(entity);
        }

        public void Delete(int id)
        {
            var governorate = Find(id);
            governorates.Remove(governorate);
        }

        public Governorate Find(int id)
        {
           return governorates.SingleOrDefault(a => a.Id == id);
        }

        public IList<Governorate> List()
        {
            return governorates;
        }

        public void Update(int id, Governorate newDep)
        {
           var oldDep = Find(id);
            oldDep.GovernorateName = newDep.GovernorateName;
            oldDep.Nationality = newDep.Nationality;
            
        }
    }
}
