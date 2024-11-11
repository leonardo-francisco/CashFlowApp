using Polly.CircuitBreaker;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlowApp.Infrastructure.Logging;

namespace CashFlowApp.Infrastructure.Resilience
{
    public class CircuitBreakerPolicyProvider
    {
        private readonly LoggingService _loggingService;

        public CircuitBreakerPolicyProvider(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy()
        {
            return Policy.Handle<Exception>()
                         .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1), 
                onBreak: (exception, timespan) =>
                {
                    _loggingService.LogError($"Disjuntor aberto devido a: {exception.Message}. Pausa para {timespan.TotalSeconds} segundos.");
                },
                onReset: () =>
                {
                    _loggingService.LogInformation("Reinicialização do disjuntor.");
                });
        }
    }
}
