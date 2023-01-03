using KinoClass.Models;
using Microsoft.EntityFrameworkCore;

namespace APIkino.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {

        }

        //seed the database
     
        public  DbSet<Movies> movies { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
    }
}
