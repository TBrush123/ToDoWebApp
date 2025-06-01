using Microsoft.EntityFrameworkCore;

namespace ToDoList.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<ToDoList.Models.Task> Tasks { get; set; }

    }
}
