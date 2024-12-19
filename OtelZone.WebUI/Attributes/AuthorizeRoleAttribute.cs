using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;


namespace OtelZone.WebUI.Attributes
{
    public class AuthorizeRoleAttribute:ActionFilterAttribute
    {
        private readonly string[] _roles;  // Geçerli roller

        // Constructor ile roller parametre olarak alınır
        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;  // Roller filtreye parametre olarak atanır
        }

        // Action öncesinde çalışacak metod
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Kullanıcının rolünü session'dan alıyoruz
            var userRole = context.HttpContext.Session.GetString("Role");

            // Eğer rol yoksa veya kullanıcı belirtilen rollerden birine sahip değilse
            if (userRole == null || !_roles.Any(role => userRole.Contains(role)))
            {
                // Kullanıcıya yetkisi olmayan sayfaya erişim engelleniyor
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);  // Filtreyi çalıştırmaya devam et
        }



    }
}
