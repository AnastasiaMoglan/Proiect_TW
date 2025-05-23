using System;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Web.Models;
using System.Web.Security;
using System.Web;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        
        public RegisterController()
        {
            _userService = new UserService();
        }
        
        // GET: Register
        public ActionResult Index()
        {
            // If user is already logged in, redirect to home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(new RegisterViewModel());
        }
        
        // POST: Register
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
                // Check if email is already registered
                if (_userService.IsEmailRegistered(model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }
                
                // Check if username is already taken
                if (_userService.IsUserRegistered(model.Username))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }
                
                // Register the user
                var user = _userService.RegisterUser(model.Email, model.Username, model.Password);
                
                if (user == null)
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                    return View(model);
                }
                
                // Record successful registration
                _userService.RecordLogin(user.Email, Request.UserHostAddress, true);
                
                // Create authentication ticket
                var authTicket = new FormsAuthenticationTicket(
                    1,                              // Version
                    user.Email,                     // User name
                    DateTime.Now,                   // Issue time
                    DateTime.Now.AddDays(7),        // Expiration time (7 days)
                    false,                          // Persistent
                    user.Role                       // User data (role)
                );

                // Encrypt the ticket
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                // Create the cookie
                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };

                // Add the cookie to the response
                Response.Cookies.Add(authCookie);

                // Set session variables
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;
                
                TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Registration Error: " + ex.ToString());
                return View(model);
            }
        }
    }
}