using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email is required.")]
        public string Email{get;set;}

        [Required(ErrorMessage="Password is required.")]
        [MinLength(8,ErrorMessage="Password must be at least 8 characters long!")]
        [DataType(DataType.Password)]
        public string Password{get;set;}
    }
}