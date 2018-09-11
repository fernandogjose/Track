using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Data.Sql.ConfigurationData.Repositories
{
    public class ConfigurationDataSqlRepository : BaseSqlRepository, IConfigurationDataSqlRepository {

        private string GetByKeySql = "SELECT Nome, Valor FROM DadosConfiguracao WHERE Nome = @Nome";

        public ConfigurationDataSqlRepository (string connectionString) : base (connectionString) { }

        public Configuration GetByKey (string key) {

            Configuration configurationDataResponse = new Configuration ();

            using (SqlConnection conn = new SqlConnection (GetConnectionString ())) {
                using (var cmd = new SqlCommand ()) {
                    cmd.Connection = conn;
                    cmd.CommandText = GetByKeySql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue ("@Nome", GetDbValue (key));

                    conn.Open ();
                    using (DbDataReader dr = cmd.ExecuteReader ()) {
                        if (dr.Read ()) {
                            configurationDataResponse._id = dr["Nome"].ToString ().ToUpper ();
                            configurationDataResponse.Nome = dr["Nome"].ToString ();
                            configurationDataResponse.Valor = dr["Valor"].ToString ();
                            configurationDataResponse.DataMudanca = DateTime.Now;
                        }
                    }
                }
            }

            return configurationDataResponse;
        }
    }
}