using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class DistrictRepository : ICollegeGradingSysRepository<District>
    {
        IList<District> districts;
        public DistrictRepository()
        {
            districts = new List<District>()
            {
                new  District{ Id=1, DistrictName="ارياف المكلا", Governorate = new Governorate{ Id=1, GovernorateName="حضرموت" }  }
            };
        }
        public void Add(District entity)
        {
            var district = districts.FirstOrDefault();
            entity.Id = district != null ? districts.Max(b => b.Id) + 1 : 1;            
            districts.Add(entity);
        }

        public void Delete(int id)
        {
            var district = Find(id);
            districts.Remove(district);
        }

        public District Find(int id)
        {
           return districts.SingleOrDefault(a => a.Id == id);
        }

        public IList<District> List()
        {
            return districts;
        }

        public void Update(int id, District newDistrict)
        {
           var oldDistrict = Find(id);
            oldDistrict.DistrictName = newDistrict.DistrictName;
            oldDistrict.Governorate = newDistrict.Governorate;
        }
    }
}
