using System;

namespace HeatWatch.API.DTOs
{
    public class UpdateRegistroTemperaturaDto
    {
        public DateTime DataRegistro { get; set; }
        public double TemperaturaCelsius { get; set; }
    }
}