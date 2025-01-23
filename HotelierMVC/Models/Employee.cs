namespace HotelierMVC.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int MajorId { get; set; }
        public Major Major { get; set; }
        public string? Image { get; set; }
        public string? FbLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
