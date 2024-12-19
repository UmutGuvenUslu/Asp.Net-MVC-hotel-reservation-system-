using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class RoomValidator: AbstractValidator<Room>
    {
        public RoomValidator() { 
        
        RuleFor(r=>r.RoomType).NotNull().WithMessage("Oda tipi boş bırakılamaz");
        RuleFor(r=>r.BedCount).NotNull().WithMessage("Yatak sayısı boş bırakılamaz");
        RuleFor(r=>r.Price).NotNull().WithMessage("Fiyat boş bırakılamaz");
        RuleFor(r => r.RoomIsAvilable).NotNull().WithMessage("Oda müsaitliği değeri boş bırakılamaz.").Must(value => value == true || value == false).WithMessage("Oda müsaitliği sadece 'true' veya 'false' değerlerini alabilir.");


        }


    }
}
