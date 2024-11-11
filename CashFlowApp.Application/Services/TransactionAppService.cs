using AutoMapper;
using CashFlowApp.Application.Caching;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Application.Interfaces;
using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Events;
using CashFlowApp.Domain.Interfaces;
using CashFlowApp.Domain.Services;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Services
{
    public class TransactionAppService : ITransactionAppService
    {     
        private readonly TransactionService _transactionService;
        private readonly RedisCacheService _cacheService;
        private readonly IMediator _mediator;
        private readonly IDailyBalanceService _dailyBalanceService;
        private readonly IMapper _mapper;

        public TransactionAppService(TransactionService transactionService, RedisCacheService cacheService, IMediator mediator, IDailyBalanceService dailyBalanceService, IMapper mapper)
        {            
            _transactionService = transactionService;
            _cacheService = cacheService;
            _mediator = mediator;
            _dailyBalanceService = dailyBalanceService;
            _mapper = mapper;
        }

        public async Task<TransactionDTO> AddTransactionAsync(TransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            var transactionResult = await _transactionService.AddTransactionAsync(transaction);

            var transactionCreatedEvent = new TransactionCreatedEvent(
            transaction.Id,
            transaction.Amount,
            transaction.Date,
            transaction.Description
             );

            await _mediator.Publish(transactionCreatedEvent);

            var cacheKey = $"DailyBalance:{transaction.Date:yyyyMMdd}";
            await _cacheService.RemoveCacheAsync(cacheKey);

            var dailyBalance = await _dailyBalanceService.CalculateDailyBalanceAsync(transaction.Date);
           
            var dailyBalanceCacheValue = _mapper.Map<DailyBalanceDTO>(dailyBalance);
            await _cacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(dailyBalanceCacheValue));

            return _mapper.Map<TransactionDTO>(transactionResult);           
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactionsByDateAsync(DateTime date)
        {
            var cacheKey = $"Transactions:{date:yyyyMMdd}";
            var cachedTransactions = await _cacheService.GetCacheAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedTransactions))
            {              
                var transactions = JsonConvert.DeserializeObject<IEnumerable<Transaction>>(cachedTransactions);
                return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
            }

            var transactionsFromDb = await _transactionService.GetTransactionsByDateAsync(date);

            await _cacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(transactionsFromDb));

            return _mapper.Map<IEnumerable<TransactionDTO>>(transactionsFromDb);
        }
    }
}
