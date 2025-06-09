// HeatWatch.API.Tests.Integration.AlertaServiceIntegrationTests.cs

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
    public class AlertaServiceIntegrationTests
    {
        private readonly HeatWatchContext _context;
        private readonly AlertaService _service;

        public AlertaServiceIntegrationTests()
        {
            // 1) Cria contexto In-Memory
            _context = TestContextFactory.Create();

            // 2) Semeia uma região para FK de eventos
            var regiao = _context.Regioes.Add(new Regiao
            {
                Nome = "R-Alerta",
                Latitude = 0,
                Longitude = 0,
                Descricao = "",
                Area = 1
            }).Entity;
            _context.SaveChanges();

            // 3) Semeia um evento de calor para FK de alerta
            var evento = _context.EventosCalor.Add(new EventoCalor
            {
                Nome = "E-Alerta",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow.AddHours(1),
                Intensidade = 3,
                RegiaoId = regiao.Id
            }).Entity;
            _context.SaveChanges();

            // 4) Instancia repositório e serviço
            var repo = new AlertaRepository(_context);
            _service = new AlertaService(repo);
        }

        [Fact]
        public async Task CreateAsync_PersistsAndReturnsDto()
        {
            var dto = new CreateAlertaDto
            {
                Mensagem = "Teste Alerta",
                DataEmissao = DateTime.UtcNow,
                Severidade = "Alta",
                EventoCalorId = _context.EventosCalor.First().Id
            };

            var created = await _service.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.Equal("Teste Alerta", created.Mensagem);

            var entity = await _context.Alertas.FindAsync(created.Id);
            Assert.NotNull(entity);
            Assert.Equal("Teste Alerta", entity.Mensagem);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenExists()
        {
            var seeded = _context.Alertas.Add(new Alerta
            {
                Mensagem = "Seed Alerta",
                DataEmissao = DateTime.UtcNow,
                Severidade = "Média",
                EventoCalorId = _context.EventosCalor.First().Id
            }).Entity;
            await _context.SaveChangesAsync();

            var dto = await _service.GetByIdAsync(seeded.Id);

            Assert.NotNull(dto);
            Assert.Equal("Seed Alerta", dto.Mensagem);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList()
        {
            // semear um alerta extra
            _context.Alertas.Add(new Alerta
            {
                Mensagem = "Outro Alerta",
                DataEmissao = DateTime.UtcNow,
                Severidade = "Baixa",
                EventoCalorId = _context.EventosCalor.First().Id
            });
            await _context.SaveChangesAsync();

            var paged = await _service.GetAllAsync(
                page: 1,
                size: 10,
                sort: "DataEmissao",
                filter: string.Empty
            );

            Assert.NotNull(paged);
            Assert.NotEmpty(paged.Items);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEntity_WhenExists()
        {
            var seeded = _context.Alertas.Add(new Alerta
            {
                Mensagem = "ParaExcluir",
                DataEmissao = DateTime.UtcNow,
                Severidade = "Crítica",
                EventoCalorId = _context.EventosCalor.First().Id
            }).Entity;
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(seeded.Id);

            Assert.True(result);
            Assert.Null(await _context.Alertas.FindAsync(seeded.Id));
        }
    }
}
