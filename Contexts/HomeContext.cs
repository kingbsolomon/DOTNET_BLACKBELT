using Microsoft.EntityFrameworkCore;
using BeltExam.Models;

namespace BeltExam.Contexts
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users {get;set;}
        public DbSet<Event> Events {get;set;}
        public DbSet<RSVP> RSVPS {get;set;}
    }
}