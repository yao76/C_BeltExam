using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace BeltExam.Models 
{
    public class User
    {
        [Key]
        public int UserId{get;set;}

        [Required(ErrorMessage="Name is required.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please use only letters and spaces")]
        [Display(Name="Name")]
        public string Name{get;set;}
        
        [Required(ErrorMessage="Alias is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please use only letters and numbers")]
        public string Alias{get;set;}


        [Required(ErrorMessage="Email is required.")]
        [EmailAddress]
        public string Email{get;set;}

        [Required(ErrorMessage="Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long!")]
        public string Password{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string Confirm{get;set;}


        public List<Idea> UserIdeas{get;set;}
        public List<Like> UserLikes{get;set;}



    }
}