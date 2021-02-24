
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.Experiments
{
    public class ResourceModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Description { get; set; }
    }
}
