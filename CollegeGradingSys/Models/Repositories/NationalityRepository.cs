using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class NationalityRepository : ICollegeGradingSysRepository<Nationality>
    {
        IList<Nationality> nationalities;
        public NationalityRepository()
        {
            nationalities = new List<Nationality>()
            {
                new Nationality { Id=1, CountryName="اليمن" , NationalityName="يمني"},
                new Nationality { Id=2, CountryName="السودان", NationalityName="سوداني"}
            };
        }
        public void Add(Nationality entity)
        {

            var nationality = nationalities.FirstOrDefault();

            entity.Id = nationality != null ? nationalities.Max(b => b.Id) + 1 : 1;
            
            nationalities.Add(entity);
        }

        public void Delete(int id)
        {
            var nationality = Find(id);
            nationalities.Remove(nationality);
        }

        public Nationality Find(int id)
        {
            return nationalities.SingleOrDefault(a => a.Id == id);
        }

        public IList<Nationality> List()
        {
            return nationalities;
        }

        public void Update(int id, Nationality newNationality)
        {
           var oldNationality = Find(id);
            oldNationality.CountryName = newNationality.CountryName;
            oldNationality.NationalityName = newNationality.NationalityName;
        }
    }
}
