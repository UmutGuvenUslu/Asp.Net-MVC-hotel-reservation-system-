using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
      
        public short Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewCreatedDate { get; set; } = DateTime.Now;



        public int HotelId { get; set; }
        public int UserId { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual User User { get; set; }
    }
}
