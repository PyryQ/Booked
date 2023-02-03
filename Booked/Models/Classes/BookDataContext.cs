using Booked.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booked.Models.Classes
{
    public class BookDataContext : DbContext
    {
        public BookDataContext(DbContextOptions<BookDataContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<IBook> Books { get; set; }
    }
}