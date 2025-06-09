using HeatWatch.API.DTOs;
using HeatWatch.API.Helpers;
using System.Threading.Tasks;

namespace HeatWatch.API.Services
{
    public interface IRegiaoService
    {
        Task<PagedResult<RegiaoDto>> GetAllAsync(int page, int size, string sort, string filter);
        Task<RegiaoDto> GetByIdAsync(int id);
        Task<RegiaoDto> CreateAsync(CreateRegiaoDto dto);
        Task<bool> UpdateAsync(int id, UpdateRegiaoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
