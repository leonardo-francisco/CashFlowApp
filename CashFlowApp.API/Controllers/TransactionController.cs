using AutoMapper;
using CashFlowApp.API.Models;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionAppService transactionAppService, IMapper mapper)
        {
            _transactionAppService = transactionAppService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var insertedTransaction = await _transactionAppService.AddTransactionAsync(request);

            return CreatedAtAction(nameof(GetTransactionsByDate), new { date = insertedTransaction.Date }, insertedTransaction);
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetTransactionsByDate(DateTime date)
        {
            var transactions = await _transactionAppService.GetTransactionsByDateAsync(date);
            return Ok(transactions);
        }
    }
}
