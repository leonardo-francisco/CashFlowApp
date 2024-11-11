using CashFlowApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Interfaces
{
    public interface IDailyBalanceAppService
    {
        Task<DailyBalanceDTO> GetDailyBalanceAsync(DateTime date);
    }
}
