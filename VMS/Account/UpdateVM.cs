using System.ComponentModel.DataAnnotations;

namespace Tasky.VMS.Account
{
    public class UpdateVM
    {
        public string? FullName { get; set; }


        public IFormFile? ProfilePhoto { get; set; }
       
        public string? ExistingProfilePhotoPath { get; set; }
    }
}
