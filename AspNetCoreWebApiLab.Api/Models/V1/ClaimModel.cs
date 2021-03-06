﻿
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiLab.Api.Models.V1
{
    public class ClaimModel
    {
        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }
    }
}
