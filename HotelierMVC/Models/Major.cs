namespace HotelierMVC.Models
{
    public class Major:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
