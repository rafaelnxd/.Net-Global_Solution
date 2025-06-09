using System.Threading.Tasks;
using HeatWatch.API.DTOs;
using HeatWatch.API.Helpers;

namespace HeatWatch.API.Services
{
    public interface IAlertaService
    {
        Task<PagedResult<AlertaDto>> GetAllAsync(int page, int size, string sort, string filter);
        Task<AlertaDto> GetByIdAsync(int id);
        Task<AlertaDto> CreateAsync(CreateAlertaDto dto);
        Task<bool> UpdateAsync(int id, UpdateAlertaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}