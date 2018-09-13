namespace Track.Domain.ClearSale.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        
        public string ExpirationDate { get; set; }
    }
}