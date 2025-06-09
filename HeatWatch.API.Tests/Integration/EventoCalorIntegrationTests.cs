// HeatWatch.API.Tests.Integration.EventoCalorServiceIntegrationTests.cs

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
    public class EventoCalorServiceIntegrationTests
    {
        private readonly HeatWatchContext _context;
        private readonly EventoCalorService _service;

        public EventoCalorServiceIntegrationTests()
        {
            _context = TestContextFactory.Create();
            // semente de região para FK
            var regiao = _context.Regioes.Add(new Regiao
            {
                Nome = "R1",
                Latitude = 0,
                Longitude = 0,
                Descricao = "",
                Area = 1
            }).Entity;
            _context.SaveChanges();

            _service = new EventoCalorService(new EventoCalorRepository(_context));
        }

        [Fact]
        public async Task CreateAsync_PersistsAndReturnsDto()
        {
            var dto = new CreateEventoCalorDto
            {
                Nome = "EC Teste",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow.AddHours(2),
                Intensidade = 4,
                RegiaoId = _context.Regioes.First().Id
            };

            var created = await _service.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("EC Teste", created.Nome);

            var entity = await _context.EventosCalor.FindAsync(created.Id);
            Assert.NotNull(entity);
            Assert.Equal("EC Teste", entity.Nome);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList()
        {
            // semear um evento extra
            _context.EventosCalor.Add(new EventoCalor
            {
                Nome = "E2",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow.AddHours(1),
                Intensidade = 2,
                RegiaoId = _context.Regioes.First().Id
            });
            await _context.SaveChangesAsync();

            var paged = await _service.GetAllAsync(
                page: 1,
                size: 10,
                sort: "DataInicio",
                filter: string.Empty
            );

            Assert.NotNull(paged);
            Assert.NotEmpty(paged.Items);
        }
    }
}
