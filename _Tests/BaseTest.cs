using DotNet.Library.Services.Azure;
using DotNet.Library.Services.Azure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// This test method left here to stub out how to easily perform validation on services from the Services project
        /// </summary>
        [TestMethod]
        public async Task Batching()
        {
            // comment this line to execute this test
            Assert.Inconclusive();

            var adoOptions = new List<DevOpsServiceOptions>
            {
                new() { Name = "My First One", Organization = "my_first_one", PersonalAccessToken = "<PAT HERE>" },
                new() { Name = "Another", Organization = "another", PersonalAccessToken = "<PAT HERE>" }
            };

            var kvOptions = new KeyVaultServiceOptions
            {
                ClientId = "<CLIENT ID HERE>",
                ClientKey = "<CLIENT KEY HERE>",
                CacheDuration = 0
            };

            ILogger<KeyVaultService> kvLogger = LoggerFactory.CreateLogger<KeyVaultService>();
            var kv = new KeyVaultService(kvLogger, Options.Create(kvOptions));
            var ado = new DevOpsService(Options.Create(adoOptions), kv);

            const string wiql = "select [system.id], [system.iterationpath] from workitems where [system.areapath] under '<AREA PATH>'";
            await ado.GetWorkItemsByWiqlAsync(wiql, "<PROJECT NAME>", "my_first_one", cancellationToken: default);
        }
    }
}
