using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Domain.Models;
using eUseControl.Web.Data;

namespace eUseControl.Web.Services
{
    public class TransferCardService
    {
        private readonly ApplicationDbContext _context;

        public TransferCardService()
        {
            _context = new ApplicationDbContext();
        }

        public List<TransferCard> GetAllTransfers()
        {
            return _context.TransferCards.ToList();
        }

        public TransferCard GetTransferById(int id)
        {
            return _context.TransferCards.Find(id);
        }

        public void CreateTransfer(TransferCard transfer)
        {
            _context.TransferCards.Add(transfer);
            _context.SaveChanges();
        }

        public void UpdateTransfer(TransferCard transfer)
        {
            var existingTransfer = _context.TransferCards.Find(transfer.Id);
            if (existingTransfer != null)
            {
                _context.Entry(existingTransfer).CurrentValues.SetValues(transfer);
                _context.SaveChanges();
            }
        }

        public void DeleteTransfer(int id)
        {
            var transfer = _context.TransferCards.Find(id);
            if (transfer != null)
            {
                _context.TransferCards.Remove(transfer);
                _context.SaveChanges();
            }
        }
    }
} 