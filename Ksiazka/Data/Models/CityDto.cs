using System.Collections.Generic;

namespace Ksiazka.Data.Models
{
    public class CityDto
    {
        public string CityName { get; set; }
        public ICollection<AddressDto> addresses { get; set; }
    }
}
