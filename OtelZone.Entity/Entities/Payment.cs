using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentStatus { get; set; }



        public int RezervationId { get; set; }
        public virtual Rezervation Rezervation { get; set; }

    }
}
