using HeatWatch.API.DTOs;
using HeatWatch.API.Entities;
using HeatWatch.API.Repositories;
using HeatWatch.API.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace HeatWatch.API.Services
{
    public class RegiaoService : IRegiaoService
    {
        private readonly IRegiaoRepository _repo;
        public RegiaoService(IRegiaoRepository repo) => _repo = repo;

        public async Task<PagedResult<RegiaoDto>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var paged = await _repo.GetAllAsync(page, size, sort, filter);
            var dtos = paged.Items.Select(r => new RegiaoDto
            {
                Id = r.Id,
                Nome = r.Nome,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Descricao = r.Descricao,
                Area = r.Area
            });
            return new PagedResult<RegiaoDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
        }

        public async Task<RegiaoDto> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new RegiaoDto
            {
                Id = r.Id,
                Nome = r.Nome,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Descricao = r.Descricao,
                Area = r.Area
            };
        }

        public async Task<RegiaoDto> CreateAsync(CreateRegiaoDto dto)
        {
            var entity = new Regiao
            {
                Nome = dto.Nome,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Descricao = dto.Descricao,
                Area = dto.Area
            };

            var created = await _repo.AddAsync(entity);

            return new RegiaoDto
            {
                Id = created.Id,
                Nome = created.Nome,
                Latitude = created.Latitude,
                Longitude = created.Longitude,
                Descricao = created.Descricao,
                Area = created.Area
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateRegiaoDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            // Mapeia os campos do DTO na entidade existente
            existing.Nome = dto.Nome;
            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;
            existing.Descricao = dto.Descricao;
            existing.Area = dto.Area;

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
