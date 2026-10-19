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
    public class VeiculoController
    {
        private readonly DataBaseService dataBaseService;

        public VeiculoController()
        {
            dataBaseService = new DataBaseService();
        }

        public int Inserir(Veiculo veiculo)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Veiculo
                  (Placa, Marca, Modelo, Ano, Cor, TipoVeiculo, IdCliente)
                  VALUES
                  (@Placa, @Marca, @Modelo, @Ano, @Cor, @TipoVeiculo, @IdCliente)");

            cmd.Parameters.Add("@Placa", SqlDbType.VarChar, 10).Value = veiculo.Placa;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 60).Value = veiculo.Marca;
            cmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 60).Value = veiculo.Modelo;
            cmd.Parameters.Add("@Ano", SqlDbType.Int).Value = veiculo.Ano;
            cmd.Parameters.Add("@Cor", SqlDbType.VarChar, 30).Value = veiculo.Cor;
            cmd.Parameters.Add("@TipoVeiculo", SqlDbType.VarChar, 40).Value = veiculo.TipoVeiculo;
            cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = veiculo.IdCliente;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Alterar(Veiculo veiculo)
        {
            SqlCommand cmd = new SqlCommand(
                @"UPDATE Veiculo
                  SET Placa = @Placa,
                      Marca = @Marca,
                      Modelo = @Modelo,
                      Ano = @Ano,
                      Cor = @Cor,
                      TipoVeiculo = @TipoVeiculo,
                      IdCliente = @IdCliente
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = veiculo.Id;
            cmd.Parameters.Add("@Placa", SqlDbType.VarChar, 10).Value = veiculo.Placa;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 60).Value = veiculo.Marca;
            cmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 60).Value = veiculo.Modelo;
            cmd.Parameters.Add("@Ano", SqlDbType.Int).Value = veiculo.Ano;
            cmd.Parameters.Add("@Cor", SqlDbType.VarChar, 30).Value = veiculo.Cor;
            cmd.Parameters.Add("@TipoVeiculo", SqlDbType.VarChar, 40).Value = veiculo.TipoVeiculo;
            cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = veiculo.IdCliente;

            return dataBaseService.ExecuteSql(cmd);
        }

        public int Excluir(int id)
        {
            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Veiculo WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            return dataBaseService.ExecuteSql(cmd);
        }

        public Veiculo GetById(int id)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Placa, Marca, Modelo, Ano, Cor, TipoVeiculo, IdCliente
                  FROM Veiculo
                  WHERE Id = @Id");

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable tabela = dataBaseService.GetDataTable(cmd);

            if (tabela.Rows.Count == 0)
                return null;

            return ConverterVeiculo(tabela.Rows[0]);
        }

        public VeiculoCollection GetAll()
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Placa, Marca, Modelo, Ano, Cor, TipoVeiculo, IdCliente
                  FROM Veiculo
                  ORDER BY Placa");

            return ConverterVeiculos(dataBaseService.GetDataTable(cmd));
        }

        public VeiculoCollection GetByFilters(
            string campo = "", string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(campo))
                return GetAll();

            switch (campo)
            {
                case "Placa":
                case "Marca":
                case "Modelo":
                case "Cor":
                case "TipoVeiculo":
                    break;
                default:
                    throw new ArgumentException("Campo de pesquisa inválido.", nameof(campo));
            }

            SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Placa, Marca, Modelo, Ano, Cor, TipoVeiculo, IdCliente
                  FROM Veiculo
                  WHERE " + campo + @" LIKE @Filtro
                  ORDER BY Placa");

            cmd.Parameters.Add("@Filtro", SqlDbType.VarChar, 200).Value =
                "%" + (filtro ?? "") + "%";

            return ConverterVeiculos(dataBaseService.GetDataTable(cmd));
        }

        private Veiculo ConverterVeiculo(DataRow linha)
        {
            return new Veiculo
            {
                Id = Convert.ToInt32(linha["Id"]),
                Placa = Convert.ToString(linha["Placa"]),
                Marca = Convert.ToString(linha["Marca"]),
                Modelo = Convert.ToString(linha["Modelo"]),
                Ano = Convert.ToInt32(linha["Ano"]),
                Cor = Convert.ToString(linha["Cor"]),
                TipoVeiculo = Convert.ToString(linha["TipoVeiculo"]),
                IdCliente = Convert.ToInt32(linha["IdCliente"])
            };
        }

        private VeiculoCollection ConverterVeiculos(DataTable tabela)
        {
            VeiculoCollection veiculos = new VeiculoCollection();

            foreach (DataRow linha in tabela.Rows)
                veiculos.Add(ConverterVeiculo(linha));

            return veiculos;
        }
    }
}