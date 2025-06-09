using System;

namespace HeatWatch.API.DTOs
{
    public class UpdateEventoCalorDto
    {
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Intensidade { get; set; }
    }
}
