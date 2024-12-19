using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Entity.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Soyisim boş bırakılamaz.")]
        public string UserSurname { get; set; }
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Telefon numarası boş bırakılamaz.")]
        public string UserPhone { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string UserPassword  { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string UserRole { get; set; } = "Müşteri";

       public bool UserIsActive { get; set; } = true;

        public ICollection<Rezervation> Rezervations { get; set; }

    }
}
