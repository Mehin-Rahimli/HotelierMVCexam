using HotelierMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelierMVC.Areas.admin.ViewModels
{
    public class UpdateEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }
        public int MajorId { get; set; }
        public ICollection<Major>? Majors { get; set; }
        public string? FbLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
