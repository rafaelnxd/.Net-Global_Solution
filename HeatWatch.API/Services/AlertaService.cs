using System.Linq;
using System.Threading.Tasks;
using HeatWatch.API.DTOs;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using HeatWatch.API.Repositories;

namespace HeatWatch.API.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly IAlertaRepository _repo;
        public AlertaService(IAlertaRepository repo) => _repo = repo;

        public async Task<PagedResult<AlertaDto>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var p = await _repo.GetAllAsync(page, size, sort, filter);
            var dtos = p.Items.Select(a => new AlertaDto
            {
                Id = a.Id,
                Mensagem = a.Mensagem,
                DataEmissao = a.DataEmissao,
                Severidade = a.Severidade,
                EventoCalorId = a.EventoCalorId
            });
            return new PagedResult<AlertaDto>(dtos, p.TotalCount, p.Page, p.PageSize);
        }

        public async Task<AlertaDto> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;
            return new AlertaDto
            {
                Id = a.Id,
                Mensagem = a.Mensagem,
                DataEmissao = a.DataEmissao,
                Severidade = a.Severidade,
                EventoCalorId = a.EventoCalorId
            };
        }

        public async Task<AlertaDto> CreateAsync(CreateAlertaDto dto)
        {
            var a = new Alerta
            {
                Mensagem = dto.Mensagem,
                DataEmissao = dto.DataEmissao,
                Severidade = dto.Severidade,
                EventoCalorId = dto.EventoCalorId
            };
            var created = await _repo.AddAsync(a);
            return new AlertaDto
            {
                Id = created.Id,
                Mensagem = created.Mensagem,
                DataEmissao = created.DataEmissao,
                Severidade = created.Severidade,
                EventoCalorId = created.EventoCalorId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateAlertaDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            existing.Mensagem = dto.Mensagem;
            existing.DataEmissao = dto.DataEmissao;
            existing.Severidade = dto.Severidade;
            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}