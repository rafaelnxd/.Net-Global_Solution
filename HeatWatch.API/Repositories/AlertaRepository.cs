using System;
using System.Linq;
using System.Threading.Tasks;
using HeatWatch.API.Data;
using HeatWatch.API.Entities;
using HeatWatch.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HeatWatch.API.Repositories
{
    public class AlertaRepository : IAlertaRepository
    {
        private readonly HeatWatchContext _ctx;
        public AlertaRepository(HeatWatchContext ctx) => _ctx = ctx;

        public async Task<PagedResult<Alerta>> GetAllAsync(int page, int size, string sort, string filter)
        {
            var q = _ctx.Alertas.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filter))
                q = q.Where(a => a.Mensagem.Contains(filter) || a.Severidade.Contains(filter));

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

            return new PagedResult<Alerta>(items, total, page, size);
        }

        public Task<Alerta> GetByIdAsync(int id) =>
            _ctx.Alertas
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Alerta> AddAsync(Alerta alerta)
        {
            _ctx.Alertas.Add(alerta);
            await _ctx.SaveChangesAsync();
            return alerta;
        }

        public async Task UpdateAsync(Alerta alerta)
        {
            _ctx.Alertas.Update(alerta);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.Alertas.FindAsync(id);
            if (e == null) return;
            _ctx.Alertas.Remove(e);
            await _ctx.SaveChangesAsync();
        }
    }
}