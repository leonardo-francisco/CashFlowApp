using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Interfaces;
using CashFlowApp.Infrastructure.Data.DbContexts;
using CashFlowApp.Infrastructure.Logging;
using CashFlowApp.Infrastructure.Resilience;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CashFlowDbContext _context;
        private readonly TransactionRetryPolicy _retryPolicy;
        private readonly CircuitBreakerPolicyProvider _circuitBreakerPolicyProvider;
        private readonly LoggingService _loggingService;

        public TransactionRepository(CashFlowDbContext context, TransactionRetryPolicy retryPolicy, CircuitBreakerPolicyProvider circuitBreakerPolicyProvider, LoggingService loggingService)
        {
            _context = context;
            _retryPolicy = retryPolicy;
            _circuitBreakerPolicyProvider = circuitBreakerPolicyProvider;
            _loggingService = loggingService;
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {           
            var retryPolicy = _retryPolicy.GetTransactionRetryPolicy();          
            var circuitBreakerPolicy = _circuitBreakerPolicyProvider.GetCircuitBreakerPolicy();

            try
            {
                return await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    return await retryPolicy.ExecuteAsync(async () =>
                    {
                        _loggingService.LogInformation($"Tentativa de adicionar transacao");
                        await _context.Transactions.InsertOneAsync(transaction);
                        _loggingService.LogInformation($"Transacao com ID: {transaction.Id} adicionada com sucesso.");
                        return transaction;
                    });
                });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Erro ao adicionar transacao com ID: {transaction.Id}. Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(DateTime date)
        {           
            var retryPolicy = _retryPolicy.GetTransactionRetryPolicy();           
            var circuitBreakerPolicy = _circuitBreakerPolicyProvider.GetCircuitBreakerPolicy();

            try
            {
                return await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    return await retryPolicy.ExecuteAsync(async () =>
                    {
                        _loggingService.LogInformation($"Buscando transacoes por data: {date.ToShortDateString()}");
                        var startOfDay = date.Date;
                        var endOfDay = date.Date.AddDays(1).AddTicks(-1);

                        var filter = Builders<Transaction>.Filter.And(
                            Builders<Transaction>.Filter.Gte(t => t.Date, startOfDay),
                            Builders<Transaction>.Filter.Lte(t => t.Date, endOfDay)
                        );

                        var transactions = await _context.Transactions.Find(filter).ToListAsync();
                        _loggingService.LogInformation($"Encontrado {transactions.Count} transacoes para a data: {date.ToShortDateString()}.");
                        return transactions;
                    });
                });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Erro ao buscar transacoes por data: {date.ToShortDateString()}. Exception: {ex.Message}");
                throw;
            }
        }
    }
}
