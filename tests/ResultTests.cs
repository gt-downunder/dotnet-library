using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests
{
    [TestClass]
    [TestCategory("Unit")]
    public class ResultTests : BaseTest
    {
        [TestMethod]
        public void Success_CreatesSuccessfulResult()
        {
            var result = Result<int>.Success(42);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Should().Be(42);
        }

        [TestMethod]
        public void Failure_CreatesFailedResult()
        {
            var result = Result<int>.Failure("something went wrong");

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("something went wrong");
        }

        [TestMethod]
        public void Value_OnFailure_ThrowsInvalidOperationException()
        {
            var result = Result<int>.Failure("error");
            Func<int> act = () => result.Value;
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Error_OnSuccess_ThrowsInvalidOperationException()
        {
            var result = Result<int>.Success(1);
            Func<string> act = () => result.Error;
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ImplicitOperator_ConvertsValueToSuccess()
        {
            Result<string> result = "hello";

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("hello");
        }

        [TestMethod]
        public void GetValueOrDefault_OnSuccess_ReturnsValue()
        {
            var result = Result<int>.Success(42);
            result.GetValueOrDefault(0).Should().Be(42);
        }

        [TestMethod]
        public void GetValueOrDefault_OnFailure_ReturnsFallback()
        {
            var result = Result<int>.Failure("err");
            result.GetValueOrDefault(99).Should().Be(99);
        }

        [TestMethod]
        public void Map_OnSuccess_TransformsValue()
        {
            var result = Result<int>.Success(5);
            Result<int> mapped = result.Map(x => x * 2);

            mapped.IsSuccess.Should().BeTrue();
            mapped.Value.Should().Be(10);
        }

        [TestMethod]
        public void Map_OnFailure_PropagatesError()
        {
            var result = Result<int>.Failure("err");
            Result<int> mapped = result.Map(x => x * 2);

            mapped.IsFailure.Should().BeTrue();
            mapped.Error.Should().Be("err");
        }

        [TestMethod]
        public void Bind_OnSuccess_ChainsOperation()
        {
            var result = Result<int>.Success(10);
            Result<string> bound = result.Bind(x => Result<string>.Success(x.ToString()));

            bound.IsSuccess.Should().BeTrue();
            bound.Value.Should().Be("10");
        }

        [TestMethod]
        public void Bind_OnFailure_PropagatesError()
        {
            var result = Result<int>.Failure("err");
            Result<string> bound = result.Bind(x => Result<string>.Success(x.ToString()));

            bound.IsFailure.Should().BeTrue();
            bound.Error.Should().Be("err");
        }

        [TestMethod]
        public void ToResult_WrapsValueInSuccess()
        {
            var result = 42.ToResult();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(42);
        }

        [TestMethod]
        public void TryExecute_SuccessfulFunc_ReturnsSuccess()
        {
            Result<int> result = ResultEx.TryExecute(() => 42);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(42);
        }

        [TestMethod]
        public void TryExecute_ThrowingFunc_ReturnsFailure()
        {
            Result<int> result = ResultEx.TryExecute<int>(() => throw new InvalidOperationException("boom"));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("boom");
        }

        [TestMethod]
        public async Task TryExecuteAsync_SuccessfulFunc_ReturnsSuccess()
        {
            Result<int> result = await ResultEx.TryExecuteAsync(() => Task.FromResult(42));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(42);
        }

        [TestMethod]
        public async Task TryExecuteAsync_ThrowingFunc_ReturnsFailure()
        {
            Result<int> result = await ResultEx.TryExecuteAsync<int>(() => throw new InvalidOperationException("async boom"));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("async boom");
        }
    }
}

