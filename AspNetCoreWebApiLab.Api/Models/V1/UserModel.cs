﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class UserModel : IValidatableObject
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

        public UserModel() { }

        public UserModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Occupation = user.Occupation;
            Email = user.Email;
        }

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
