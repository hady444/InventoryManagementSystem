using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BLL.ViewModel
{
    public class CreateProductVM
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
