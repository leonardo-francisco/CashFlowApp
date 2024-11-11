using CashFlowApp.Application.Caching;
using CashFlowApp.Application.Interfaces;
using CashFlowApp.Application.Services;
using CashFlowApp.Domain.Interfaces;
using CashFlowApp.Domain.Services;
using CashFlowApp.Infrastructure.Data.DbContexts;
using CashFlowApp.Infrastructure.Data.Repositories;
using CashFlowApp.Infrastructure.Logging;
using CashFlowApp.Infrastructure.Mappings;
using CashFlowApp.Infrastructure.MessageQueue;
using CashFlowApp.Infrastructure.Resilience;
using CashFlowApp.Infrastructure.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton(new CashFlowDbContext(mongoSettings.ConnectionString, mongoSettings.DatabaseName));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IDailyBalanceService, BalanceService>();
            services.AddScoped<ITransactionAppService, TransactionAppService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<IDailyBalanceAppService, DailyBalanceAppService>();

            // Configuração de Cache (Redis)           
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection") + ",connectTimeout=10000";
                options.InstanceName = "cash-CashFlow";
            });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));           
            services.AddScoped<RedisCacheService>();

            services.AddMediatR(typeof(TransactionAppService).Assembly);

            // Add Mapper
            services.AddAutoMapper(typeof(TransactionMapping));

            // Logging (Serilog)
            services.AddSingleton<LoggingService>();

            // Criptografia
            services.AddSingleton<EncryptionService>();

            // RabbitMQ
            services.AddSingleton<RabbitMQProducer>();

            // Política de Resiliência (Polly)
            services.AddSingleton<CircuitBreakerPolicyProvider>();
            services.AddSingleton<TransactionRetryPolicy>();
            services.AddHttpClient("CashFlowHttpClient")
               .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3)) 
               .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        }
    }
}
