using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string ipAddress = Request.UserHostAddress;
                string userAgent = Request.UserAgent;

                bool isValid = _authService.ValidateCredentials(model.UsernameOrEmail, model.Password);
                _authService.RecordLoginAttempt(model.UsernameOrEmail, ipAddress, userAgent, isValid);

                if (!isValid)
                {
                    ModelState.AddModelError("", "Invalid username/email or password.");
                    return View(model);
                }

                var user = _authService.GetUserByUsernameOrEmail(model.UsernameOrEmail);
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Your account has been disabled. Please contact support.");
                    return View(model);
                }

                FormsAuthentication.SetAuthCookie(user.Id.ToString(), model.RememberMe);
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["UserRole"] = user.Role;

                return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home"));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (_authService.UsernameExists(model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                if (_authService.EmailExists(model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(model);
                }

                int userId = _authService.RegisterUser(model.Username, model.Email, model.Password);

                FormsAuthentication.SetAuthCookie(userId.ToString(), false);
                Session["UserId"] = userId;
                Session["Username"] = model.Username;
                Session["UserRole"] = "User";

                TempData["SuccessMessage"] = "Registration successful! Welcome to our site.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Profile()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var user = _authService.GetUserById(userId);

                if (user == null)
                    return RedirectToAction("Login");

                var model = new ProfileViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading your profile: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(ProfileViewModel model)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                _authService.UpdateUserProfile(userId, model.FirstName, model.LastName, model.Phone, model.Address);

                TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating your profile: " + ex.Message;
                return RedirectToAction("Profile");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                _authService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

                TempData["SuccessMessage"] = "Your password has been changed successfully.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while changing your password: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Normally: generate token, send email, etc.
            TempData["SuccessMessage"] = "If your email exists, you will receive a password reset link shortly.";
            return RedirectToAction("Login");
        }
    }
}
