using System.Threading.Tasks;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;

namespace HeatWatch.API.Repositories
{
    public interface IRegistroTemperaturaRepository
    {
        Task<PagedResult<RegistroTemperatura>> GetAllAsync(int page, int size, string sort, string filter);
        Task<RegistroTemperatura> GetByIdAsync(int id);
        Task<RegistroTemperatura> AddAsync(RegistroTemperatura reg);
        Task UpdateAsync(RegistroTemperatura reg);
        Task DeleteAsync(int id);
    }
}