using CashFlowApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Interfaces
{
    public interface ITransactionAppService
    {
        Task<TransactionDTO> AddTransactionAsync(TransactionDTO transactionDto);
        Task<IEnumerable<TransactionDTO>> GetTransactionsByDateAsync(DateTime date);
    }
}
