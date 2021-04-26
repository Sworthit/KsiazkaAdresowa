using Microsoft.EntityFrameworkCore;

namespace Ksiazka.Data
{
    public class AddressBookDBContext : DbContext
    {
        public AddressBookDBContext(DbContextOptions<AddressBookDBContext> options)
            : base(options) { }

        public DbSet<Entitites.City> Cities { get; set; }
        public DbSet<Entitites.Address> Addresses { get; set; }
    }
}
