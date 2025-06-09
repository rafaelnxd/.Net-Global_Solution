using System;

namespace HeatWatch.API.DTOs
{
    public class EventoCalorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Intensidade { get; set; }
        public int RegiaoId { get; set; }
    }
}
