using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace Grondo.Tests
{
    [TestCategory("Unit")]
    [TestClass]
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider { get; private set; } = null!;

        protected ILoggerFactory LoggerFactory { get; set; } = null!;

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = new ServiceCollection()
                .AddOptions()
                .AddLogging()
                .BuildServiceProvider();

            LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
