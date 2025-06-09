// Entities/RegistroTemperatura.cs
using System;

namespace HeatWatch.API.Entities
{
    public class RegistroTemperatura
    {
        public int Id { get; set; }
        public int RegiaoId { get; set; }
        public Regiao Regiao { get; set; }
        public DateTime DataRegistro { get; set; }
        public double TemperaturaCelsius { get; set; }
    }
}
