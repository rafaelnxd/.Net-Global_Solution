using System;

namespace HeatWatch.API.DTOs
{
    public class CreateAlertaDto
    {
        public string Mensagem { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Severidade { get; set; }
        public int EventoCalorId { get; set; }
    }
}