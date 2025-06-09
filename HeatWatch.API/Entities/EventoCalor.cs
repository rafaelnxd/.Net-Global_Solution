using System;
using System.Collections.Generic;

namespace HeatWatch.API.Entities
{
    public class EventoCalor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Intensidade { get; set; }
        public int RegiaoId { get; set; }
        public Regiao Regiao { get; set; }
        public ICollection<Alerta> Alertas { get; set; }
    }
}