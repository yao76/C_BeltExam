using Microsoft.EntityFrameworkCore;
namespace BeltExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext  (DbContextOptions options) : base(options) { }

        public DbSet<User> users{get;set;}
        public DbSet<Idea> ideas{get;set;}
        public DbSet<Like> likes{get;set;}
    }
}