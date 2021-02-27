using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class UserModel
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
        [StringLength(255)]
        public string Email { get; set; }
    }
}
