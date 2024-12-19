using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtelZone.Business.UserService;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;
using OtelZone.WebUI.Attributes;
using System.Security.Claims;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;



namespace OtelZone.WebUI.Controllers
{
    public class GirisController : Controller
    {


        private readonly IValidator<User> _validator;

        private readonly OtelZoneContext _context;
        private readonly IUserService _userService;

        public GirisController(OtelZoneContext context, IUserService userService, IValidator<User>? validator)
        {
            _context = context;
            _userService = userService;
            _validator = validator;

        }

        // Giriş sayfası (GET isteği)
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Giriş Yap";
            return View();  // Login.cshtml sayfasını döndürüyoruz
        }


        // Kullanıcı giriş işlemi
        [HttpPost]

        public async Task<IActionResult> Index(User gelen)
        {
            ViewData["Title"] = "Giriş Yap";
            ViewBag.Hata = false;
            TempData["YeniUye"] = false;
            var user = await _userService.Authenticate(gelen.UserEmail, gelen.UserPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
                ViewBag.Hata = true;
                return View();
            }

            // Kimlik doğrulama işlemleri
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Surname, user.UserSurname),
            new Claim(ClaimTypes.Role, user.UserRole)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetString("Role", user.UserRole);

            TempData["YeniUye"] = true;
            return RedirectToAction("index", "anasayfa");
        }



        public async Task<IActionResult> CikisYap()
        {
            // Kullanıcının oturumunu kapatma
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();  // Oturum verilerini temizle


            // Çıkış yaptıktan sonra ana sayfaya yönlendirme
            return RedirectToAction("index", "anasayfa");
        }









        //Üye Ol
        [HttpGet]
        public IActionResult uyeol() {

            ViewData["Title"] = "Kayıt Ol";
            User kullanici = new User();
      
        return View();
        }


        [HttpPost]
        public async Task<IActionResult> uyeOl(User gelen)
        {
            ViewData["Title"] = "Kayıt Ol";
            ViewBag.Hata = false;
            var validationResult = _validator.Validate(gelen);

            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View(gelen);
            }
          

        
            var success = await _userService.Register(gelen.UserName, gelen.UserSurname, gelen.UserEmail, gelen.UserPhone, gelen.UserPassword);

            return RedirectToAction("index", "anasayfa");
        }

        


        //Üye Listele
        [AuthorizeRole("Admin")]
        public IActionResult uyelistele() {

            ViewData["Title"] = "Kullanıcı Listesi";
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            var users = _context.Users.ToList();

            return View(users);
        
        }

        //Kullanıcı Sil
        [AuthorizeRole("Admin")]
        public IActionResult kullanicisil(string email)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            User silinecek = _context.Users.FirstOrDefault(a => a.UserEmail == email);
            if (silinecek != null)
            {
                _context.Users.Remove(silinecek);
                _context.SaveChanges(); // Değişikliği kaydet
            }
            return RedirectToAction("uyelistele");
        }

        //Kullanıcı Düzenle
        [AuthorizeRole("Admin")]
        [HttpGet]
        public IActionResult kullaniciduzenle(string email)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Kullanıcı Düzenle";
            User duzenle = _context.Users.FirstOrDefault(a => a.UserEmail == email);
            return View(duzenle);
        }




        [AuthorizeRole("Admin")]
        [HttpPost]
        public IActionResult kullaniciduzenle(User kullanıcı)
        {
            var cities = _context.Citys.ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name");
            ViewData["Title"] = "Kullanıcı Düzenle";
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, kullanıcı.UserPassword);
            User duzenle = _context.Users.FirstOrDefault(a => a.UserEmail == kullanıcı.UserEmail);
            duzenle.UserName =kullanıcı.UserName ;
            duzenle.UserSurname=kullanıcı.UserSurname;
            duzenle.UserPassword = hashedPassword;
            duzenle.UserRole = kullanıcı.UserRole;
            duzenle.UserEmail = kullanıcı.UserEmail;
            _context.SaveChanges();


            return RedirectToAction("uyelistele");
        }










    }
}
