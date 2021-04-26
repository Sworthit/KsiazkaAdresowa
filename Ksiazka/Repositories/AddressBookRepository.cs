using Ksiazka.Data;
using Ksiazka.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Ksiazka.Repositories
{
    public class AddressBookRepository
    {
        private readonly AddressBookDBContext context;
        private ILogger<AddressBookRepository> logger;

        public AddressBookRepository(AddressBookDBContext context, ILogger<AddressBookRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            logger.LogInformation($"Adding object of type {entity.GetType()} to the context");
            context.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            logger.LogInformation($"Attempting to save the changes in the context");

            // If at least one row was change then return true
            return (await context.SaveChangesAsync()) > 0;
        }


        public async Task<City> GetLastAddedCity(int cityId)
        {
            logger.LogInformation($"Getting last added city");

            IQueryable<City> query = context.Cities;

            query = query
                .Where(c => c.CityId == cityId);

            query = query
                .Include(a => a.Addresses);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<City> GetCity(string cityName)
        {
            logger.LogInformation($"Getting all addresses from {cityName}");

            IQueryable<City> query = context.Cities;

            query = query
                .Where(c => c.CityName == cityName);

            query = query
                .Include(a => a.Addresses);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Address[]> GetAddress(string cityName, string address)
        {
            logger.LogInformation($"Getting all addresses from {cityName}, {address}");
            IQueryable<Address> query = context.Addresses
                .Where(a => a.StreetName == address && a.CityName == cityName);

            return await query.ToArrayAsync();
        }

       public async Task<Address> GetLastAddedAddress(string cityName, int addressId)
        {
            logger.LogInformation($"Getting last added address");

            IQueryable<Address> query = context.Addresses
                .Where(c => c.CityName == cityName && c.AddressId == addressId);


            return await query.FirstOrDefaultAsync();
        }

    }
}
