using System;
using System.Web.Mvc;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class MedicalConsultationController : Controller
    {
        public ActionResult Index()
        {
            var model = new MedicalConsultationViewModel
            {
                PreferredDate = DateTime.Now.AddDays(1)
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MedicalConsultationViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Your consultation request has been submitted successfully. We will contact you shortly.";
                return RedirectToAction("Index");
            }
            
            return View(model);
        }
    }
}