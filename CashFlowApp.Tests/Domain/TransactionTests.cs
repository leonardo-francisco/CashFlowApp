using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Tests.Domain
{
    public class TransactionTests
    {
        [Fact]
        public void Should_Apply_Valid_Transaction()
        {
            // Arrange
            var transaction = new Transaction(100, TransactionType.Credito, "Transação 1" );
            var dailyBalance = new DailyBalance(DateTime.Now);

            // Act
            dailyBalance.AddTransaction(transaction);

            // Assert
            Assert.Equal(100, dailyBalance.TotalCredit);
        }
    }
}
