using HotelierMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelierMVC.Areas.admin.ViewModels
{
    public class CreateEmployeeVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public int MajorId { get; set; }
        public ICollection<Major>? Majors { get; set; }
        public string? FbLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
