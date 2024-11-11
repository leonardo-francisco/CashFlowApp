using CashFlowApp.Infrastructure.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.Resilience
{
    public class TransactionRetryPolicy
    {
        private readonly LoggingService _loggingService;

        public TransactionRetryPolicy(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public AsyncRetryPolicy GetTransactionRetryPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         (exception, timeSpan, retryCount, context) =>
                         {
                             _loggingService.LogInformation($"Retry #{retryCount} devido a: {exception.Message}. Tentando novamente em {timeSpan.TotalSeconds} segundos.");
                         });
        }
    }
}
