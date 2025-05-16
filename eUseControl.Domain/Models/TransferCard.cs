using System;

namespace eUseControl.Domain.Models
{
    public class TransferCard
    {
        public int Id { get; set; }
        public string BankSender { get; set; }
        public string SenderName { get; set; }
        public string CardSender { get; set; }
        public decimal Amount { get; set; }
        public string CardBeneficiary { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankBeneficiary { get; set; }
        public DateTime TransferDate { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
} 