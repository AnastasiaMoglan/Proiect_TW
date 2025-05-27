using System.Web.Mvc;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransferService _transferService;

        public AdminController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public ActionResult Dashboard()
        {
            var transfers = _transferService.GetAllTransfers();
            ViewBag.A2ATransfers = transfers;
            return View();
        }
    }
}