using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class NationalityRepository : ICollegeGradingSysRepository<Nationality>
    {
        IList<Nationality> nationalities;
        public NationalityRepository()
        {
            nationalities = new List<Nationality>()
            {
                new Nationality { Id=1, CountryName="اليمن" , NationalityName="يمني"},
                new Nationality { Id=2, CountryName="السعودية", NationalityName="سعودي"}
            };
        }
        public Nationality Add(Nationality entity)
        {
            var nationality = nationalities.FirstOrDefault();
            entity.Id = nationality != null ? nationalities.Max(b => b.Id) + 1 : 1;            
            nationalities.Add(entity);
            return entity;
        }

        public Nationality Delete(int id)
        {
            var nationality = Find(id);
            if (nationality != null)
            {
                nationalities.Remove(nationality);
            }
            return nationality;
        }

        public Nationality Find(int id)
        {
            return nationalities.SingleOrDefault(a => a.Id == id);
        }

        public IList<Nationality> List()
        {
            return nationalities;
        }

        public Nationality Update(int id, Nationality newNationality)
        {
           var oldNationality = Find(id);
            if (oldNationality != null)
            {
                oldNationality.CountryName = newNationality.CountryName;
                oldNationality.NationalityName = newNationality.NationalityName;
            }
            return newNationality;
        }
    }
}
