using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class RoleModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public int Description { get; set; }
    }
}
