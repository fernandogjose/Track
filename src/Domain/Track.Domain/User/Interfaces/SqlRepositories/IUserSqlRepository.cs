using Track.Domain.User.Models;

namespace Track.Domain.User.Interfaces.SqlRepositories
{
    public interface IUserSqlRepository
    {
         GetUserIdByEmailResponse GetUserIdByEmail (GetUserIdByEmailRequest getUserIdByEmailRequest);
    }
}