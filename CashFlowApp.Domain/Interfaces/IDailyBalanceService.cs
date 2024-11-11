using CashFlowApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Interfaces
{
    public interface IDailyBalanceService
    {
        Task<DailyBalance> CalculateDailyBalanceAsync(DateTime date);
    }
}
