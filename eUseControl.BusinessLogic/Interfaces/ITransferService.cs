using System.Collections.Generic;

// asigură-te că TransferCard e aici

namespace eUseControl.BusinessLogic.Services
{
    public interface ITransferService
    {
        List<Domain.Entities.TransferCard> GetAllTransfers();
    }
}