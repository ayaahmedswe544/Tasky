using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tasky.Models;

namespace Tasky.VMS.Category
{
    public class CreateVM
    {
        [Required(ErrorMessage ="Name is required")]
        [Remote(action: "IsCatNameInUse", controller: "Categories", ErrorMessage = "Category name is already in use.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string AppUserId { get; set; }

    }
}
