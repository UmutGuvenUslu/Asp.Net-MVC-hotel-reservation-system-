using OtelZone.Entity.Entities;
using OtelZone.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

namespace OtelZone.Business.UserService
{
    public class UserService : IUserService
    {
        private readonly OtelZoneContext _context;  //Veritabanı erişimi

        public UserService(OtelZoneContext context)
        {
            _context = context;
        }

        // Kullanıcıyı doğrulama (login)
        public async Task<User> Authenticate(string email, string password)
        {
            var result= PasswordVerificationResult.Failed;
            // Kullanıcıyı email ile bul
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email);

            if (user == null)
            {
                return null;  // Kullanıcı bulunamadı
            }

            // PasswordHasher ile şifreyi doğrula
            if (password != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                 result = passwordHasher.VerifyHashedPassword(user, user.UserPassword, password);
            }
            else
            {
                 result = PasswordVerificationResult.Failed;
            }
            if (result == PasswordVerificationResult.Failed)
            {
                return null;  // Şifre yanlış
            }

            return user;  // Kullanıcı doğrulandı
        }
        // Yeni kullanıcı kaydı 
        public async Task<bool> Register(string username, string usersurname, string useremail, string userphone, string password)
        {

            // Kullanıcı zaten var mı kontrol et
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == useremail);
            var result = PasswordVerificationResult.Failed;

            if (existingUser != null)
            {
                return false;  // Kullanıcı zaten mevcut
            }

            if (password == null)
            {
                return false;

            }
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, password);

            // Yeni kullanıcı oluştur
            var user = new User
            {
                UserName = username,
                UserSurname = usersurname,
                UserEmail = useremail,
                UserPhone = userphone,
                UserPassword = hashedPassword,  // Hash'lenmiş şifreyi kaydet
                UserRole = "Müşteri"
            };

            // Kullanıcıyı veritabanına kaydet
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;  // Kayıt başarılı
        }

        // Kullanıcıyı ID ile bulma
        public async Task<User> GetUserByEmail(string useremail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == useremail);
        }
    }

}
