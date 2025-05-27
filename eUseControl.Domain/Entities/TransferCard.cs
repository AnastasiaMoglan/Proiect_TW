using System;

namespace eUseControl.Domain.Entities
{
    public class TransferCard
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        // Adaugă toate proprietățile necesare pentru TransferCard
    }
}