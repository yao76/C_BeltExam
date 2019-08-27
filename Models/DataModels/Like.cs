using System;
using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models
{
    public class Like
    {
        [Key]
        public int LikeId{get;set;}

        public int UserId{get;set;}
        public User AssociatedUser{get;set;}

        public int IdeaId{get;set;}
        public Idea AssociatedIdea{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
    }
}