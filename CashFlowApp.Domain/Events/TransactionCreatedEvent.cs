using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Events
{
    public class TransactionCreatedEvent : INotification
    {
        public string TransactionId { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Description { get; }

        public TransactionCreatedEvent(string transactionId, decimal amount, DateTime date, string description)
        {
            TransactionId = transactionId;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}
