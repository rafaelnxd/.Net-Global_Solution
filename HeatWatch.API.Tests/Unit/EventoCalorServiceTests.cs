using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Unit
{
    public class EventoCalorServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenEventExists()
        {
            // Arrange
            var mockRepo = new Mock<IEventoCalorRepository>();
            var ev = new EventoCalor
            {
                Id = 3,
                Nome = "Calorão",
                DataInicio = new DateTime(2025, 6, 1),
                DataFim = new DateTime(2025, 6, 5),
                Intensidade = 8,
                RegiaoId = 2
            };
            mockRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(ev);
            var service = new EventoCalorService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("Calorão", result.Nome);
            Assert.Equal(8, result.Intensidade);
            Assert.Equal(2, result.RegiaoId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenEventNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IEventoCalorRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((EventoCalor)null);
            var service = new EventoCalorService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_PersistsAndReturnsDto()
        {
            // Arrange
            var dto = new DTOs.CreateEventoCalorDto
            {
                Nome = "Teste",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(2),
                Intensidade = 5,
                RegiaoId = 1
            };
            var evEntity = new EventoCalor
            {
                Id = 15,
                Nome = dto.Nome,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Intensidade = dto.Intensidade,
                RegiaoId = dto.RegiaoId
            };

            var mockRepo = new Mock<IEventoCalorRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<EventoCalor>()))
                    .ReturnsAsync(evEntity);

            var service = new EventoCalorService(mockRepo.Object);

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<EventoCalor>(e =>
                e.Nome == dto.Nome &&
                e.RegiaoId == dto.RegiaoId
            )), Times.Once);

            Assert.Equal(15, result.Id);
            Assert.Equal("Teste", result.Nome);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenEventNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IEventoCalorRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync((EventoCalor)null);
            var service = new EventoCalorService(mockRepo.Object);

            // Act
            var ok = await service.UpdateAsync(7, new DTOs.UpdateEventoCalorDto());

            // Assert
            Assert.False(ok);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenEventExists()
        {
            // Arrange
            var ev = new EventoCalor { Id = 11 };
            var mockRepo = new Mock<IEventoCalorRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(ev);
            var service = new EventoCalorService(mockRepo.Object);

            // Act
            var ok = await service.DeleteAsync(11);

            // Assert
            Assert.True(ok);
            mockRepo.Verify(r => r.DeleteAsync(11), Times.Once);
        }
    }
}
