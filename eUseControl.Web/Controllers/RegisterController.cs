using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Web.Models;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        // Inject service via constructor (Dependency Injection)
        public RegisterController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
public RegisterController(){}
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (_userService.IsEmailRegistered(model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                if (_userService.IsUserRegistered(model.Username))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }

                var user = _userService.RegisterUser(model.Email, model.Username, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                    return View(model);
                }

                var authTicket = new FormsAuthenticationTicket(
                    version: 1,
                    name: user.Email,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddDays(7),
                    isPersistent: false,
                    userData: user.Role
                );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };

                Response.Cookies.Add(authCookie);

                // Store user session data
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;

                TempData["SuccessMessage"] = "Registration successful! You are now logged in.";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                System.Diagnostics.Debug.WriteLine("Registration Exception: " + ex);
                return View(model);
            }
        }
    }
}
