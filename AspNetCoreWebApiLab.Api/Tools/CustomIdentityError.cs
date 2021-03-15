using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebApiLab.Api.Tools
{
    public class CustomIdentityError
    {
        public static void CatchErrorIfNeeded(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                var identityError = identityResult.Errors.First();

                if (identityError != null)
                {
                    var errorDetail = identityError.Description;
                    throw new ApplicationException(errorDetail);
                }
            }
        }
    }
}
