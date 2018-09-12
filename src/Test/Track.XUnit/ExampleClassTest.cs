using Microsoft.Extensions.DependencyInjection;
using Moq;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Services;
using Xunit;

namespace Track.XUnit
{
    public class ExampleClassTest
    {
        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<ITestProxy> _testProxyMock;

        private readonly ITestService _testService;

        public ExampleClassTest()
        {
            //--- configuração do DI
            _serviceCollection = new ServiceCollection();
            _testProxyMock = new Mock<ITestProxy>();
            _serviceCollection.AddSingleton<ITestProxy>(_testProxyMock.Object);
            _serviceCollection.AddSingleton<ITestService, TestService>();

            //--- obter o service
            var services = _serviceCollection.BuildServiceProvider();
            _testService = services.GetService<ITestService>();
        }

        [Fact]
        public void EnviarRequestVazio () {
            _testProxyMock
                .Setup(r => r.GetTest());

            _testService.GetTest();
        }
    }
}