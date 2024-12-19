using Microsoft.EntityFrameworkCore;
using OtelZone.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OtelZone.DataAccess.Context;



public class OtelZoneContext:DbContext
{
    public OtelZoneContext(DbContextOptions options): base(options)
    { 
    
    }

    public DbSet<User> Users {  get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Rezervation> Rezervations { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<City> Citys { get; set; }
    public DbSet<District> Districts { get; set; }

   
    public IEnumerable<Hotel> GetAllHotels() =>
        Hotels.Include(h => h.City).Include(h => h.District).Include(h=>h.Rooms).Include(h=>h.Reviews).ToList();


    public async Task<Hotel> Selectedhotel(int hotelid)
    {
        return await Hotels.Include(h => h.City)
                           .Include(h => h.District)
                           .Include(h => h.Rooms)
                           .Include(h => h.Reviews)
                           .FirstOrDefaultAsync(h => h.HotelId == hotelid);
    }


    public IEnumerable<District> GetDistrictsByCityId(int cityId) =>
     Districts.Include(d => d.City).Where(d => d.CityId == cityId).ToList();


    public IEnumerable<Review> GetAllRewiews(int hotelid) =>
       Reviews.Include(h => h.User).Include(h => h.Hotel).Where(h=>h.HotelId==hotelid).ToList();



}





