using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    using FluentValidation;

    public class HotelValidator : AbstractValidator<Hotel>
    {
        public HotelValidator()
        {
            // HotelName: Boş olamaz, en az 3 karakter, en fazla 100 karakter.
            RuleFor(h => h.HotelName)
                .NotEmpty().WithMessage("Otel adı boş bırakılamaz.")
                .Length(3, 100).WithMessage("Otel adı 3 ile 100 karakter arasında olmalıdır.");

            // HotelAdress: Boş olamaz, maksimum 200 karakter.
            RuleFor(h => h.HotelAdress)
                .NotEmpty().WithMessage("Otel adresi boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Otel adresi en fazla 200 karakter olmalıdır.");

            // Rating: 1 ile 5 arasında bir değer olmalı.
            RuleFor(h => h.Rating)
                .InclusiveBetween((short)1, (short)5).WithMessage("Otel derecesi 1 ile 5 arasında olmalıdır.");

            // HotelDesc: Opsiyonel, maksimum 500 karakter.
            RuleFor(h => h.HotelDesc)
                .MaximumLength(800).WithMessage("Otel açıklaması en fazla 800 karakter olmalıdır.");

            // HotelFacilities: Opsiyonel, maksimum 300 karakter.
            RuleFor(h => h.HotelFacilities)
                .MaximumLength(300).WithMessage("Otel olanakları en fazla 300 karakter olmalıdır.");

            // HotelImgUrl: Opsiyonel, URL formatında olmalı.
            RuleFor(h => h.HotelImgUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                .When(h => !string.IsNullOrEmpty(h.HotelImgUrl))
                .WithMessage("Geçerli bir resim URL'si giriniz.");

            // HotelIsActive: Boş olamaz, zaten bool türünde olduğundan başka kontrol gerekmez.
            RuleFor(h => h.HotelIsActive)
                .NotNull().WithMessage("Otel aktif durumu boş olamaz.");
        }
    }


}

