using DotNet.Library.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DotNet.Library.Tests.Exceptions
{
    [TestClass]
    public class ExceptionTests : BaseExceptionTest
    {
        [TestMethod]
        public void BadRequestException_Throws_400Error()
        {
            try
            {
                ThrowException(new BadRequestException(string.Empty));
            }
            catch (BadRequestException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                ex.MessageHeader.Should().Be("Bad Request");
                ((int)ex.StatusCode).Should().Be(400);
            }
        }

        [TestMethod]
        public void NotAuthorizedException_Throws_401Error()
        {
            try
            {
                ThrowException(new NotAuthorizedException(string.Empty));
            }
            catch (NotAuthorizedException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                ex.MessageHeader.Should().Be("Not authorized");
                ex.MessageHeader.Should().NotBe("Not authorised");
                ((int)ex.StatusCode).Should().Be(401);
            }
        }

        [TestMethod]
        public void ForbiddenException_Throws_403Error()
        {
            try
            {
                ThrowException(new ForbiddenException(string.Empty));
            }
            catch (ForbiddenException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.Forbidden);
                ex.MessageHeader.Should().Be("Forbidden");
                ((int)ex.StatusCode).Should().Be(403);
            }
        }

        [TestMethod]
        public void EntityNotFoundException_Throws_404Error()
        {
            try
            {
                ThrowException(new EntityNotFoundException(string.Empty));
            }
            catch (EntityNotFoundException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.NotFound);
                ex.MessageHeader.Should().Be("Not found");
                ((int)ex.StatusCode).Should().Be(404);
            }
        }

        [TestMethod]
        public void BusinessException_Throws_409Error()
        {
            try
            {
                ThrowException(new BusinessException(string.Empty));
            }
            catch (BusinessException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.Conflict);
                ex.MessageHeader.Should().Be("Business rule violation");
                ((int)ex.StatusCode).Should().Be(409);
            }
        }

        [TestMethod]
        public void ConflictException_Throws_409Error()
        {
            try
            {
                ThrowException(new ConflictException(string.Empty));
            }
            catch (ConflictException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.Conflict);
                ex.MessageHeader.Should().Be("Conflict");
                ((int)ex.StatusCode).Should().Be(409);
            }
        }

        [TestMethod]
        public void DuplicateFoundException_Throws_409Error()
        {
            try
            {
                ThrowException(new DuplicateFoundException(string.Empty));
            }
            catch (DuplicateFoundException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.Conflict);
                ex.MessageHeader.Should().Be("Duplicate found");
                ((int)ex.StatusCode).Should().Be(409);
            }
        }

        [TestMethod]
        public void TechnicalException_Throws_500Error()
        {
            try
            {
                ThrowException(new TechnicalException(string.Empty));
            }
            catch (TechnicalException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
                ex.MessageHeader.Should().Be("Internal server error");
                ((int)ex.StatusCode).Should().Be(500);
            }
        }

        [TestMethod]
        public void MethodNotImplemented_Throws_501Error()
        {
            try
            {
                ThrowException(new MethodNotAvailableException(string.Empty));
            }
            catch (MethodNotAvailableException ex)
            {
                ex.StatusCode.Should().Be(HttpStatusCode.NotImplemented);
                ex.MessageHeader.Should().Be("Method not available");
                ((int)ex.StatusCode).Should().Be(501);
            }
        }
    }
}
