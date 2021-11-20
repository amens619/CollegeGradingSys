using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CityRepository : ICollegeGradingSysRepository<City>
    {
        IList<City> citys;
        public CityRepository()
        {
            citys = new List<City>()
            {
                new  City{ Id=1, CityName="المكلا",  District = new District{ Id=1,  DistrictName="ارياف المكلا" }  }
            };
        }
        public City Add(City entity)
        {
            var city = citys.FirstOrDefault();
            entity.Id = city != null ? citys.Max(b => b.Id) + 1 : 1;            
            citys.Add(entity);
            return entity;
        }

        public City Delete(int id)
        {
            var city = Find(id);
            if (city != null)
            {
                citys.Remove(city);
            }
            return city;
        }

        public City Find(int id)
        {
           return citys.SingleOrDefault(a => a.Id == id);
        }

        public IList<City> List()
        {
            return citys;
        }

        public City Update(int id, City newCity)
        {
           var oldCity = Find(id);
            if (oldCity != null)
            {
                oldCity.CityName = newCity.CityName;
                oldCity.District = newCity.District;
            }
            return newCity;
        }
    }
}
