using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using HeatWatch.API.DTOs;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Unit
{
    public class RegistroTemperaturaServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenRecordExists()
        {
            var rec = new RegistroTemperatura { Id = 2, RegiaoId = 1, DataRegistro = DateTime.UtcNow, TemperaturaCelsius = 36.6 };
            var mockRepo = new Mock<IRegistroTemperaturaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(rec);
            var service = new RegistroTemperaturaService(mockRepo.Object);

            var result = await service.GetByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistAndReturnDto()
        {
            var dto = new CreateRegistroTemperaturaDto { RegiaoId = 5, DataRegistro = DateTime.UtcNow, TemperaturaCelsius = 22.2 };
            var entity = new RegistroTemperatura { Id = 8, RegiaoId = dto.RegiaoId, DataRegistro = dto.DataRegistro, TemperaturaCelsius = dto.TemperaturaCelsius };
            var mockRepo = new Mock<IRegistroTemperaturaRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<RegistroTemperatura>())).ReturnsAsync(entity);
            var service = new RegistroTemperaturaService(mockRepo.Object);

            var result = await service.CreateAsync(dto);

            mockRepo.Verify(r => r.AddAsync(It.IsAny<RegistroTemperatura>()), Times.Once);
            Assert.Equal(8, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenRecordNotFound()
        {
            var mockRepo = new Mock<IRegistroTemperaturaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((RegistroTemperatura)null);
            var service = new RegistroTemperaturaService(mockRepo.Object);

            var ok = await service.UpdateAsync(3, new UpdateRegistroTemperaturaDto());

            Assert.False(ok);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepo_WhenRecordExists()
        {
            var rec = new RegistroTemperatura { Id = 6 };
            var mockRepo = new Mock<IRegistroTemperaturaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(6)).ReturnsAsync(rec);
            var service = new RegistroTemperaturaService(mockRepo.Object);

            var ok = await service.DeleteAsync(6);

            Assert.True(ok);
            mockRepo.Verify(r => r.DeleteAsync(6), Times.Once);
        }
    }
}
