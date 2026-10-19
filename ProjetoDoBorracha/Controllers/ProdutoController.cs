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
    public class ProdutoController
    {
        private readonly DataBaseService dataBaseService;

        public ProdutoController()
        {
            dataBaseService = new DataBaseService();
        }

        public int Inserir(Produto produto)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Produto
                  (Nome, Descricao, Marca, Medida, PrecoCusto, PrecoVenda, Estoque)
                  VALUES
                  (@Nome, @Descricao, @Marca, @Medida, @PrecoCusto, @PrecoVenda, @Estoque)");

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = produto.Nome;
            cmd.Parameters.Add("@Descricao", SqlDbType.VarChar, 250).Value =
                (object)produto.Descricao ?? DBNull.Value;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 60).Value =
                (object)produto.Marca ?? DBNull.Value;
            cmd.Parameters.Add("@Medida", SqlDbType.VarChar, 30).Value =
                (object)produto.Medida ?? DBNull.Value;
            cmd.Parameters.Add("@PrecoCusto", SqlDbType.Decimal).Value = produto.PrecoCusto;
            cmd.Parameters.Add("@PrecoVenda", SqlDbType.Decimal).Value = produto.PrecoVenda;
            cmd.Parameters.Add("@Estoque", SqlDbType.Int).Value = produto.Estoque;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Alterar(Produto produto)
        {
            SqlCommand cmd = new SqlCommand(
                @"UPDATE Produto
                  SET Nome = @Nome,
                      Descricao = @Descricao,
                      Marca = @Marca,
                      Medida = @Medida,
                      PrecoCusto = @PrecoCusto,
                      PrecoVenda = @PrecoVenda,
                      Estoque = @Estoque
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = produto.Id;
            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 100).Value = produto.Nome;
            cmd.Parameters.Add("@Descricao", SqlDbType.VarChar, 250).Value =
                (object)produto.Descricao ?? DBNull.Value;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 60).Value =
                (object)produto.Marca ?? DBNull.Value;
            cmd.Parameters.Add("@Medida", SqlDbType.VarChar, 30).Value =
                (object)produto.Medida ?? DBNull.Value;
            cmd.Parameters.Add("@PrecoCusto", SqlDbType.Decimal).Value = produto.PrecoCusto;
            cmd.Parameters.Add("@PrecoVenda", SqlDbType.Decimal).Value = produto.PrecoVenda;
            cmd.Parameters.Add("@Estoque", SqlDbType.Int).Value = produto.Estoque;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Excluir(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Produto WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return dataBaseService.ExecuteSql(cmd);
        }

        public Produto GetById(int id)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Marca, Medida,
                         PrecoCusto, PrecoVenda, Estoque
                  FROM Produto
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable tabela = dataBaseService.GetDataTable(cmd);

            if (tabela.Rows.Count == 0)
                return null;

            return ConverterProduto(tabela.Rows[0]);
        }

        public ProdutoCollection GetAll()
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Marca, Medida,
                         PrecoCusto, PrecoVenda, Estoque
                  FROM Produto
                  ORDER BY Nome");

            return ConverterProdutos(dataBaseService.GetDataTable(cmd));
        }

        public ProdutoCollection GetByFilters(
            string campo = "", string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(campo))
                return GetAll();

            switch (campo)
            {
                case "Nome":
                case "Descricao":
                case "Marca":
                case "Medida":
                    break;
                default:
                    throw new ArgumentException("Campo de pesquisa inválido.", nameof(campo));
            }

            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Nome, Descricao, Marca, Medida,
                         PrecoCusto, PrecoVenda, Estoque
                  FROM Produto
                  WHERE " + campo + @" LIKE @Filtro
                  ORDER BY Nome");

            cmd.Parameters.Add("@Filtro", SqlDbType.VarChar, 200).Value =
                "%" + (filtro ?? "") + "%";

            return ConverterProdutos(dataBaseService.GetDataTable(cmd));
        }

        private Produto ConverterProduto(DataRow linha)
        {
            return new Produto
            {
                Id = Convert.ToInt32(linha["Id"]),
                Nome = Convert.ToString(linha["Nome"]),
                Descricao = linha["Descricao"] == DBNull.Value
                    ? null : Convert.ToString(linha["Descricao"]),
                Marca = linha["Marca"] == DBNull.Value
                    ? null : Convert.ToString(linha["Marca"]),
                Medida = linha["Medida"] == DBNull.Value
                    ? null : Convert.ToString(linha["Medida"]),
                PrecoCusto = Convert.ToDecimal(linha["PrecoCusto"]),
                PrecoVenda = Convert.ToDecimal(linha["PrecoVenda"]),
                Estoque = Convert.ToInt32(linha["Estoque"])
            };
        }

        private ProdutoCollection ConverterProdutos(DataTable tabela)
        {
            ProdutoCollection produtos = new ProdutoCollection();

            foreach (DataRow linha in tabela.Rows)
                produtos.Add(ConverterProduto(linha));

            return produtos;
        }
    }
}