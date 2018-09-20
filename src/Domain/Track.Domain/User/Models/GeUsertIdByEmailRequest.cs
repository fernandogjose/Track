using System.Net;
using Track.Domain.Common.Exceptions;

namespace Track.Domain.User.Models {
    public class GetUserIdAndSessionIdByEmailRequest {
        public string Email { get; private set; }

        public GetUserIdAndSessionIdByEmailRequest (string email) {
            if (string.IsNullOrEmpty (email)) {
                throw new CustomException ($"e-mail é obrigatório", HttpStatusCode.BadRequest, "Track.Domain.User.Models.GetUserIdByEmailRequest", "Constructor");
            }

            Email = email;
        }
    }
}