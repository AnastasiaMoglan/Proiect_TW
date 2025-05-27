using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal? DiscountedPrice { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public string? Gender { get; set; }

        public string? VisionType { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}