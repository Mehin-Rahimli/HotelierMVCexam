

using System.ComponentModel.DataAnnotations;

namespace HotelierMVC.Areas.admin.ViewModels
{
    public class CreateMajorVM
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

    }
}
