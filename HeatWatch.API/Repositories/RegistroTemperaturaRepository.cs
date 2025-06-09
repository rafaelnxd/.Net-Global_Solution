// Repositories/RegistroTemperaturaRepository.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HeatWatch.API.Repositories
{
    public class RegistroTemperaturaRepository : IRegistroTemperaturaRepository
    {
        private readonly HeatWatchContext _ctx;
        public RegistroTemperaturaRepository(HeatWatchContext ctx) => _ctx = ctx;

        public async Task<PagedResult<RegistroTemperatura>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var q = _ctx.RegistrosTemperatura.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filter) && double.TryParse(filter, out var temp))
                q = q.Where(r => r.TemperaturaCelsius == temp);

            var total = await q.CountAsync();

            bool desc = sort.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            string prop = desc
                ? sort.Substring(0, sort.Length - 5)
                : sort;

            q = desc
                ? q.OrderByDescending(e => EF.Property<object>(e, prop))
                : q.OrderBy(e => EF.Property<object>(e, prop));

            var items = await q
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResult<RegistroTemperatura>(items, total, page, size);
        }

        public Task<RegistroTemperatura> GetByIdAsync(int id) =>
            _ctx.RegistrosTemperatura
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<RegistroTemperatura> AddAsync(RegistroTemperatura reg)
        {
            _ctx.RegistrosTemperatura.Add(reg);
            await _ctx.SaveChangesAsync();
            return reg;
        }

        public async Task UpdateAsync(RegistroTemperatura reg)
        {
            _ctx.RegistrosTemperatura.Update(reg);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.RegistrosTemperatura.FindAsync(id);
            if (e == null) return;
            _ctx.RegistrosTemperatura.Remove(e);
            await _ctx.SaveChangesAsync();
        }
    }
}