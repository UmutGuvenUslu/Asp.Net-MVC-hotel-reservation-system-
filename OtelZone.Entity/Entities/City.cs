using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public ICollection<District> District { get; set; }

    }
}
