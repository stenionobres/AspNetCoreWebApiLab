using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class UserPostModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public string Occupation { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var allowedDomain = "email.com";

            if (Email.Contains(allowedDomain) == false)
            {
                yield return new ValidationResult($"The provided email must belong to the {allowedDomain} domain", new[] { "email" });
            }
        }
    }
}
