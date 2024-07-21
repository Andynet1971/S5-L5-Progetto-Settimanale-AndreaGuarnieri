using System.ComponentModel.DataAnnotations;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
