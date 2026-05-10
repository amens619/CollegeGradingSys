using AutoMapper;
using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels.StPersonalData;

namespace CollegeGradingSys.Data
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<StPersonalData, StPersonalDataDto>();

            // من الـ ViewModel إلى الـ Entity (للحفظ)
            CreateMap<StPersonalDataFormViewModel, StPersonalData>();
        }
    }
}
