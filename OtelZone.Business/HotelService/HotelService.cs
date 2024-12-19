using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OtelZone.DataAccess.Context;
using OtelZone.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Business.HotelService
{
    public class HotelService:IHotelService
    {
        private readonly OtelZoneContext _context;  //Veritabanı erişimi

        public HotelService(OtelZoneContext context)
        {
            _context = context;
        }

        public async Task<bool> Otelkaydet(string HotelName, string HotelAdress, short Rating, string HotelDesc, string HotelFacilities, string HotelImgUrl, bool HotelIsActive, int CityId, int DistrictId)
        {

            // Hotel zaten var mı kontrol et
            var existingHotel = await _context.Hotels.FirstOrDefaultAsync(u => u.HotelAdress == HotelAdress);
           

            if (existingHotel != null)
            {
                return false;  // Hotel zaten mevcut
            }

            if (HotelAdress == null)
            {
                return false;

            }
        

            // Yeni hotel oluştur
            var hotel = new Hotel
            {
                HotelName = HotelName,
                HotelAdress = HotelAdress,
                Rating = Rating,
                HotelDesc = HotelDesc,
                HotelFacilities = HotelFacilities,  
                HotelImgUrl = HotelImgUrl,
                HotelCreatedDate = DateTime.Now,
                HotelIsActive = HotelIsActive,
                CityId = CityId,
                DistrictId = DistrictId
            };

            // Hoteli veritabanına kaydet
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return true;  // Kayıt başarılı
        }

        public async Task<Hotel> GetHotelById(int hotelid)
        {
            var selectedHotel = await _context.Selectedhotel(hotelid);
            return selectedHotel;
        }

        public async Task<List<Review>> GetReviewByHotelId(int hotelid)
        {
            var selectedreview =  _context.GetAllRewiews(hotelid);
            

            return (List<Review>)selectedreview;
        }



    }
}
