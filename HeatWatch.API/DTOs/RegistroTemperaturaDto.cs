using System;

namespace HeatWatch.API.DTOs
{
    public class RegistroTemperaturaDto
    {
        public int Id { get; set; }
        public int RegiaoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public double TemperaturaCelsius { get; set; }
    }
}