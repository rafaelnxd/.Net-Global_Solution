using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HeatWatch.API.Repositories
{
    public class EventoCalorRepository : IEventoCalorRepository
    {
        private readonly HeatWatchContext _ctx;
        public EventoCalorRepository(HeatWatchContext ctx) => _ctx = ctx;

        public async Task<PagedResult<EventoCalor>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var q = _ctx.EventosCalor.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filter))
                q = q.Where(e => e.Nome.Contains(filter));
            var total = await q.CountAsync();
            bool desc = sort.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            var prop = desc ? sort[..^5] : sort;
            q = desc ? q.OrderByDescending(e => EF.Property<object>(e, prop))
                     : q.OrderBy(e => EF.Property<object>(e, prop));
            var items = await q.Skip((page - 1) * size).Take(size).ToListAsync();
            return new PagedResult<EventoCalor>(items, total, page, size);
        }

        public Task<EventoCalor> GetByIdAsync(int id) =>
            _ctx.EventosCalor.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<EventoCalor> AddAsync(EventoCalor ev)
        {
            _ctx.EventosCalor.Add(ev);
            await _ctx.SaveChangesAsync();
            return ev;
        }

        public async Task UpdateAsync(EventoCalor ev)
        {
            _ctx.EventosCalor.Update(ev);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.EventosCalor.FindAsync(id);
            if (e == null) return;
            _ctx.EventosCalor.Remove(e);
            await _ctx.SaveChangesAsync();
        }
    }
}