using HeatWatch.API.DTOs;
using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace HeatWatch.API.Services
{
    public class EventoCalorService : IEventoCalorService   
    {
        private readonly IEventoCalorRepository _repo;
        public EventoCalorService(IEventoCalorRepository repo) => _repo = repo;

        public async Task<PagedResult<EventoCalorDto>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var paged = await _repo.GetAllAsync(page, size, sort, filter);
            var dtos = paged.Items.Select(e => new EventoCalorDto
            {
                Id = e.Id,
                Nome = e.Nome,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                Intensidade = e.Intensidade,
                RegiaoId = e.RegiaoId
            });
            return new PagedResult<EventoCalorDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
        }

        public async Task<EventoCalorDto> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;
            return new EventoCalorDto
            {
                Id = e.Id,
                Nome = e.Nome,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                Intensidade = e.Intensidade,
                RegiaoId = e.RegiaoId
            };
        }

        public async Task<EventoCalorDto> CreateAsync(CreateEventoCalorDto dto)
        {
            var entity = new EventoCalor
            {
                Nome = dto.Nome,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Intensidade = dto.Intensidade,
                RegiaoId = dto.RegiaoId
            };

            var created = await _repo.AddAsync(entity);

            return new EventoCalorDto
            {
                Id = created.Id,
                Nome = created.Nome,
                DataInicio = created.DataInicio,
                DataFim = created.DataFim,
                Intensidade = created.Intensidade,
                RegiaoId = created.RegiaoId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateEventoCalorDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            // Atualiza campos permitidos (RegiaoId não está no DTO de atualização)
            existing.Nome = dto.Nome;
            existing.DataInicio = dto.DataInicio;
            existing.DataFim = dto.DataFim;
            existing.Intensidade = dto.Intensidade;

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
