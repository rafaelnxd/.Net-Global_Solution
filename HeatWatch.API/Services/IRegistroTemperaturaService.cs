using System.Threading.Tasks;
using HeatWatch.API.DTOs;
using HeatWatch.API.Helpers;

namespace HeatWatch.API.Services
{
    public interface IRegistroTemperaturaService
    {
        Task<PagedResult<RegistroTemperaturaDto>> GetAllAsync(int page, int size, string sort, string filter);
        Task<RegistroTemperaturaDto> GetByIdAsync(int id);
        Task<RegistroTemperaturaDto> CreateAsync(CreateRegistroTemperaturaDto dto);
        Task<bool> UpdateAsync(int id, UpdateRegistroTemperaturaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}