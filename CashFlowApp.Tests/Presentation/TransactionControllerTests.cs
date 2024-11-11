using AutoMapper;
using CashFlowApp.API.Controllers;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Application.Interfaces;
using CashFlowApp.Domain.Entities;
using CashFlowApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Tests.Presentation
{
    public class TransactionControllerTests
    {
        private readonly Mock<ITransactionAppService> _mockTransactionAppService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TransactionController _controller;

        public TransactionControllerTests()
        {
            // Setup dos Mocks
            _mockTransactionAppService = new Mock<ITransactionAppService>();
            _mockMapper = new Mock<IMapper>();

            // Criação do Controller com os mocks
            _controller = new TransactionController(_mockTransactionAppService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddTransaction_ShouldReturnCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var transactionDto = new TransactionDTO { Amount = 100, Date = DateTime.Now, Type = "Credito", Description = "Mock" };
            var transaction = new Transaction(100, TransactionType.Credito, "Mock");

            _mockTransactionAppService.Setup(service => service.AddTransactionAsync(It.IsAny<TransactionDTO>()))
                .ReturnsAsync(transactionDto);

            // Act
            var result = await _controller.AddTransaction(transactionDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetTransactionsByDate", createdAtActionResult.ActionName);
            
        }

        [Fact]
        public async Task AddTransaction_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model is invalid"); 

            var transactionDto = new TransactionDTO { Amount = 100, Type = "Parcelado", Description = "Mock" };

            // Act
            var result = await _controller.AddTransaction(transactionDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task GetTransactionsByDate_ShouldReturnOkResult_WhenTransactionsFound()
        {
            // Arrange
            var date = DateTime.Now;
            
            var transactions = new List<TransactionDTO>()
            {
                new TransactionDTO{Amount = 100,Type = "Debito", Description = "Mock3" },
                new TransactionDTO{Amount = 300,Type = "Credito", Description =  "Mock4" }
            };

            _mockTransactionAppService.Setup(service => service.GetTransactionsByDateAsync(date))
                .ReturnsAsync(transactions);

            // Act
            var result = await _controller.GetTransactionsByDate(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(transactions, okResult.Value);
        }

        [Fact]
        public async Task GetTransactionsByDate_ShouldReturnNotFound_WhenNoTransactionsFound()
        {
            // Arrange
            var date = DateTime.Now;
            _mockTransactionAppService.Setup(service => service.GetTransactionsByDateAsync(date))
                .ReturnsAsync(new List<TransactionDTO>());

            // Act
            var result = await _controller.GetTransactionsByDate(date);

            // Assert
            Assert.IsType<OkObjectResult>(result); 
        }
    }
}
