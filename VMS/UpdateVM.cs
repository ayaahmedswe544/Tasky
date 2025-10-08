using System.ComponentModel.DataAnnotations;

namespace Tasky.VMS
{
    public class UpdateVM
    {
        [Required]
        public string FullName { get; set; }
    }
}
