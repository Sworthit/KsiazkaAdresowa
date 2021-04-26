using AutoMapper;
using Ksiazka.Data.Entitites;
using Ksiazka.Data.Models;

namespace Ksiazka.Data
{
    public class AddressBookProfile : Profile
    {
       public AddressBookProfile()
        {
            CreateMap<City, CityDto>()
                .ForMember(c => c.addresses, a => a.MapFrom(s => s.Addresses))
                .ReverseMap();

            CreateMap<Address, AddressDto>()
                .ReverseMap();

        }
    }
}
