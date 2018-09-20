using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Domain.User.Models;

namespace Track.Data.Sql.User.Repositories
{
    public class UserSqlRepository : BaseSqlRepository, IUserSqlRepository {

        private string GetByKeySql = "SELECT IdCliente, SessionId FROM Cliente WHERE Email = @Email";

        public UserSqlRepository (string connectionString) : base (connectionString) { }

        public GetUserIdAndSessionIdByEmailResponse GetUserIdAndSessionIdByEmail(GetUserIdAndSessionIdByEmailRequest getUserIdAndSessionIdByEmailRequest)
        {
            GetUserIdAndSessionIdByEmailResponse getUserIdAndSessionIdByEmailResponse = new GetUserIdAndSessionIdByEmailResponse ();

            using (SqlConnection conn = new SqlConnection (GetConnectionString ())) {
                using (var cmd = new SqlCommand ()) {
                    cmd.Connection = conn;
                    cmd.CommandText = GetByKeySql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue ("@Email", GetDbValue (getUserIdAndSessionIdByEmailRequest.Email));

                    conn.Open ();
                    using (DbDataReader dr = cmd.ExecuteReader ()) {
                        if (dr.Read ()) {
                            getUserIdAndSessionIdByEmailResponse.UserId = Convert.ToInt32(dr["IdCliente"]);
                            getUserIdAndSessionIdByEmailResponse.SessionId = dr["SessionId"].ToString();
                        }
                    }
                }
            }

            return getUserIdAndSessionIdByEmailResponse;
        }
    }
}