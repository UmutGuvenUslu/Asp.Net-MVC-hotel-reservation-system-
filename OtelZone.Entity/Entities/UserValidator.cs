using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u=>u.UserName).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.").Length(3,80).WithMessage("Geçersiz kullanıcı adı girişi uzunluğu").Matches(@"^(?!\s*$)([A-Za-zÇçĞğÖöŞşİıÜü]+(\s[A-Za-zÇçĞğÖöŞşİıÜü]+)*)$").WithMessage("Kullanıcı adı yalnızca harfler ve bir veya daha fazla kelimeden oluşan boşluklarla ayrılmış ifadeler içerebilir.");

            RuleFor(u => u.UserSurname).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.").Length(2, 40).WithMessage("Fazla uzun kullanıcı soyadı girişi");
            RuleFor(u => u.UserEmail).NotEmpty().WithMessage("Email boş bırakılamaz.").EmailAddress().WithMessage("Geçersiz Email girişi");
            RuleFor(u=>u.UserPhone).NotEmpty().WithMessage("Telefon numarası boş olamaz.").Matches(@"^5\d{9}$").WithMessage("Telefon numarası geçerli bir numara olmalıdır. (Örn: 51234567890)");
            RuleFor(u => u.UserPassword).NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.");


        }



    }
}
