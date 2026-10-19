using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Marca { get; set; }
        public string Medida { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int Estoque { get; set; }
    }

    public class ProdutoCollection : List<Produto>
    {
    }
}
