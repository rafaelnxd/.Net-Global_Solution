// HeatWatch.API.Tests.Integration.RegiaoServiceIntegrationTests.cs

using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using HeatWatch.API.DTOs;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Integration
{
    public class RegiaoServiceIntegrationTests
    {
        private readonly HeatWatchContext _context;
        private readonly RegiaoService _service;

        public RegiaoServiceIntegrationTests()
        {
            _context = TestContextFactory.Create();
            _service = new RegiaoService(new RegiaoRepository(_context));
        }

        [Fact]
        public async Task CreateAsync_PersistsAndReturnsDto()
        {
            var dto = new CreateRegiaoDto
            {
                Nome = "NovaRegiao",
                Latitude = -5,
                Longitude = 30,
                Descricao = "Desc",
                Area = 100
            };

            var created = await _service.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);

            var entity = await _context.Regioes.FindAsync(created.Id);
            Assert.NotNull(entity);
            Assert.Equal("NovaRegiao", entity.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenExists()
        {
            var seeded = _context.Regioes.Add(new Regiao
            {
                Nome = "Semeada",
                Latitude = 0,
                Longitude = 0,
                Descricao = "",
                Area = 1
            }).Entity;
            await _context.SaveChangesAsync();

            var dto = await _service.GetByIdAsync(seeded.Id);

            Assert.NotNull(dto);
            Assert.Equal("Semeada", dto.Nome);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEntity_WhenExists()
        {
            var seeded = _context.Regioes.Add(new Regiao
            {
                Nome = "X",
                Latitude = 0,
                Longitude = 0,
                Descricao = "",
                Area = 1
            }).Entity;
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(seeded.Id);
            Assert.True(result);

            Assert.Null(await _context.Regioes.FindAsync(seeded.Id));
        }
    }
}
