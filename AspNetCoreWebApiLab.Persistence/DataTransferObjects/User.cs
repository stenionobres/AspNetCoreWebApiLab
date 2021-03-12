using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Persistence.DataTransferObjects
{
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Occupation { get; set; }
    }
}
