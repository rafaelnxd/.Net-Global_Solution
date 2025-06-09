using System.Linq;
using System.Threading.Tasks;
using HeatWatch.API.DTOs;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using HeatWatch.API.Repositories;

namespace HeatWatch.API.Services
{
    public class RegistroTemperaturaService : IRegistroTemperaturaService
    {
        private readonly IRegistroTemperaturaRepository _repo;
        public RegistroTemperaturaService(IRegistroTemperaturaRepository repo) => _repo = repo;

        public async Task<PagedResult<RegistroTemperaturaDto>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var p = await _repo.GetAllAsync(page, size, sort, filter);
            var dtos = p.Items.Select(r => new RegistroTemperaturaDto
            {
                Id = r.Id,
                RegiaoId = r.RegiaoId,
                DataRegistro = r.DataRegistro,
                TemperaturaCelsius = r.TemperaturaCelsius
            });
            return new PagedResult<RegistroTemperaturaDto>(dtos, p.TotalCount, p.Page, p.PageSize);
        }

        public async Task<RegistroTemperaturaDto> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new RegistroTemperaturaDto
            {
                Id = r.Id,
                RegiaoId = r.RegiaoId,
                DataRegistro = r.DataRegistro,
                TemperaturaCelsius = r.TemperaturaCelsius
            };
        }

        public async Task<RegistroTemperaturaDto> CreateAsync(CreateRegistroTemperaturaDto dto)
        {
            var r = new RegistroTemperatura
            {
                RegiaoId = dto.RegiaoId,
                DataRegistro = dto.DataRegistro,
                TemperaturaCelsius = dto.TemperaturaCelsius
            };
            var created = await _repo.AddAsync(r);
            return new RegistroTemperaturaDto
            {
                Id = created.Id,
                RegiaoId = created.RegiaoId,
                DataRegistro = created.DataRegistro,
                TemperaturaCelsius = created.TemperaturaCelsius
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateRegistroTemperaturaDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            existing.DataRegistro = dto.DataRegistro;
            existing.TemperaturaCelsius = dto.TemperaturaCelsius;
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