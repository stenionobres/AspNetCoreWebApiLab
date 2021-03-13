using AspNetCoreWebApiLab.Api.Models.V1;
using System.Collections.Generic;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class RoleClaimService
    {
        public void Associate(RoleModel role, ClaimModel claim)
        {

        }

        public IEnumerable<ClaimModel> GetClaimsBy(RoleModel role)
        {
            return new List<ClaimModel>();
        }

        public void RemoveAssociation(int roleId, int claimId)
        {

        }
    }
}
