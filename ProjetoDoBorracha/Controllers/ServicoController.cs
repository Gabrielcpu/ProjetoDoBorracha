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
    public class ServicoController
    {
        private readonly DataBaseService dataBaseService;

        public ServicoController()
        {
            dataBaseService = new DataBaseService();
        }

        public int Inserir(Servico servico)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Servico
                  (Nome, Descricao, Preco, TempoEstimado, GarantiaDias, Ativo)
                  VALUES
                  (@Nome, @Descricao, @Preco, @TempoEstimado, @GarantiaDias, @Ativo)");

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = servico.Nome;
            cmd.Parameters.Add("@Descricao", SqlDbType.VarChar, 250).Value =
                (object)servico.Descricao ?? DBNull.Value;
            cmd.Parameters.Add("@Preco", SqlDbType.Decimal).Value = servico.Preco;
            cmd.Parameters.Add("@TempoEstimado", SqlDbType.Int).Value = servico.TempoEstimado;
            cmd.Parameters.Add("@GarantiaDias", SqlDbType.Int).Value = servico.GarantiaDias;
            cmd.Parameters.Add("@Ativo", SqlDbType.Bit).Value = servico.Ativo;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Alterar(Servico servico)
        {
            SqlCommand cmd = new SqlCommand(
                @"UPDATE Servico
                  SET Nome = @Nome,
                      Descricao = @Descricao,
                      Preco = @Preco,
                      TempoEstimado = @TempoEstimado,
                      GarantiaDias = @GarantiaDias,
                      Ativo = @Ativo
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = servico.Id;
            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = servico.Nome;
            cmd.Parameters.Add("@Descricao", SqlDbType.VarChar, 250).Value =
                (object)servico.Descricao ?? DBNull.Value;
            cmd.Parameters.Add("@Preco", SqlDbType.Decimal).Value = servico.Preco;
            cmd.Parameters.Add("@TempoEstimado", SqlDbType.Int).Value = servico.TempoEstimado;
            cmd.Parameters.Add("@GarantiaDias", SqlDbType.Int).Value = servico.GarantiaDias;
            cmd.Parameters.Add("@Ativo", SqlDbType.Bit).Value = servico.Ativo;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Excluir(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Servico WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return dataBaseService.ExecuteSql(cmd);
        }

        public Servico GetById(int id)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Preco,
                         TempoEstimado, GarantiaDias, Ativo
                  FROM Servico
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable tabela = dataBaseService.GetDataTable(cmd);

            if (tabela.Rows.Count == 0)
                return null;

            return ConverterServico(tabela.Rows[0]);
        }

        public ServicoCollection GetAll()
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Preco,
                         TempoEstimado, GarantiaDias, Ativo
                  FROM Servico
                  ORDER BY Nome");

            return ConverterServicos(dataBaseService.GetDataTable(cmd));
        }

        public ServicoCollection GetByFilters(
            string campo = "", string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(campo))
                return GetAll();

            switch (campo)
            {
                case "Nome":
                case "Descricao":
                    break;
                default:
                    throw new ArgumentException("Campo de pesquisa inválido.", nameof(campo));
            }

            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Preco,
                         TempoEstimado, GarantiaDias, Ativo
                  FROM Servico
                  WHERE " + campo + @" LIKE @Filtro
                  ORDER BY Nome");

            cmd.Parameters.Add("@Filtro", SqlDbType.VarChar, 200).Value =
                "%" + (filtro ?? "") + "%";

            return ConverterServicos(dataBaseService.GetDataTable(cmd));
        }

        private Servico ConverterServico(DataRow linha)
        {
            return new Servico
            {
                Id = Convert.ToInt32(linha["Id"]),
                Nome = Convert.ToString(linha["Nome"]),
                Descricao = linha["Descricao"] == DBNull.Value
                    ? null : Convert.ToString(linha["Descricao"]),
                Preco = Convert.ToDecimal(linha["Preco"]),
                TempoEstimado = Convert.ToInt32(linha["TempoEstimado"]),
                GarantiaDias = Convert.ToInt32(linha["GarantiaDias"]),
                Ativo = Convert.ToBoolean(linha["Ativo"])
            };
        }

        private ServicoCollection ConverterServicos(DataTable tabela)
        {
            ServicoCollection servicos = new ServicoCollection();

            foreach (DataRow linha in tabela.Rows)
                servicos.Add(ConverterServico(linha));

            return servicos;
        }
    }
}