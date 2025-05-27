using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        
        [StringLength(500)]
        public string ShippingAddress { get; set; }
        
        [StringLength(500)]
        public string BillingAddress { get; set; }
        
        [StringLength(50)]
        public string Phone { get; set; }
        
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";
        
        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; } = "Processing";
        
        [StringLength(100)]
        public string TrackingNumber { get; set; }
        
        [StringLength(1000)]
        public string Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        [NotMapped]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}