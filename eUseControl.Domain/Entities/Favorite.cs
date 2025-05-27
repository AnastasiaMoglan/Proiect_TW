using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Favorite")]
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required] // E bine să marchezi cheile străine ca Required
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
        
    }
}