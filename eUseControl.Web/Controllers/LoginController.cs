using System;
using System.Web.Mvc;
using eUseControl.Data.Services;
using eUseControl.Domain.Entities;
using eUseControl.Web.Models;
using System.Web.Security;
using System.Web;
using System.Net;

namespace eUseControl.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        
        public LoginController()
        {
            _userService = new UserService();
            EnsureAdminExists();
        }
        
        private void EnsureAdminExists()
        {
            try
            {
                if (!_userService.IsEmailRegistered("admin@admin"))
                {
                    _userService.RegisterUser("admin@admin", "Administrator", "admin1");
                    
                    var adminUser = _userService.ValidateUser("admin@admin", "admin1");
                    if (adminUser != null)
                    {
                        adminUser.Role = "Admin";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ensuring admin exists: " + ex.ToString());
            }
        }
        
        private string GetClientIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        ipAddress = client.DownloadString("https://api.ipify.org");
                    }
                }
                catch
                {
                    ipAddress = "Unknown";
                }
            }

            return ipAddress;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = Request.Cookies["UserAuth"];
                try
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        if (ticket.UserData == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (ArgumentException)
                {
                    var expiredCookie = new HttpCookie("UserAuth", "")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };
                    Response.Cookies.Add(expiredCookie);
                }
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

            try
            {
                var user = _userService.ValidateUser(model.Email, model.Password);
                
                _userService.RecordLogin(model.Email, GetClientIPAddress(), user != null);
                
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                var authTicket = new FormsAuthenticationTicket(
                    1,                              
                    user.Email,                    
                    DateTime.Now,                   
                    DateTime.Now.AddDays(7),        
                    model.RememberMe,               
                    user.Role                       
                );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie("UserAuth", encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                
                if (model.RememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(7);
                }

                Response.Cookies.Add(authCookie);

                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.Username;
                Session["UserRole"] = user.Role;

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Login Error: " + ex.ToString());
                return View(model);
            }
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            if (Request.Cookies["UserAuth"] != null)
            {
                var authCookie = new HttpCookie("UserAuth", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(authCookie);
            }

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult ViewLoginHistory()
        {
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var loginRecords = _userService.GetLoginHistory();
                ViewBag.LastUpdated = DateTime.Now;
                return View(loginRecords);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while accessing the login history.";
                ViewBag.DetailedError = ex.Message;
                return View(new System.Collections.Generic.List<eUseControl.Domain.Models.LoginRecord>());
            }
        }
    }
}