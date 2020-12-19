using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email:")]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name="Password:")]
        public string LoginPassword { get; set; }
    }
}