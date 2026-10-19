using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }
        public string TipoVeiculo { get; set; }
        public int IdCliente { get; set; }
    }

    public class VeiculoCollection : List<Veiculo>
    {
    }
}

