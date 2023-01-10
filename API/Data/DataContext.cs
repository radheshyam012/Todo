using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<TodoUser> Todos { get; set; }
        public DbSet<TodoTask> Tasks{ get; set; }
        
    }
}