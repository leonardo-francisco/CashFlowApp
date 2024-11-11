using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Tests.Domain
{
    public class DailyBalanceTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithCorrectDateAndZeroBalances()
        {
            // Arrange
            var date = new DateTime(2024, 11, 7);

            // Act
            var dailyBalance = new DailyBalance(date);

            // Assert
            Assert.Equal(date.Date, dailyBalance.Date);
            Assert.Equal(0, dailyBalance.TotalCredit);
            Assert.Equal(0, dailyBalance.TotalDebit);
            Assert.Equal(0, dailyBalance.Balance.Value);
        }

        [Fact]
        public void AddTransaction_ShouldAddCreditTransactionCorrectly()
        {
            // Arrange
            var date = new DateTime(2024, 11, 7);
            var dailyBalance = new DailyBalance(date);
            var transaction = new Transaction(100, TransactionType.Credito, "Test Credit");

            // Act
            dailyBalance.AddTransaction(transaction);

            // Assert
            Assert.Equal(100, dailyBalance.TotalCredit);
            Assert.Equal(0, dailyBalance.TotalDebit);
            Assert.Equal(100, dailyBalance.Balance.Value);
        }

        [Fact]
        public void AddTransaction_ShouldAddDebitTransactionCorrectly()
        {
            // Arrange
            var date = new DateTime(2024, 11, 7);
            var dailyBalance = new DailyBalance(date);
            var transaction = new Transaction(50, TransactionType.Debito, "Test Debit");

            // Act
            dailyBalance.AddTransaction(transaction);

            // Assert
            Assert.Equal(0, dailyBalance.TotalCredit);
            Assert.Equal(50, dailyBalance.TotalDebit);
            Assert.Equal(-50, dailyBalance.Balance.Value);
        }

        [Fact]
        public void AddTransaction_ShouldAccumulateMultipleTransactionsCorrectly()
        {
            // Arrange
            var date = new DateTime(2024, 11, 7);
            var dailyBalance = new DailyBalance(date);
            var creditTransaction = new Transaction(200, TransactionType.Credito, "Test Credit");
            var debitTransaction = new Transaction(50, TransactionType.Debito, "Test Debit");

            // Act
            dailyBalance.AddTransaction(creditTransaction);
            dailyBalance.AddTransaction(debitTransaction);

            // Assert
            Assert.Equal(200, dailyBalance.TotalCredit);
            Assert.Equal(50, dailyBalance.TotalDebit);
            Assert.Equal(150, dailyBalance.Balance.Value);
        }
    }
}
