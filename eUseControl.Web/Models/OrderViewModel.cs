// eUseControl.Web/Models/OrderViewModel.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Status")]
        public string OrderStatus { get; set; }

        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Tracking Number")]
        public string TrackingNumber { get; set; }

        public List<OrderItemViewModel> Items { get; set; }

        public OrderViewModel()
        {
            Items = new List<OrderItemViewModel>();
        }

        public bool CanBeCancelled
        {
            get
            {
                return OrderStatus != "Shipped" && 
                       OrderStatus != "Delivered" && 
                       OrderStatus != "Cancelled";
            }
        }

        public bool HasTrackingNumber
        {
            get
            {
                return !string.IsNullOrEmpty(TrackingNumber);
            }
        }
    }
}