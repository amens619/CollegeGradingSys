using CollegeGradingSys.ViewModels;
using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string UserId { get; set; }

        public IList<string> UserRoles { get; set; }
        public List<UserClaim> Claims { get; set; }

    }

   
}
