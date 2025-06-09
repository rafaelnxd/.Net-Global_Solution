
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using System.Threading.Tasks;

namespace HeatWatch.API.Repositories
{
    public interface IEventoCalorRepository
    {
        Task<PagedResult<EventoCalor>> GetAllAsync(int page, int size, string sort, string filter);
        Task<EventoCalor> GetByIdAsync(int id);
        Task<EventoCalor> AddAsync(EventoCalor ev);
        Task UpdateAsync(EventoCalor ev);
        Task DeleteAsync(int id);
    }
}