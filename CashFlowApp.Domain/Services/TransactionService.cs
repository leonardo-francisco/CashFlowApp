using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Services
{
    public class TransactionService : ITransactionRepository
    {
        private readonly ITransactionRepository _transactionRepository;      

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            var result = await _transactionRepository.AddTransactionAsync(transaction);
            return result;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(DateTime date)
        {
            return await _transactionRepository.GetTransactionsByDateAsync(date);
        }
    }
}
