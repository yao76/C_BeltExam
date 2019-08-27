using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace BeltExam.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId{get;set;}

        [Required]
        [MinLength(5)]
        public string Content{get;set;}

    
        public int UserId{get;set;}
        public User Author{get;set;}

        public List<Like> LikedBy{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;


    }
}