using Microsoft.EntityFrameworkCore;
using Narato.Academy.Events.DataProvider.Models;

namespace Narato.Academy.Events.DataProvider.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
