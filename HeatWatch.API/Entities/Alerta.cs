
using System;

namespace HeatWatch.API.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Severidade { get; set; }

        public int EventoCalorId { get; set; }
        public EventoCalor EventoCalor { get; set; }
    }
}
