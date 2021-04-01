using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class SignInModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
