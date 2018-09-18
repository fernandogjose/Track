using System.Net;
using Track.Domain.Common.Exceptions;

namespace Track.Domain.User.Models {
    public class GetUserIdByEmailRequest {
        public string Email { get; private set; }

        public GetUserIdByEmailRequest (string email) {
            if (string.IsNullOrEmpty (email)) {
                throw new CustomException ($"e-mail é obrigatório", HttpStatusCode.BadRequest);
            }

            Email = email;
        }
    }
}