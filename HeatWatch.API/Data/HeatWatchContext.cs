using Microsoft.EntityFrameworkCore;
using HeatWatch.API.Entities;

namespace HeatWatch.API.Data
{
    public class HeatWatchContext : DbContext
    {
        public HeatWatchContext(DbContextOptions<HeatWatchContext> opts)
          : base(opts)
        { }

        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<EventoCalor> EventosCalor { get; set; }
        public DbSet<RegistroTemperatura> RegistrosTemperatura { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
    }
}
