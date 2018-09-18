using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Domain.User.Models;

namespace Track.Data.Sql.User.Repositories
{
    public class UserSqlRepository : BaseSqlRepository, IUserSqlRepository {

        private string GetByKeySql = "SELECT IdCliente FROM Cliente WHERE Email = @Email";

        public UserSqlRepository (string connectionString) : base (connectionString) { }

        public GetUserIdByEmailResponse GetUserIdByEmail(GetUserIdByEmailRequest getUserIdByEmailRequest)
        {
            GetUserIdByEmailResponse getUserIdByEmailResponse = new GetUserIdByEmailResponse ();

            using (SqlConnection conn = new SqlConnection (GetConnectionString ())) {
                using (var cmd = new SqlCommand ()) {
                    cmd.Connection = conn;
                    cmd.CommandText = GetByKeySql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue ("@Email", GetDbValue (getUserIdByEmailRequest.Email));

                    conn.Open ();
                    using (DbDataReader dr = cmd.ExecuteReader ()) {
                        if (dr.Read ()) {
                            getUserIdByEmailResponse.UserId = Convert.ToInt32(dr["IdCliente"]);
                        }
                    }
                }
            }

            return getUserIdByEmailResponse;
        }
    }
}