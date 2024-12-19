using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OtelZone.Business.HotelService;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;
using OtelZone.WebUI.Attributes;

namespace OtelZone.WebUI.Controllers
{
    public class AdminPanelController : Controller
    {


        private readonly OtelZoneContext _context;
        private readonly IHotelService _hotelService;

        public AdminPanelController(OtelZoneContext context, IHotelService hotelService)
        {
            _context = context;
            _hotelService = hotelService;
        }








        [AuthorizeRole("Admin")]
        public IActionResult Index()
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Admin Paneli";
            return View();
        }


        [HttpGet]
        public IActionResult adminotellistele(string? otelname, int? CityId, int? DistrictId, DateTime? otelgiris, DateTime? otelcikis)
        {
            ViewData["Title"] = "OtelListesi";

            // Şehirleri ViewBag'e ekle
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");

            // Otel sorgusunu başlat
            var oteller = _context.GetAllHotels().AsQueryable();

            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(otelname))
            {
                oteller = oteller.Where(o => o.HotelName.Contains(otelname, StringComparison.OrdinalIgnoreCase));
            }

            if (CityId.HasValue)
            {
                oteller = oteller.Where(o => o.CityId == CityId.Value);
            }

            if (DistrictId.HasValue)
            {
                oteller = oteller.Where(o => o.DistrictId == DistrictId.Value);
            }

           
            // Sonuçları listele ve view'a gönder
            var otellerListesi = oteller.ToList();
            return View(otellerListesi);
        }

        [HttpPost] 
        [ActionName("adminotellistele")]
        public IActionResult adminotellistelePost(string? otelname, int? CityId, int? DistrictId, DateTime? otelgiris, DateTime? otelcikis)
        {
            // Query string parametrelerini HttpGet metoduna yönlendir
            return RedirectToAction("adminotellistele", new
            {
                otelname = otelname,
                CityId = CityId,
                DistrictId = DistrictId,
                otelgiris = otelgiris?.ToString("yyyy-MM-dd"),
                otelcikis = otelcikis?.ToString("yyyy-MM-dd")
            });
        }


        [AuthorizeRole("Admin")]
        public IActionResult adminotelsil(int HotelId)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Otel Sil";
            var otel = _context.Hotels.FirstOrDefault(h => h.HotelId == HotelId);
            if (otel != null)
            {
                _context.Hotels.Remove(otel);
                _context.SaveChanges();
            }
            return RedirectToAction("adminotellistele");

        }


        [AuthorizeRole("Admin")]
        [HttpGet]
        public  IActionResult otelduzenle(int HotelId)
        {
           
            ViewData["Title"] = "Otel Düzenle";
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            Hotel duzenle =  _context.GetAllHotels().FirstOrDefault(h=>h.HotelId==HotelId);
            return View(duzenle);

        }






        [AuthorizeRole("Admin")]
        [HttpPost]
        public  IActionResult otelduzenle(Hotel otel)
        {

            ViewData["Title"] = "Otel Düzenle";
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            Hotel duzenle =  _context.GetAllHotels().FirstOrDefault(h => h.HotelId == otel.HotelId);
            duzenle.HotelName = otel.HotelName;
            duzenle.HotelAdress = otel.HotelAdress;
            duzenle.HotelDesc = otel.HotelDesc;
            duzenle.HotelFacilities = otel.HotelFacilities;
            duzenle.DistrictId = otel.DistrictId;
            duzenle.CityId = otel.CityId;
            duzenle.HotelImgUrl = otel.HotelImgUrl;
            duzenle.HotelIsActive = otel.HotelIsActive;
            duzenle.Rating = otel.Rating;
            
            _context.SaveChanges();
            return  RedirectToAction("adminotellistele");
        }




        [AuthorizeRole("Admin")]
        [HttpGet]
        public IActionResult odaekle(int HotelId)
        {

            ViewData["Title"] = "Otel Düzenle";
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            Room oda = new Room
            {
                HotelId = HotelId,
            };
            return View(oda);

        }






        [AuthorizeRole("Admin")]
        [HttpPost]
        public IActionResult odaekle(Room oda)
        {
            ViewData["Title"] = "Otel Düzenle";
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            Room duzenle = new Room();
            duzenle.Price = oda.Price;
            duzenle.RoomDesc = oda.RoomDesc;
            duzenle.RoomCreatedDate = DateTime.Now;
            duzenle.RoomImageUrl = oda.RoomImageUrl;
            duzenle.RoomIsAvilable = oda.RoomIsAvilable;
            duzenle.RoomType = oda.RoomType;
            duzenle.BedCount = oda.BedCount;
            duzenle.HotelId = oda.HotelId;
           
            _context.Rooms.Add(duzenle);
            _context.SaveChanges();
            return RedirectToAction("adminotellistele");
        }


        [AuthorizeRole("Admin")]
        public IActionResult odalistele(int hotelid)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Oda Listele";
            var odalar = _context.GetAllHotels().FirstOrDefault(r=>r.HotelId==hotelid).Rooms.ToList();


            return View(odalar);
        }


        [AuthorizeRole("Admin")]
        public IActionResult odasil(int HotelId)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Oda Sil";
            var oda = _context.Rooms.FirstOrDefault(h => h.HotelId == HotelId);
            if (oda != null)
            {
                _context.Rooms.Remove(oda);
                _context.SaveChanges();
            }
            return RedirectToAction("odalistele");

        }

        [AuthorizeRole("Admin")]
        [HttpGet]
        public IActionResult odaduzenle(int id)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Oda Düzenle";
            var oda = _context.Rooms.FirstOrDefault(h => h.RoomId == id);
            
            return View(oda);

        }


        [AuthorizeRole("Admin")]
        [HttpPost]
        public IActionResult odaduzenle(Room gelen)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Oda Düzenle";
            var oda = _context.Rooms.FirstOrDefault(r => r.RoomId == gelen.RoomId);
            if (gelen != null)
            {
                
                oda.RoomImageUrl = gelen.RoomImageUrl;
                oda.RoomDesc = gelen.RoomDesc;
                oda.BedCount = gelen.BedCount;
                oda.Price = gelen.Price;
                oda.RoomType = gelen.RoomType;

                _context.SaveChanges();
            }
            return RedirectToAction("adminotellistele");

        }




    }
}
