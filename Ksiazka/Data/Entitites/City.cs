using System.Collections.Generic;

namespace Ksiazka.Data.Entitites
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
