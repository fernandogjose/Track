namespace Track.Domain.User.Models
{
    public class GetUserIdAndSessionIdByEmailResponse
    {
        public int UserId { get; set; }

        public string SessionId { get; set; }
    }
}