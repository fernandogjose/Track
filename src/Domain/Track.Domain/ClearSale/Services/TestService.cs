using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;

namespace Track.Domain.ClearSale.Services {
    
    public class TestService : ITestService {
    
        private readonly ITestProxy _testProxy;

        public TestService (ITestProxy testProxy) {
            _testProxy = testProxy;
        }

        public void GetTest () {
            _testProxy.GetTest();
        }
    }
}