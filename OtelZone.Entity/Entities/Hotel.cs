using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class Hotel
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string HotelAdress { get; set; }

        public short Rating { get; set; }

        public string HotelDesc { get; set; }

        public string HotelFacilities { get; set; }

        public string HotelImgUrl { get; set; }

        public DateTime HotelCreatedDate { get; set; } = DateTime.Now;

        public bool HotelIsActive { get; set; }

        
        



        public int CityId { get; set; }
        public virtual City City { get; set; }

        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        public ICollection<Room> Rooms { get; set; } 
        public ICollection<Review> Reviews { get; set; }


        public int RoomCount => Rooms?.Count ?? 0;


    }
}
