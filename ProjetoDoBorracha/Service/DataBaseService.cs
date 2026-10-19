using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDoBorracha.Service
{
    public class DataBaseService
    {
            private readonly string connectionString =
                @"Data Source=.\SQLEXPRESS; Initial Catalog=BorrachariaDoBorracha; Integrated Security=SSPI;";

            private SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }

            public int ExecuteSql(SqlCommand cmd)
            {
                using (SqlConnection connection = GetConnection())
                {
                    cmd.Connection = connection;

                    connection.Open();

                    return cmd.ExecuteNonQuery();
                }
            }

            public DataTable GetDataTable(SqlCommand cmd)
            {
                using (SqlConnection connection = GetConnection())
                {
                    cmd.Connection = connection;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable tabela = new DataTable();

                        adapter.Fill(tabela);

                        return tabela;
                    }
                }
            }
        
    }
}

