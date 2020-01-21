using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class ChangeSecretModel
    {
        public int ApiClientId { get; set; }
        public string Key { get; set; }
        [Required]
        [MinLength(16, ErrorMessage = "Secret must be at least 16 characters.")]
        public string Secret { get; set; }
    }
}