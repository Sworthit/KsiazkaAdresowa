using AutoMapper;
using Ksiazka.Data.Entitites;
using Ksiazka.Data.Models;
using Ksiazka.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksiazka.Controllers
{
    [ApiController]
    [Route("{city}/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly AddressBookRepository repository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        private static int? lastAddressId;
        private static string lastCityName;

        public AddressesController(AddressBookRepository repository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.repository = repository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet("{addressName}")]
        public async Task<ActionResult<AddressDto[]>> Get(string city, string addressName)
        {
            try
            {
                var address = await repository.GetAddress(city, addressName);
                return mapper.Map<AddressDto[]>(address);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet("last")]
        public async Task<ActionResult<AddressDto>> GetLastAddedAddress()
        {
            try
            {
                if (lastAddressId == null || lastCityName == null) return BadRequest("No address was added just yet");
                var lastAddedCity = await repository.GetLastAddedAddress(lastCityName, lastAddressId.Value);

                return mapper.Map<AddressDto>(lastAddedCity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult<AddressDto>> Post(string city, Address address)
        {
            try
            {
                var existingCity = await repository.GetCity(city);
                if (existingCity == null)
                {
                    return BadRequest($"{city} does not exists");
                }

                var existingAddress = existingCity.Addresses;
                if (existingAddress != null)
                {
                    foreach (var adr in existingAddress)
                    {
                        if (adr.AddressId == address.AddressId)
                        {
                            return BadRequest($"Address : {address.StreetName} {address.AddressId} already exists");
                        }
                    }
                }
                if (existingAddress == null)
                {
                    existingAddress = new List<Address>();
                }
                address.CityName = city;
                existingAddress.Add(address);
                
                if (await repository.SaveChangesAsync())
                {
                    var location = linkGenerator.GetPathByAction("Get",
                        "Addresses",
                        values: new { city = city, addressName = address.StreetName });

                    lastAddressId = address.AddressId;
                    lastCityName = address.CityName;
                    return Created(location, mapper.Map<AddressDto>(address));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
            return BadRequest();
        }
    }
}
