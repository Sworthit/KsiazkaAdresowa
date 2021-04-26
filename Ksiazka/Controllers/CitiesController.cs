using AutoMapper;
using Ksiazka.Data.Entitites;
using Ksiazka.Data.Models;
using Ksiazka.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Ksiazka.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly AddressBookRepository repository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        private static int? lastId;

        public CitiesController(AddressBookRepository repository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.repository = repository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        
        [HttpGet("{cityName}")]
        public async Task<ActionResult<CityDto>> Get(string cityName)
        {
            try
            {
                var result = await repository.GetCity(cityName);

                return mapper.Map<CityDto>(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet("last")]
        public async Task<ActionResult<CityDto>> GetLastAddedCity()
        {
            try
            {
                if (lastId == null) return BadRequest("No city was added just yet");
                var city = await repository.GetLastAddedCity(lastId.Value);

                return mapper.Map<CityDto>(city);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult<City>> Post(City city)
        {
            try
            {
                var existingCity = await repository.GetCity(city.CityName);
                if (existingCity != null)
                {
                    return BadRequest($"City : {city.CityName} already exists");
                }
                var location = linkGenerator.GetPathByAction("Get",
                    "Cities",
                    new { cityName = city.CityName });

                repository.Add(city);

                if (await repository.SaveChangesAsync())
                {
                    lastId = city.CityId;
                    return Created(location, mapper.Map<CityDto>(city));
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
