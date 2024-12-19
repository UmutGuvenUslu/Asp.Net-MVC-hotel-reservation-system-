using OtelZone.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Business.HotelService
{
    public interface IHotelService
    {


        Task<bool>  Otelkaydet(string HotelName,string HotelAdress,short Rating,string HotelDesc,string HotelFacilities,string HotelImgUrl,bool HotelIsActive,int CityId,int DistrictId);
        Task<Hotel> GetHotelById(int hotelid);
        Task<List<Review>> GetReviewByHotelId(int hotelid);
    }
}
