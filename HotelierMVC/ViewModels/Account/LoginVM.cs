using System.ComponentModel.DataAnnotations;

namespace HotelierMVC.ViewModels.Account
{
    public class LoginVM
    {
        [MaxLength(256)]
        public string UserNameOrEmail { get; set; }

        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
