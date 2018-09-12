using System;
using Xunit;
using Track.Domain.ClearSale.Models;

namespace Track.XUnit
{
     
    public class UnitTest1
    {
        [Fact]
        public void EnviarRequestVazio()
        {
            SendDataLoginResponse sendDataLoginResponse = new SendDataLoginResponse();

            int expectedStatusCode = 200;

            
        }
    }
}
