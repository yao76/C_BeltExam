using System.Collections.Generic;
namespace BeltExam.Models
{
    public class DashboardViewModel
    {
        public List<Idea> AllIdeas{get;set;}
        public User CurrentUser{get;set;}
        public Idea NewIdea{get;set;}
    }
}