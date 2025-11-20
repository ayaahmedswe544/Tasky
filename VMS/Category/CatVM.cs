using System.ComponentModel.DataAnnotations;

namespace Tasky.VMS.Category
{
    public class CatVM
    {
        [Required]
        public int Id { get; set; }
    //remote validation
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
