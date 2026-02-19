using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class TaskExTests : BaseExtensionTest
    {
        [TestMethod]
        public async Task WithTimeout_Generic_CompletesBeforeTimeout_ReturnsResult()
        {
            int result = await Task.FromResult(42).WithTimeoutAsync(TimeSpan.FromSeconds(5), TestContext.CancellationToken);
            result.Should().Be(42);
        }

        [TestMethod]
        public async Task WithTimeout_Generic_ExceedsTimeout_ThrowsTimeoutException()
        {
            Func<Task<int>> act = async () => await Task.Delay(TimeSpan.FromSeconds(10), TestContext.CancellationToken)
                .ContinueWith(_ => 42)
                .WithTimeoutAsync(TimeSpan.FromMilliseconds(50), TestContext.CancellationToken);

            await act.Should().ThrowAsync<TimeoutException>();
        }

        [TestMethod]
        public async Task WithTimeout_NonGeneric_CompletesBeforeTimeout() => await Task.CompletedTask.WithTimeoutAsync(TimeSpan.FromSeconds(5), TestContext.CancellationToken);

        [TestMethod]
        public async Task WithTimeout_NonGeneric_ExceedsTimeout_ThrowsTimeoutException()
        {
            Func<Task> act = async () => await Task.Delay(TimeSpan.FromSeconds(10), TestContext.CancellationToken)
                .WithTimeoutAsync(TimeSpan.FromMilliseconds(50), TestContext.CancellationToken);

            await act.Should().ThrowAsync<TimeoutException>();
        }

        [TestMethod]
        public async Task OnFailure_Generic_TaskSucceeds_DoesNotCallHandler()
        {
            bool handlerCalled = false;

            int result = await Task.FromResult(42)
                .OnFailureAsync(_ => handlerCalled = true);

            result.Should().Be(42);
            handlerCalled.Should().BeFalse();
        }

        [TestMethod]
        public async Task OnFailure_Generic_TaskFails_CallsHandlerAndRethrows()
        {
            Exception? caught = null;

            Func<Task<int>> act = async () => await Task.FromException<int>(new InvalidOperationException("fail"))
                .OnFailureAsync(ex => caught = ex);

            await act.Should().ThrowAsync<InvalidOperationException>();
            caught.Should().NotBeNull();
            caught!.Message.Should().Be("fail");
        }

        [TestMethod]
        public async Task OnFailure_NonGeneric_TaskFails_CallsHandlerAndRethrows()
        {
            Exception? caught = null;

            Func<Task> act = async () => await Task.FromException(new InvalidOperationException("fail"))
                .OnFailureAsync(ex => caught = ex);

            await act.Should().ThrowAsync<InvalidOperationException>();
            caught.Should().NotBeNull();
        }

        [TestMethod]
        public async Task WhenAllSequential_ExecutesInOrder()
        {
            var order = new List<int>();

            await TaskEx.WhenAllSequentialAsync(
                () => Task.Run(() => order.Add(1), TestContext.CancellationToken),
                () => Task.Run(() => order.Add(2), TestContext.CancellationToken),
                () => Task.Run(() => order.Add(3), TestContext.CancellationToken)
            );

            order.Should().BeEquivalentTo([1, 2, 3]);
        }

        [TestMethod]
        public async Task WhenAllSequential_Generic_CollectsResults()
        {
            int[] results = await TaskEx.WhenAllSequentialAsync<int>(
                () => Task.FromResult(10),
                () => Task.FromResult(20),
                () => Task.FromResult(30)
            );

            results.Should().BeEquivalentTo([10, 20, 30]);
        }

        // --- RetryAsync<T> ---

        [TestMethod]
        public async Task RetryAsync_Generic_SucceedsFirstTry_ReturnsResult()
        {
            int result = await TaskEx.RetryAsync(() => Task.FromResult(42), maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));
            result.Should().Be(42);
        }

        [TestMethod]
        public async Task RetryAsync_Generic_FailsThenSucceeds_Retries()
        {
            int attempt = 0;
            int result = await TaskEx.RetryAsync(() =>
            {
                attempt++;
                if (attempt < 3) throw new InvalidOperationException("fail");
                return Task.FromResult(42);
            }, maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));

            result.Should().Be(42);
            attempt.Should().Be(3);
        }

        [TestMethod]
        public async Task RetryAsync_Generic_ExceedsRetries_Throws()
        {
            Func<Task> act = async () => await TaskEx.RetryAsync<int>(
                () => throw new InvalidOperationException("always fails"),
                maxRetries: 2, delay: TimeSpan.FromMilliseconds(1));

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        // --- RetryAsync (non-generic) ---

        [TestMethod]
        public async Task RetryAsync_NonGeneric_SucceedsFirstTry()
        {
            bool called = false;
            await TaskEx.RetryAsync(() => { called = true; return Task.CompletedTask; }, maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));
            called.Should().BeTrue();
        }

        [TestMethod]
        public async Task RetryAsync_NonGeneric_FailsThenSucceeds()
        {
            int attempt = 0;
            await TaskEx.RetryAsync(() =>
            {
                attempt++;
                if (attempt < 2) throw new InvalidOperationException("fail");
                return Task.CompletedTask;
            }, maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));

            attempt.Should().Be(2);
        }

        // --- FireAndForget ---

        [TestMethod]
        public async Task FireAndForget_SuccessfulTask_DoesNotThrow()
        {
            Task.CompletedTask.FireAndForget();
            await Task.Delay(50, TestContext.CancellationToken); // give continuation time to run
        }

        [TestMethod]
        public async Task FireAndForget_FailedTask_CallsErrorHandler()
        {
            Exception? caught = null;
            var tcs = new TaskCompletionSource();

            Task.FromException(new InvalidOperationException("boom"))
                .FireAndForget(ex =>
                {
                    caught = ex;
                    tcs.SetResult();
                });

            await tcs.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.CancellationToken);
            caught.Should().NotBeNull();
            caught!.Message.Should().Be("boom");
        }

        public TestContext TestContext { get; set; } = null!;
    }
}
