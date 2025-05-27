using System.Web.Mvc;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Web.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace eUseControl.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransferService _transferService;
        private readonly IOrderService _orderService;

        public AdminController(ITransferService transferService, IOrderService orderService)
        {
            _transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public ActionResult Dashboard()
        {
            var transfers = _transferService.GetAllTransfers();
            ViewBag.A2ATransfers = transfers;
            
            ViewBag.RecentOrders = _orderService.GetRecentOrders(5); // Get 5 most recent orders
            
            return View();
        }

    }
}