using System;

namespace HeatWatch.API.DTOs
{
    public class AlertaDto
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Severidade { get; set; }
        public int EventoCalorId { get; set; }
    }
}