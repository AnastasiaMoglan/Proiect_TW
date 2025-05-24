using System.Web.Mvc;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            var model = new ContactViewModel
            {
                CompanyAddress = "123 Eye Care Street, Cityville, ST 12345",
                CompanyPhone = "(123) 456-7890",
                CompanyEmail = "info@eyecare.com"
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Thank you for contacting us. We will get back to you soon.";
                return RedirectToAction("Index");
            }
            
            model.CompanyAddress = "123 Eye Care Street, Cityville, ST 12345";
            model.CompanyPhone = "(123) 456-7890";
            model.CompanyEmail = "info@eyecare.com";
            
            return View(model);
        }
    }
}