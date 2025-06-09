using System.Threading.Tasks;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;

namespace HeatWatch.API.Repositories
{
    public interface IAlertaRepository
    {
        Task<PagedResult<Alerta>> GetAllAsync(int page, int size, string sort, string filter);
        Task<Alerta> GetByIdAsync(int id);
        Task<Alerta> AddAsync(Alerta alerta);
        Task UpdateAsync(Alerta alerta);
        Task DeleteAsync(int id);
    }
}