using AutoMapper;
using CashFlowApp.API.Controllers;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Tests.Presentation
{
    public class DailyBalanceControllerTests
    {
        private readonly Mock<IDailyBalanceAppService> _mockDailyBalanceAppService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DailyBalanceController _controller;

        public DailyBalanceControllerTests()
        {
            // Criação dos mocks
            _mockDailyBalanceAppService = new Mock<IDailyBalanceAppService>();
            _mockMapper = new Mock<IMapper>();

            // Criação do controller com os mocks
            _controller = new DailyBalanceController(_mockDailyBalanceAppService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetDailyBalance_ShouldReturnOk_WhenBalanceExists()
        {
            // Arrange
            var date = DateTime.Now;
            var balanceDto = new DailyBalanceDTO { Date = date, Balance = 100.00m };

            // Configuração do mock para retornar um DailyBalanceDTO válido
            _mockDailyBalanceAppService.Setup(service => service.GetDailyBalanceAsync(date))
                .ReturnsAsync(balanceDto);

            // Act
            var result = await _controller.GetDailyBalance(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); 
            Assert.Equal(balanceDto, okResult.Value); 
        }

        [Fact]
        public async Task GetDailyBalance_ShouldReturnNoContent_WhenBalanceDoesNotExist()
        {
            // Arrange
            var date = DateTime.Now;

            // Configuração do mock para retornar null
            _mockDailyBalanceAppService.Setup(service => service.GetDailyBalanceAsync(date))
                .ReturnsAsync((DailyBalanceDTO)null);

            // Act
            var result = await _controller.GetDailyBalance(date);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result); 
        }
    }
}
