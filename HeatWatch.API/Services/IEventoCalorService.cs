
using HeatWatch.API.DTOs;
using HeatWatch.API.Helpers;
using System.Threading.Tasks;

namespace HeatWatch.API.Services
{
    public interface IEventoCalorService
    {
        Task<PagedResult<EventoCalorDto>> GetAllAsync(int page, int size, string sort, string filter);
        Task<EventoCalorDto> GetByIdAsync(int id);
        Task<EventoCalorDto> CreateAsync(CreateEventoCalorDto dto);
        Task<bool> UpdateAsync(int id, UpdateEventoCalorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}