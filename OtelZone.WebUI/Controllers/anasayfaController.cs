using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OtelZone.Business.UserService;
using OtelZone.DataAccess.Context;

namespace OtelZone.WebUI.Controllers
{
    public class anasayfaController : Controller
    {
        private readonly OtelZoneContext _context;
        private readonly IUserService _userService;

        public anasayfaController(OtelZoneContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public IActionResult index()
        {
            ViewData["Title"] = "Ana Sayfa";
            ViewBag.UyeGiris = TempData["YeniUye"];
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            return View();
        }

        public IActionResult kullanimkosullari()
        {
            ViewData["Title"] = "Kullanım Koşulları";


            return View();
        }

        public IActionResult gizlilikpolitikasi()
        {
            ViewData["Title"] = "Gizlilik Politikası";


            return View();
        }

        public IActionResult hakkımızda()
        {
            ViewData["Title"] = "Hakkımızda";

          
            return View();
        }
    }
}
