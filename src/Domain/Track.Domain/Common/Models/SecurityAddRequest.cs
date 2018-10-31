using System;

namespace Track.Domain.Common.Models
{
    public class SecurityAddRequest
    {
        public string Token { get; set; }

        public DateTime UsadoEm { get; set; }
    }
}