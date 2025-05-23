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
                    1,                              
                    user.Email,                     
                    DateTime.Now,                  
                    DateTime.Now.AddDays(7),     
                    false,                        
                    user.Role                       
                );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };

                Response.Cookies.Add(authCookie);

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