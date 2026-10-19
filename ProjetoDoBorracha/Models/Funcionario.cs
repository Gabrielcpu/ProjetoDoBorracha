using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
    }

    public class FuncionarioCollection : List<Funcionario>
    {
    }
}

