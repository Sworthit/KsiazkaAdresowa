using Ksiazka.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ksiazka.Data.Models
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            List<Address> addresses1 = new List<Address>();
            addresses1.Add(new Address
            {
                AddressId = 1,
                BuildingNumber = 2,
                StreetName = "Sukiennicza",
                CityName = "Bb"
            });
            addresses1.Add(new Address
            {
                AddressId = 2,
                BuildingNumber = 1,
                StreetName = "Sukiennicza",
                CityName = "Bb"
            });
            using (var context = new AddressBookDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<AddressBookDBContext>>()))
            {
                if (context.Cities.Any()) return;

                context.Cities.AddRange(
                    new City
                    {
                        CityId = 1,
                        CityName = "Bb",
                        Addresses = addresses1
                    });

                context.SaveChanges();
            }
        }
    }
}
