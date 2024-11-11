using CashFlowApp.Domain.Enums;
using CashFlowApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Entities
{
    public class DailyBalance
    {
        public DateTime Date { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
        public Amount Balance => new Amount(TotalCredit - TotalDebit);

        public DailyBalance(DateTime date)
        {
            Date = date.Date;
            TotalCredit = 0;
            TotalDebit = 0;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Credito)
                TotalCredit += transaction.Amount;
            else
                TotalDebit += transaction.Amount;
        }
    }
}
