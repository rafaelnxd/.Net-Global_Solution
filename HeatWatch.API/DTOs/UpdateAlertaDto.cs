using System;

namespace HeatWatch.API.DTOs
{
    public class UpdateAlertaDto
    {
        public string Mensagem { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Severidade { get; set; }
    }
}