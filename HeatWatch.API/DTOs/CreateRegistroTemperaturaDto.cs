using System;

namespace HeatWatch.API.DTOs
{
    public class CreateRegistroTemperaturaDto
    {
        public int RegiaoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public double TemperaturaCelsius { get; set; }
    }
}