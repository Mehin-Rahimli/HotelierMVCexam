using System.ComponentModel.DataAnnotations;

namespace HotelierMVC.ViewModels.Account
{
    public class RegisterVM
    {
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [MaxLength(50)]
        [MinLength(2)]
        public string Surname { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
