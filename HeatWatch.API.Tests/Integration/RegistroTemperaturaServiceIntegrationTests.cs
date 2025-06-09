// HeatWatch.API.Tests.Integration.RegistroTemperaturaServiceIntegrationTests.cs

using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Services;
using HeatWatch.API.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HeatWatch.API.Tests.Integration
{
    public class RegistroTemperaturaServiceIntegrationTests
    {
        private readonly HeatWatchContext _context;
        private readonly RegistroTemperaturaService _service;

        public RegistroTemperaturaServiceIntegrationTests()
        {
            // 1) Cria contexto In-Memory
            _context = TestContextFactory.Create();

            // 2) Semeia uma região para a FK
            _context.Regioes.Add(new Regiao
            {
                Nome = "R-Temp",
                Latitude = 0,
                Longitude = 0,
                Descricao = "",
                Area = 1
            });
            _context.SaveChanges();

            // 3) Instancia repositório e serviço
            var repo = new RegistroTemperaturaRepository(_context);
            _service = new RegistroTemperaturaService(repo);
        }

        [Fact]
        public async Task CreateAsync_PersistsAndReturnsDto()
        {
            var dto = new CreateRegistroTemperaturaDto
            {
                RegiaoId = _context.Regioes.First().Id,
                DataRegistro = DateTime.UtcNow,
                TemperaturaCelsius = 25.5
            };

            var created = await _service.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal(25.5, created.TemperaturaCelsius);

            var entity = await _context.RegistrosTemperatura.FindAsync(created.Id);
            Assert.NotNull(entity);
            Assert.Equal(25.5, entity.TemperaturaCelsius);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenExists()
        {
            var seeded = _context.RegistrosTemperatura.Add(new RegistroTemperatura
            {
                RegiaoId = _context.Regioes.First().Id,
                DataRegistro = DateTime.UtcNow,
                TemperaturaCelsius = 30.0
            }).Entity;
            await _context.SaveChangesAsync();

            var dto = await _service.GetByIdAsync(seeded.Id);

            Assert.NotNull(dto);
            Assert.Equal(30.0, dto.TemperaturaCelsius);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList()
        {
            // semear um registro extra
            _context.RegistrosTemperatura.Add(new RegistroTemperatura
            {
                RegiaoId = _context.Regioes.First().Id,
                DataRegistro = DateTime.UtcNow,
                TemperaturaCelsius = 18.2
            });
            await _context.SaveChangesAsync();

            // usar campo real para sort e filtro vazio
            var paged = await _service.GetAllAsync(
                page: 1,
                size: 10,
                sort: "DataRegistro",
                filter: string.Empty
            );

            Assert.NotNull(paged);
            Assert.NotEmpty(paged.Items);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEntity_WhenExists()
        {
            var seeded = _context.RegistrosTemperatura.Add(new RegistroTemperatura
            {
                RegiaoId = _context.Regioes.First().Id,
                DataRegistro = DateTime.UtcNow,
                TemperaturaCelsius = 22.1
            }).Entity;
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(seeded.Id);

            Assert.True(result);
            Assert.Null(await _context.RegistrosTemperatura.FindAsync(seeded.Id));
        }
    }
}
