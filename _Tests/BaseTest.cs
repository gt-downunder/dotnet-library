using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace DotNet.Library.Tests
{
    [TestCategory("Unit")]
    [TestClass]
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected ILoggerFactory LoggerFactory { get; set; }

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
