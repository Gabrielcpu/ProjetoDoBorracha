using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int TempoEstimado { get; set; }
        public int GarantiaDias { get; set; }
        public bool Ativo { get; set; }
    }

    public class ServicoCollection : List<Servico>
    {
    }
}

