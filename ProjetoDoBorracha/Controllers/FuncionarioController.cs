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
    public class FuncionarioController
    {
        private readonly DataBaseService dataBaseService;

        public FuncionarioController()
        {
            dataBaseService = new DataBaseService();
        }

        public int Inserir(Funcionario funcionario)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Funcionario
                  (Nome, CPF, Telefone, Email, Cargo, DataAdmissao, Salario)
                  VALUES
                  (@Nome, @CPF, @Telefone, @Email, @Cargo, @DataAdmissao, @Salario)");

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = funcionario.Nome;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value = funcionario.CPF;
            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value = funcionario.Telefone;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = funcionario.Email;
            cmd.Parameters.Add("@Cargo", SqlDbType.VarChar, 60).Value = funcionario.Cargo;
            cmd.Parameters.Add("@DataAdmissao", SqlDbType.Date).Value = funcionario.DataAdmissao.Date;
            cmd.Parameters.Add("@Salario", SqlDbType.Decimal).Value = funcionario.Salario;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Alterar(Funcionario funcionario)
        {
            SqlCommand cmd = new SqlCommand(
                @"UPDATE Funcionario
                  SET Nome = @Nome,
                      CPF = @CPF,
                      Telefone = @Telefone,
                      Email = @Email,
                      Cargo = @Cargo,
                      DataAdmissao = @DataAdmissao,
                      Salario = @Salario
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = funcionario.Id;
            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = funcionario.Nome;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value = funcionario.CPF;
            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value = funcionario.Telefone;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = funcionario.Email;
            cmd.Parameters.Add("@Cargo", SqlDbType.VarChar, 60).Value = funcionario.Cargo;
            cmd.Parameters.Add("@DataAdmissao", SqlDbType.Date).Value = funcionario.DataAdmissao.Date;
            cmd.Parameters.Add("@Salario", SqlDbType.Decimal).Value = funcionario.Salario;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Excluir(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Funcionario WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return dataBaseService.ExecuteSql(cmd);
        }

        public Funcionario GetById(int id)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email, Cargo,
                         DataAdmissao, Salario
                  FROM Funcionario
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable tabela = dataBaseService.GetDataTable(cmd);

            if (tabela.Rows.Count == 0)
                return null;

            return ConverterFuncionario(tabela.Rows[0]);
        }

        public FuncionarioCollection GetAll()
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email, Cargo,
                         DataAdmissao, Salario
                  FROM Funcionario
                  ORDER BY Nome");

            return ConverterFuncionarios(dataBaseService.GetDataTable(cmd));
        }

        public FuncionarioCollection GetByFilters(
            string campo = "", string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(campo))
                return GetAll();

            switch (campo)
            {
                case "Nome":
                case "CPF":
                case "Telefone":
                case "Email":
                case "Cargo":
                    break;
                default:
                    throw new ArgumentException("Campo de pesquisa inválido.", nameof(campo));
            }

            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, CPF, Telefone, Email, Cargo,
                         DataAdmissao, Salario
                  FROM Funcionario
                  WHERE " + campo + @" LIKE @Filtro
                  ORDER BY Nome");

            cmd.Parameters.Add("@Filtro", SqlDbType.VarChar, 200).Value =
                "%" + (filtro ?? "") + "%";

            return ConverterFuncionarios(dataBaseService.GetDataTable(cmd));
        }

        private Funcionario ConverterFuncionario(DataRow linha)
        {
            return new Funcionario
            {
                Id = Convert.ToInt32(linha["Id"]),
                Nome = Convert.ToString(linha["Nome"]),
                CPF = Convert.ToString(linha["CPF"]),
                Telefone = Convert.ToString(linha["Telefone"]),
                Email = Convert.ToString(linha["Email"]),
                Cargo = Convert.ToString(linha["Cargo"]),
                DataAdmissao = Convert.ToDateTime(linha["DataAdmissao"]),
                Salario = Convert.ToDecimal(linha["Salario"])
            };
        }

        private FuncionarioCollection ConverterFuncionarios(DataTable tabela)
        {
            FuncionarioCollection funcionarios = new FuncionarioCollection();

            foreach (DataRow linha in tabela.Rows)
                funcionarios.Add(ConverterFuncionario(linha));

            return funcionarios;
        }
    }
}

