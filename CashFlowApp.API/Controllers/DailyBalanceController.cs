using AutoMapper;
using CashFlowApp.API.Models;
using CashFlowApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyBalanceController : ControllerBase
    {
        private readonly IDailyBalanceAppService _dailyBalanceAppService;
        private readonly IMapper _mapper;

        public DailyBalanceController(IDailyBalanceAppService dailyBalanceAppService, IMapper mapper)
        {
            _dailyBalanceAppService = dailyBalanceAppService;
            _mapper = mapper;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetDailyBalance(DateTime date)
        {
            var balanceDto = await _dailyBalanceAppService.GetDailyBalanceAsync(date);

            if (balanceDto == null)
                return NoContent();
           
            return Ok(balanceDto);
        }
    }
}
