using HotelierMVC.Models;

namespace HotelierMVC.ViewModels.Home
{
    public class HomeVM
    {
        public ICollection<Employee>? Employees { get; set; }
    }
}
