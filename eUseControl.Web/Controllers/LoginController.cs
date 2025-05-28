using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.Services;
using eUseControl.Data.Repository;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly AuthenticationService _authService;

        public LoginController()
        {
            // Instanțiere manuală a dependenței (fără Unity)
            var userRepository = new UserRepository();
            _authService = new AuthenticationService(userRepository);
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_authService.IsUserLocked(model.UsernameOrEmail))
            {
                ModelState.AddModelError("", "This account is locked due to too many failed attempts. Please contact support.");
                return View(model);
            }

            var user = _authService.ValidateUser(model.UsernameOrEmail, model.Password);

            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddDays(model.RememberMe ? 7 : 1),
                    model.RememberMe,
                    user.Role
                );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                if (model.RememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(7);
                }

                Response.Cookies.Add(authCookie);

                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username/email or password");
            return View(model);
        }
    }
}
