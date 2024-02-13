using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {}

        public DbSet<User> User { set; get; }
        public DbSet<Priority> Priority { set; get; }
        public DbSet<Status> Status { set; get; }
        public DbSet<ToDoTask> ToDoTask { set; get; }
    }
}
