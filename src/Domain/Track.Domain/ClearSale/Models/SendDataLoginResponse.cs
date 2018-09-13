namespace Track.Domain.ClearSale.Models
{
    public class SendDataLoginResponse
    {
        public string RequestId { get; set; }

        public SendDataLoginAccountResponse Account { get; set; }
    }
}