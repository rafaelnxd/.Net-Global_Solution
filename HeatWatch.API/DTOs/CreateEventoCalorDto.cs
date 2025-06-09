using System;

namespace HeatWatch.API.DTOs

    {
    public class CreateEventoCalorDto
    {
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Intensidade { get; set; }
        public int RegiaoId { get; set; }
    }
}

