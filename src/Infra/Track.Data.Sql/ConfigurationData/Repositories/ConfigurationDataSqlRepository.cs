using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Data.Sql.ConfigurationData.Repositories {
    public class ConfigurationDataSqlRepository : BaseSqlRepository, IConfigurationDataSqlRepository {

        private string GetByKeySql = "SELECT Nome, Valor FROM DadosConfiguracao WHERE Nome = @Nome";

        public ConfigurationDataSqlRepository (string connectionString) : base (connectionString) { }

        public Configuration GetByKey (string key) {

            try {

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

            } catch (Exception ex) {
                string innerException = "";
                if (ex.InnerException != null) {
                    innerException = ex.InnerException.Message;
                }
                string message = $"Message: {ex.Message} - Inner Exception: {innerException}";
                throw new CustomException (message, HttpStatusCode.NotAcceptable, "Track.Data.Sql.ConfigurationData.Repositories.ConfigurationDataSqlRepository", "GetByKey");
            }

        }
    }
}