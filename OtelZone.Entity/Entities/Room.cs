using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class Room
    {
        public int RoomId { get; set; }

       

        public string RoomType { get; set; }

        public int BedCount { get; set; }

        public decimal Price { get; set; }

        public bool RoomIsAvilable  { get; set; }

        public string RoomDesc { get; set; }

        public string RoomImageUrl { get; set; }

        public DateTime RoomCreatedDate { get; set; } = DateTime.Now;


        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
        public ICollection<Rezervation> Rezervations { get; set; }



       
        public bool IsAvailableForDates(DateTime checkInDate, DateTime checkOutDate)
        {
            return !Rezervations.Any(r =>
                (checkInDate < r.CheckOutDate && checkOutDate > r.CheckInDate));
        }


    }
}
