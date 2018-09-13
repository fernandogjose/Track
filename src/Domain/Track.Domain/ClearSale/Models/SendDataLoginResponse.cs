namespace Track.Domain.ClearSale.Models
{
    public class SendDataLoginResponse
    {
        public string RequestId { get; set; }

        public SendDataLoginAccount Account { get; set; }
    }

    public class SendDataLoginAccount
    {
        public string Code { get; set; }
    }
}