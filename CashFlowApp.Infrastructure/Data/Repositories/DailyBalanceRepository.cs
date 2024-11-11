using CashFlowApp.Domain.Entities;
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
    public class DailyBalanceRepository
    {
        private readonly CashFlowDbContext _context;
        private readonly TransactionRetryPolicy _retryPolicy;
        private readonly CircuitBreakerPolicyProvider _circuitBreakerPolicyProvider;
        private readonly LoggingService _loggingService;

        public DailyBalanceRepository(CashFlowDbContext context, TransactionRetryPolicy retryPolicy, CircuitBreakerPolicyProvider circuitBreakerPolicyProvider, LoggingService loggingService)
        {
            _context = context;
            _retryPolicy = retryPolicy;
            _circuitBreakerPolicyProvider = circuitBreakerPolicyProvider;
            _loggingService = loggingService;
        }

        public async Task SaveDailyBalanceAsync(DailyBalance dailyBalance)
        {            
            var retryPolicy = _retryPolicy.GetTransactionRetryPolicy();           
            var circuitBreakerPolicy = _circuitBreakerPolicyProvider.GetCircuitBreakerPolicy();

            try
            {
                _loggingService.LogInformation($"Tentando salvar o saldo diario para a data: {dailyBalance.Date.ToShortDateString()}.");

                await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        var filter = Builders<DailyBalance>.Filter.Eq(b => b.Date, dailyBalance.Date);
                        await _context.DailyBalances.ReplaceOneAsync(filter, dailyBalance, new ReplaceOptions { IsUpsert = true });
                        _loggingService.LogInformation($"Saldo diário por data: {dailyBalance.Date.ToShortDateString()} salvo com sucesso.");
                    });
                });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Erro ao salvar o saldo diario para a data: {dailyBalance.Date.ToShortDateString()}. Exception: {ex.Message}");
                throw;
            }
        }
    }
}
