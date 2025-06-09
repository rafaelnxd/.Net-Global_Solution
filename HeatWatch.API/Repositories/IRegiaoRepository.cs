using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using System.Threading.Tasks;

namespace HeatWatch.API.Repositories
{
    public interface IRegiaoRepository
    {
        Task<PagedResult<Regiao>> GetAllAsync(int page, int size, string sort, string filter);
        Task<Regiao> GetByIdAsync(int id);
        Task<Regiao> AddAsync(Regiao regiao);
        Task UpdateAsync(Regiao regiao);
        Task DeleteAsync(int id);
    }
}
