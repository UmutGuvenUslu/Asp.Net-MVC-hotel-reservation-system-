using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Dac.Model;
using OtelZone.Business.HotelService;
using OtelZone.Business.UserService;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;
using OtelZone.WebUI.Attributes;
using System.Security.Claims;

namespace OtelZone.WebUI.Controllers
{
    public class HotelController : Controller
    {
        private readonly OtelZoneContext _context;
        private readonly IHotelService _hotelService;

        public HotelController(OtelZoneContext context, IHotelService hotelService)
        {
            _context = context;
            _hotelService = hotelService;
        }


        [HttpGet]
        public IActionResult Index(string? otelname, int? CityId, int? DistrictId, DateTime? otelgiris, DateTime? otelcikis)
        {
            ViewData["Title"] = "OtelListesi";

        
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            TempData["bitis"] = otelcikis;
            TempData["baslangic"] = otelgiris;
        
            var oteller = _context.GetAllHotels().AsQueryable();

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

            oteller = oteller.Where(o => o.RoomCount > 0); 

            
            var otellerListesi = oteller.ToList();
            return View(otellerListesi);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(string? otelname, int? CityId, int? DistrictId, DateTime? otelgiris, DateTime? otelcikis)
        {

            TempData["bitis"] = otelcikis;
            TempData["baslangic"] = otelgiris;
           
            return RedirectToAction("Index", new
            {
                otelname = otelname,
                CityId = CityId,
                DistrictId = DistrictId,
                otelgiris = otelgiris?.ToString("yyyy-MM-dd"),
                otelcikis = otelcikis?.ToString("yyyy-MM-dd")
            });
        }



        [AuthorizeRole("Admin")]
        [HttpGet]
        public IActionResult otelekle()
        {
            ViewData["Title"] = "Otel Ekle";
            Hotel otel = new Hotel();
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");

            return View();
        }
        [AuthorizeRole("Admin")]
        [HttpPost]
        public async Task<IActionResult> otelekle(Hotel otel)
        {

            ViewData["Title"] = "Otel Ekle";
            ViewBag.Hata = false;
            var success = await _hotelService.Otelkaydet(otel.HotelName, otel.HotelAdress, otel.Rating, otel.HotelDesc, otel.HotelFacilities, otel.HotelImgUrl, otel.HotelIsActive, otel.CityId, otel.DistrictId);

            if (!success)
            {
                ViewBag.Hata = true;
                ModelState.AddModelError("", "Otel zaten mevcut.");
                return View();
            }



            return RedirectToAction("Index", "AdminPanel");

        }


        
        [HttpPost]
        public JsonResult GetDistrictsByCityId(int cityId)
        {
            var districts = _context.Districts.Where(d => d.CityId == cityId).ToList();
            var districtList = districts.Select(d => new { d.DistrictId, d.DistrictName }).ToList();
            return Json(districtList);
        }




        public async Task<IActionResult> detay(int Hotelid,DateTime? baslangicTarihi,DateTime? bitisTarihi)
        {
            ViewData["Title"] = "Otel Detayı";
            TempData["hotelid"] = Hotelid;

          
            var indate = TempData["baslangic"] as DateTime?;
            var outdate = TempData["bitis"] as DateTime?;
            TempData["bgonder"]= TempData["baslangic"] as DateTime?;
            TempData["bitisgonder"] = TempData["bitis"] as DateTime?;
           
            if (indate == null || outdate == null)
            {
                TempData["Error"] = "Geçerli bir tarih aralığı seçilmedi!";
                return RedirectToAction("Index");
            }

            var odalar = _context.Rooms
       .Where(o =>
           !o.Rezervations.Any() || 
           !o.Rezervations.Any(r =>
               r.RezervationStatus == "Onaylandı" && 
               r.CheckInDate < outdate &&          
               r.CheckOutDate > indate             
           ) 
       ).ToList();


          
            var reviews = await _hotelService.GetReviewByHotelId(Hotelid);
            var otel = await _hotelService.GetHotelById(Hotelid);

            return View(Tuple.Create<Hotel, List<Review>,List<Room>>(otel, reviews,odalar));

        }


        [HttpPost]
        public async Task<ActionResult>  AddComment(short rating,string commentText, int hotelId)
        {
            var indate = TempData["baslangic"] as DateTime?;
            var outdate = TempData["bitis"] as DateTime?;

            var odalar = _context.Rooms
      .Where(o =>
          !o.Rezervations.Any() ||
          !o.Rezervations.Any(r =>
              r.RezervationStatus == "Onaylandı" &&
              r.CheckInDate < outdate &&
              r.CheckOutDate > indate
          )
      ).ToList();

            var newComment = new Review
            {
                Rating = rating,
                Comment = commentText,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                HotelId= hotelId, 
                ReviewCreatedDate = DateTime.Now
            };

            _context.Reviews.Add(newComment);
            _context.SaveChanges();

          
            var reviews = await _hotelService.GetReviewByHotelId(hotelId);
            var otel = await _hotelService.GetHotelById(hotelId);

            var model = Tuple.Create(otel, reviews,odalar);


            return View("detay", model); 
        }

        [HttpPost]
        public async Task<IActionResult> yorumsil(int reviewid,int hotelId)
        {


            var indate = TempData["baslangic"] as DateTime?;
            var outdate = TempData["bitis"] as DateTime?;

            var odalar = _context.Rooms
      .Where(o =>
          !o.Rezervations.Any() ||
          !o.Rezervations.Any(r =>
              r.RezervationStatus == "Onaylandı" &&
              r.CheckInDate < outdate &&
              r.CheckOutDate > indate
          )
      ).ToList();


            var reviews = await _hotelService.GetReviewByHotelId(hotelId);
            var otel = await _hotelService.GetHotelById(hotelId);
            Review silinecek = _context.Reviews.FirstOrDefault(r => r.ReviewId == reviewid);
            
            if (silinecek != null)
            {
                _context.Reviews.Remove(silinecek);
                _context.SaveChanges(); 
            }
            var model = Tuple.Create(otel, reviews,odalar);
            return View("detay",model);
        }


    }
}

