
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModel
{
    public class CreateWarehouseVM
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Shorten the location - maximum 200 letter")]
        public string? Location { get; set; }
    }
}
