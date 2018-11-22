using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Domain.User.Models;

namespace Track.Data.Sql.User.Repositories
{
    public class UserSqlRepository : BaseSqlRepository, IUserSqlRepository {

        public UserSqlRepository (string connectionString) : base (connectionString) { }

        public GetUserIdAndSessionIdByEmailResponse GetUserIdAndSessionIdByEmail(GetUserIdAndSessionIdByEmailRequest getUserIdAndSessionIdByEmailRequest)
        {
            GetUserIdAndSessionIdByEmailResponse getUserIdAndSessionIdByEmailResponse = new GetUserIdAndSessionIdByEmailResponse ();

            using (SqlConnection conn = new SqlConnection (GetConnectionString ())) {
                using (var cmd = new SqlCommand ("GetUserIdAndSessionIdByEmail")) {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
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