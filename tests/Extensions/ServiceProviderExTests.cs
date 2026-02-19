using FluentAssertions;
using Grondo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class ServiceProviderExTests : BaseExtensionTest
    {
        [TestMethod]
        public async Task UseScoped_FuncWithResult_ResolvesServiceAndReturnsResult()
        {
            string result = await ServiceProvider.UseScopedAsync<ILoggerFactory, string>(factory =>
            {
                factory.Should().NotBeNull();
                return Task.FromResult("success");
            });

            result.Should().Be("success");
        }

        [TestMethod]
        public async Task UseScoped_FuncNoResult_ResolvesServiceAndExecutes()
        {
            bool executed = false;

            await ServiceProvider.UseScopedAsync<ILoggerFactory>(factory =>
            {
                factory.Should().NotBeNull();
                executed = true;
                return Task.CompletedTask;
            });

            executed.Should().BeTrue();
        }

        [TestMethod]
        public void UseScoped_Action_ResolvesServiceAndExecutes()
        {
            bool executed = false;

            ServiceProvider.UseScoped<ILoggerFactory>(factory =>
            {
                factory.Should().NotBeNull();
                executed = true;
            });

            executed.Should().BeTrue();
        }

        [TestMethod]
        public void UseScoped_NullProvider_ThrowsArgumentNullException()
        {
            IServiceProvider provider = null!;
            Action act = () => provider.UseScoped<ILoggerFactory>(factory => { });
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void UseScoped_NullAction_ThrowsArgumentNullException()
        {
            Action act = () => ServiceProvider.UseScoped<ILoggerFactory>((Action<ILoggerFactory>)null!);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}

