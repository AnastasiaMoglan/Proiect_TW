using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Interfaces
{
    public interface ITransferRepository
    {
        List<TransferCard> GetAllTransfersOrderedByDateDesc();
    }
}