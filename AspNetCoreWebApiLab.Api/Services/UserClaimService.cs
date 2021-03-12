using AspNetCoreWebApiLab.Api.Models.V1;
using System.Collections.Generic;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserClaimService
    {
        public void Associate(UserModel user, ClaimModel claim)
        {

        }

        public IEnumerable<ClaimModel> GetClaimsBy(UserModel user)
        {
            return new List<ClaimModel>();
        }

        public void RemoveAssociation(int userId, int claimId)
        {

        }
    }
}
