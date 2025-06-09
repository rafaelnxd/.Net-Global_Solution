using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeatWatch.API.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly HeatWatchContext _ctx;
        public RegiaoRepository(HeatWatchContext ctx) => _ctx = ctx;

        public async Task<PagedResult<Regiao>> GetAllAsync(
            int page, int size, string sort, string filter)
        {
            var query = _ctx.Regioes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(r => r.Nome.Contains(filter));

            var total = await query.CountAsync();

            // Determina ordenação e direção
            bool descending = sort.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            string propertyName = descending
                ? sort.Substring(0, sort.Length - " desc".Length)
                : sort;

            query = descending
                ? query.OrderByDescending(e => EF.Property<object>(e, propertyName))
                : query.OrderBy(e => EF.Property<object>(e, propertyName));

            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResult<Regiao>(items, total, page, size);
        }

        public Task<Regiao> GetByIdAsync(int id) =>
            _ctx.Regioes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Regiao> AddAsync(Regiao regiao)
        {
            _ctx.Regioes.Add(regiao);
            await _ctx.SaveChangesAsync();
            return regiao;
        }

        public async Task UpdateAsync(Regiao regiao)
        {
            _ctx.Regioes.Update(regiao);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _ctx.Regioes.FindAsync(id);
            if (entity == null) return;
            _ctx.Regioes.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
