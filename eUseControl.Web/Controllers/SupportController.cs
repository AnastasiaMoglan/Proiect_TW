using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.Domain.Models;
using eUseControl.Web.Data;
using System.Text;

namespace eUseControl.Web.Controllers
{
    public class SupportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportController()
        {
            _context = new ApplicationDbContext();
        }

        private void EnsureSessionFromAuthCookie()
        {
            if (Session["UserEmail"] == null && Request.Cookies["UserAuth"] != null)
            {
                try
                {
                    var authCookie = Request.Cookies["UserAuth"];
                    var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired)
                    {
                        var userEmail = ticket.Name;
                        using (var context = new eUseControl.Web.Data.ApplicationDbContext())
                        {
                            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
                            if (user != null)
                            {
                                Session["UserId"] = user.Id;
                                Session["UserEmail"] = user.Email;
                                Session["UserName"] = user.Username;
                                Session["UserRole"] = user.Role;
                            }
                        }
                    }
                }
                catch { }
            }
        }

        [HttpPost]
        public JsonResult SaveMessage(string message)
        {
            EnsureSessionFromAuthCookie();
            var debugInfo = new StringBuilder();
            debugInfo.AppendLine($"Session[UserEmail]: {Session["UserEmail"]}");
            debugInfo.AppendLine($"Cookie[UserAuth]: {(Request.Cookies["UserAuth"] != null ? "EXISTS" : "MISSING")}");
            debugInfo.AppendLine($"Session[UserName]: {Session["UserName"]}");
            debugInfo.AppendLine($"Session[UserRole]: {Session["UserRole"]}");
            debugInfo.AppendLine($"Session[UserPhone]: {Session["UserPhone"]}");

            if (Session["UserEmail"] == null)
            {
                return Json(new { success = false, message = "Utilizator neautentificat", debug = debugInfo.ToString() });
            }

            try
            {
                var supportMessage = new SupportTable
                {
                    UserName = Session["UserName"].ToString(),
                    UserEmail = Session["UserEmail"].ToString(),
                    PhoneNumber = Session["UserPhone"]?.ToString() ?? "N/A",
                    Message = message,
                    CreatedAt = DateTime.Now
                };

                _context.SupportTables.Add(supportMessage);
                _context.SaveChanges();

                return Json(new { success = true, debug = debugInfo.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Eroare la salvarea mesajului", debug = debugInfo.ToString() });
            }
        }

        public ActionResult ViewSupportMessages()
        {
            EnsureSessionFromAuthCookie();
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var messages = _context.SupportTables
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public ActionResult CreateSupportMessage(SupportTable model)
        {
            if (ModelState.IsValid)
            {
                _context.SupportTables.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ViewSupportMessages");
            }
            return View(model);
        }
    }
} 