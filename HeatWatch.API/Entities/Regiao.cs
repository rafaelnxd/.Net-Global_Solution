
using System;
using System.Collections.Generic;

namespace HeatWatch.API.Entities
{
    public class Regiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Descricao { get; set; }
        public double Area { get; set; }

        public ICollection<EventoCalor> EventosCalor { get; set; }
        public ICollection<RegistroTemperatura> RegistrosTemperatura { get; set; }
    }
}
