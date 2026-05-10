using CollegeGradingSys.Models;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface INationalityService : IGenericService<Nationality>
    {
        Task CreateAsync(Nationality nationality);
        Task UpdateAsync(Nationality nationality);
        Task DeleteWithValidationAsync(int id);
    }
}
