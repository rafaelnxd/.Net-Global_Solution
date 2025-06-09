using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Unit
{
    public class RegiaoServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenRegionExists()
        {
            // Arrange
            var mockRepo = new Mock<IRegiaoRepository>();
            mockRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Regiao
                {
                    Id = 1,
                    Nome = "Teste",
                    Latitude = -10.0,
                    Longitude = 20.0,
                    Descricao = "Desc",
                    Area = 100.5
                });

            var service = new RegiaoService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Teste", result.Nome);
            Assert.Equal(-10.0, result.Latitude);
            Assert.Equal(20.0, result.Longitude);
            Assert.Equal("Desc", result.Descricao);
            Assert.Equal(100.5, result.Area);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenRegionNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IRegiaoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Regiao)null);
            var service = new RegiaoService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(42);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_InvokesRepositoryAndReturnsCreatedDto()
        {
            // Arrange
            var dto = new DTOs.CreateRegiaoDto
            {
                Nome = "Nova",
                Latitude = 1,
                Longitude = 2,
                Descricao = "D",
                Area = 3.3
            };
            var entity = new Regiao
            {
                Id = 7,
                Nome = dto.Nome,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Descricao = dto.Descricao,
                Area = dto.Area
            };

            var mockRepo = new Mock<IRegiaoRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Regiao>()))
                    .ReturnsAsync(entity);

            var service = new RegiaoService(mockRepo.Object);

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Regiao>(e =>
                e.Nome == dto.Nome &&
                e.Latitude == dto.Latitude &&
                e.Longitude == dto.Longitude &&
                e.Descricao == dto.Descricao &&
                e.Area == dto.Area
            )), Times.Once);

            Assert.Equal(7, result.Id);
            Assert.Equal("Nova", result.Nome);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenRegionNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IRegiaoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Regiao)null);
            var service = new RegiaoService(mockRepo.Object);

            // Act
            var ok = await service.UpdateAsync(5, new DTOs.UpdateRegiaoDto());

            // Assert
            Assert.False(ok);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenRegionExists()
        {
            // Arrange
            var existing = new Regiao { Id = 9 };
            var mockRepo = new Mock<IRegiaoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(existing);
            var service = new RegiaoService(mockRepo.Object);

            // Act
            var ok = await service.DeleteAsync(9);

            // Assert
            Assert.True(ok);
            mockRepo.Verify(r => r.DeleteAsync(9), Times.Once);
        }
    }
}
