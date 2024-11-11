using AutoMapper;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Application.Interfaces;
using CashFlowApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Services
{
    public class DailyBalanceAppService : IDailyBalanceAppService
    {
        private readonly IDailyBalanceService _dailyBalanceService;
        private readonly IMapper _mapper;

        public DailyBalanceAppService(IDailyBalanceService dailyBalanceService, IMapper mapper)
        {
            _dailyBalanceService = dailyBalanceService;
            _mapper = mapper;
        }

        public async Task<DailyBalanceDTO> GetDailyBalanceAsync(DateTime date)
        {
            var dailyBalance = await _dailyBalanceService.CalculateDailyBalanceAsync(date);
            return _mapper.Map<DailyBalanceDTO>(dailyBalance);
        }
    }
}
