using CashFlowApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.EventHandlers
{
    public class TransactionEventHandler
    {
        public async Task HandleTransactionEvent(TransactionDTO transaction)
        {           
            Console.WriteLine($"Transaction {transaction.Id} processed.");
        }
    }
}
