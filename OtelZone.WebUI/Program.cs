using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Dac.Model;
using OtelZone.Business.HotelService;
using OtelZone.Business.UserService;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;

namespace OtelZone.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<OtelZoneContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IHotelService, HotelService>();


            builder.Services.AddTransient<IValidator<Entity.Entities.User>, UserValidator>();
            builder.Services.AddTransient<IValidator<Entity.Entities.Room>, RoomValidator>();
            builder.Services.AddTransient<IValidator<Entity.Entities.Hotel>,HotelValidator>();
         
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");




            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                // Ýsteðe baðlý olarak cookie ayarlarýný burada yapabilirsiniz.
                options.LoginPath = "/Account/Login";
            });



            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
                options.Cookie.HttpOnly = true; // Güvenlik için sadece HTTP
                options.Cookie.IsEssential = true; // GDPR uyumluluðu için gerekli
            });

            var app = builder.Build();


           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=anasayfa}/{action=index}/{id?}");

            app.Run();
        }
    }
}
