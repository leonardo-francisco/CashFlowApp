using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Interfaces;
using CashFlowApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Services
{
    public class BalanceService : IDailyBalanceService
    {
        private readonly ITransactionRepository _transactionRepository;

        public BalanceService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
       
        public async Task<DailyBalance> CalculateDailyBalanceAsync(DateTime date)
        {
            var transactions = await _transactionRepository.GetTransactionsByDateAsync(date);
            var dailyBalance = new DailyBalance(date);

            foreach (var transaction in transactions)
            {
                dailyBalance.AddTransaction(transaction);
            }

            return dailyBalance;
        }
    }
}
