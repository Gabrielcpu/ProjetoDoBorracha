using ProjetoDoBorracha.Models;
using ProjetoDoBorracha.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Controllers
{
    public class ClienteController
    {
        private readonly DataBaseService dataBaseService;

        public ClienteController()
        {
            dataBaseService = new DataBaseService();
        }

       
        public int Inserir(Cliente cliente)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Cliente
                  (Nome, CPF, Telefone, Email, Endereco)
                  VALUES
                  (@Nome, @CPF, @Telefone, @Email, @Endereco)");

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value =
                cliente.Nome;

            cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value =
                cliente.CPF;

            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value =
                cliente.Telefone;

            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                cliente.Email;

            cmd.Parameters.Add("@Endereco", SqlDbType.VarChar, 200).Value =
                cliente.Endereco;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Alterar(Cliente cliente)
        {
            SqlCommand cmd = new SqlCommand(
                @"UPDATE Cliente
                  SET Nome = @Nome,
                      CPF = @CPF,
                      Telefone = @Telefone,
                      Email = @Email,
                      Endereco = @Endereco
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value =
                cliente.Id;

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value =
                cliente.Nome;

            cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value =
                cliente.CPF;

            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value =
                cliente.Telefone;

            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                cliente.Email;

            cmd.Parameters.Add("@Endereco", SqlDbType.VarChar, 200).Value =
                cliente.Endereco;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Excluir(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Cliente WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return dataBaseService.ExecuteSql(cmd);
        }

        public Cliente GetById(int id)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email,
                         Endereco, DataCadastro
                  FROM Cliente
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable tabela = dataBaseService.GetDataTable(cmd);

            if (tabela.Rows.Count == 0)
                return null;

            return ConverterCliente(tabela.Rows[0]);
        }

        public ClienteCollection GetAll()
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email,
                         Endereco, DataCadastro
                  FROM Cliente
                  ORDER BY Nome");

            return ConverterClientes(
                dataBaseService.GetDataTable(cmd));
        }

        public ClienteCollection GetByFilters(
            string campo = "",
            string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(campo))
                return GetAll();


            switch (campo)
            {
                case "Nome":
                case "CPF":
                case "Telefone":
                case "Email":
                    break;

                default:
                    throw new ArgumentException(
                        "Campo de pesquisa inválido.", nameof(campo));
            }

            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email,
                         Endereco, DataCadastro
                  FROM Cliente
                  WHERE " + campo + @" LIKE @Filtro
                  ORDER BY Nome");

            cmd.Parameters.Add("@Filtro", SqlDbType.VarChar, 200).Value =
                "%" + (filtro ?? "") + "%";

            return ConverterClientes(
                dataBaseService.GetDataTable(cmd));
        }

        public ClienteCollection GetByName(string nome)
        {
            return GetByFilters("Nome", nome);
        }

        public ClienteCollection GetByEmail(string email)
        {
            return GetByFilters("Email", email);
        }
        private Cliente ConverterCliente(DataRow linha)
        {
            return new Cliente
            {
                Id = Convert.ToInt32(linha["Id"]),
                Nome = Convert.ToString(linha["Nome"]),
                CPF = Convert.ToString(linha["CPF"]),
                Telefone = Convert.ToString(linha["Telefone"]),
                Email = Convert.ToString(linha["Email"]),
                Endereco = Convert.ToString(linha["Endereco"]),
                DataCadastro = Convert.ToDateTime(
                    linha["DataCadastro"])
            };
        }

        private ClienteCollection ConverterClientes(DataTable tabela)
        {
            ClienteCollection clientes = new ClienteCollection();

            foreach (DataRow linha in tabela.Rows)
            {
                clientes.Add(ConverterCliente(linha));
            }

            return clientes;
        }
    }
}
    

