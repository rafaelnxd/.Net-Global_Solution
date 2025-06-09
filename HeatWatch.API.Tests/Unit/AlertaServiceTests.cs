using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Unit
{
    public class AlertaServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenAlertaExists()
        {
            // Arrange
            var mockRepo = new Mock<IAlertaRepository>();
            var alerta = new Alerta
            {
                Id = 5,
                Mensagem = "Teste alerta",
                DataEmissao = DateTime.UtcNow,
                Severidade = "2",
                EventoCalorId = 3
            };
            mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(alerta);
            var service = new AlertaService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("Teste alerta", result.Mensagem);
            Assert.Equal("2", result.Severidade);
            Assert.Equal(3, result.EventoCalorId);
        }

        [Fact]
        public async Task CreateAsync_CallsRepoAndReturnsDto()
        {
            // Arrange
            var dto = new DTOs.CreateAlertaDto
            {
                Mensagem = "Nova mensagem",
                DataEmissao = DateTime.UtcNow,
                Severidade = "2",
                EventoCalorId = 7
            };
            var entity = new Alerta
            {
                Id = 10,
                Mensagem = dto.Mensagem,
                DataEmissao = dto.DataEmissao,
                Severidade = dto.Severidade,
                EventoCalorId = dto.EventoCalorId
            };
            var mockRepo = new Mock<IAlertaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Alerta>())).ReturnsAsync(entity);
            var service = new AlertaService(mockRepo.Object);

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Alerta>(a =>
                a.Mensagem == dto.Mensagem &&
                a.Severidade == dto.Severidade &&
                a.EventoCalorId == dto.EventoCalorId
            )), Times.Once);
            Assert.Equal(10, result.Id);
            Assert.Equal(dto.Mensagem, result.Mensagem);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IAlertaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Alerta)null);
            var service = new AlertaService(mockRepo.Object);

            // Act
            var ok = await service.DeleteAsync(1);

            // Assert
            Assert.False(ok);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepo_WhenExists()
        {
            // Arrange
            var alerta = new Alerta { Id = 4 };
            var mockRepo = new Mock<IAlertaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(alerta);
            var service = new AlertaService(mockRepo.Object);

            // Act
            var ok = await service.DeleteAsync(4);

            // Assert
            Assert.True(ok);
            mockRepo.Verify(r => r.DeleteAsync(4), Times.Once);
        }
    }
}