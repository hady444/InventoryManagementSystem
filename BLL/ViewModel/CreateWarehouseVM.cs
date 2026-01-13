
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModel
{
    public class CreateWarehouseVM
    {
        [Required]
        public string Name { get; set; }
        public string? Location { get; set; }
    }
}
