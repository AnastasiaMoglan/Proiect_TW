// eUseControl.Web/Models/OrderItemViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class OrderItemViewModel
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Discount { get; set; }

        [Display(Name = "Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal 
        { 
            get 
            { 
                return (Price - Discount) * Quantity; 
            } 
        }
    }
}