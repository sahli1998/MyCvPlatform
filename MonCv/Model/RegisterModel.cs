using System.ComponentModel.DataAnnotations;

namespace MonCv.Model
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
