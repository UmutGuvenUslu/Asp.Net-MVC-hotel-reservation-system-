using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtelZone.Business.HotelService;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;
using OtelZone.WebUI.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace OtelZone.WebUI.Controllers
{
    public class RezervasyonController : Controller
    {


        
            private readonly OtelZoneContext _context;
            private readonly IHotelService _hotelService;

            public RezervasyonController(OtelZoneContext context, IHotelService hotelService)
            {
                _context = context;
                _hotelService = hotelService;
            }



             [HttpGet]
            public IActionResult Index(Room oda)
             {
          
            
            var yenirezervasyon = new Rezervation
            {
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                RoomId = oda.RoomId,
                TotalPrice =oda.Price

            };

            _context.Rezervations.Add(yenirezervasyon);
            _context.SaveChanges();
          

            return View();
             }


        
        [HttpPost]
        public IActionResult CreateReservation(int roomId,string HotelId, DateTime checkInDate, DateTime checkOutDate)
        {
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

           
            var room = _context.Rooms.Include(r => r.Rezervations).Include(h=>h.Hotel).FirstOrDefault(r => r.RoomId == roomId);

            if (room == null) { 
                TempData["mesaj"] = "Oda Bulunamadı";
                return RedirectToAction("mesaj", "Rezervasyon");
            }
          
            var hasConflict = room.Rezervations.Any(r =>
            r.RezervationStatus == "Onaylandı" && 
                (checkInDate < r.CheckOutDate && checkOutDate > r.CheckInDate)); 


            if (hasConflict) { 
                TempData["mesaj"] = "Bu tarihler arasında rezervasyon yapılamaz.";
                return RedirectToAction("mesaj", "Rezervasyon");
            }



         
            var indate = TempData["bgonder"] as DateTime?;
            var outdate = TempData["bitisgonder"] as DateTime?;
            decimal totalPrice = 0;
           
            if (indate.HasValue && outdate.HasValue)
            {
                var totalDays = (outdate.Value - indate.Value).Days;
                totalPrice = totalDays * room.Price;
            }
           
       
            var reservation = new Rezervation
            {
                RoomId = roomId,
                HotelName = HotelId,
                UserId = userId,
                CheckInDate = (DateTime)TempData["bgonder"],
                CheckOutDate = (DateTime)TempData["bitisgonder"],
                TotalPrice = totalPrice,
                UserName = User.Identity.Name
            };

    
            _context.Rezervations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Rezervasyonlist");
        }


        public IActionResult Rezervasyonlist() {

            ViewData["Title"] = "Rezervasyon listesi";
            var benimrezervasyonlarım = _context.Rezervations.Where(r => r.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        
            return View(benimrezervasyonlarım);
        
        
        }
        

            public IActionResult AdminRezerveListe()
        {
            ViewData["Title"] = "Rezervasyon listesi";
            var benimrezervasyonlarım = _context.Rezervations.ToList();

            return View(benimrezervasyonlarım);


        }


        public IActionResult Cancel(int id)
        {
            var rezervasyon = _context.Rezervations
                .FirstOrDefault(r => r.RezervationId == id);

            if (rezervasyon == null)
            {
                TempData["mesaj"] = "Rezervasyon bulunamadı veya iptal etme yetkiniz yok.";
                return RedirectToAction("mesaj","Rezervasyon");
            }

            _context.Rezervations.Remove(rezervasyon);
            _context.SaveChanges();

            return RedirectToAction("Rezervasyonlist");
        }


        public IActionResult mesaj(string mesaj)
        {

            ViewData["Title"] = "Uyarı";

            return View();
        }

        [AuthorizeRole("Admin")]
       public IActionResult AdminCancel(int id)
        {
            var rezervasyon = _context.Rezervations
                .FirstOrDefault(r => r.RezervationId == id);

            if (rezervasyon == null)
            {
                TempData["mesaj"] = "Rezervasyon bulunamadı veya iptal etme yetkiniz yok.";
                return RedirectToAction("mesaj", "Rezervasyon");
            }

            _context.Rezervations.Remove(rezervasyon);
            _context.SaveChanges();

            return RedirectToAction("AdminRezerveListe");
        }
        [AuthorizeRole("Admin")]
        public IActionResult AdminOnay(int id)
        {
       
            var rezervasyon = _context.Rezervations
                .FirstOrDefault(r => r.RezervationId == id);

            if (rezervasyon == null)
            {
                TempData["mesaj"] = "Rezervasyon bulunamadı veya işlem yapma yetkiniz yok.";
                return RedirectToAction("mesaj", "Rezervasyon");
            }

        
            rezervasyon.RezervationStatus = "Onaylandı"; 
            _context.SaveChanges();

        
            return RedirectToAction("AdminRezerveListe");
        }
        [AuthorizeRole("Admin")]
        public IActionResult AdminRed(int id)
        {
           
            var rezervasyon = _context.Rezervations
                .FirstOrDefault(r => r.RezervationId == id);

        
            if (rezervasyon == null)
            {
                TempData["mesaj"] = "Rezervasyon bulunamadı veya işlem yapma yetkiniz yok.";
                return RedirectToAction("mesaj", "Rezervasyon");
            }

            rezervasyon.RezervationStatus = "Onaylanmadı"; 
            _context.SaveChanges();

          
            return RedirectToAction("AdminRezerveListe");
        }
        [AuthorizeRole("Admin")]
        public IActionResult AdminBekleme(int id)
        {
            var rezervasyon = _context.Rezervations
                .FirstOrDefault(r => r.RezervationId == id);

         
            if (rezervasyon == null)
            {
                TempData["mesaj"] = "Rezervasyon bulunamadı veya işlem yapma yetkiniz yok.";
                return RedirectToAction("mesaj", "Rezervasyon");
            }

            rezervasyon.RezervationStatus = "Admin Onayı Bekleniyor";
            _context.SaveChanges();

            return RedirectToAction("AdminRezerveListe");
        }


    }
}
