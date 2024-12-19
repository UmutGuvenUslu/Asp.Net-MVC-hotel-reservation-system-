using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class Rezervation
    {
        public int RezervationId { get; set; }
     
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string RezervationStatus { get; set; } = "Admin Onayı Bekleniyor";
        public DateTime RezervationCreatedDate { get; set; } = DateTime.Now;
        public string UserName { get; set; }

        public string HotelName { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
       
        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
