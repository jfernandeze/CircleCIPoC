using System;
using System.Linq;
using System.Net;
using DialogWeaver.WebApi.Controllers;
using DialogWeaver.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DialogWeaver.WebApi.Tests
{
    public class ValueControllerTests : IDisposable
    {
        /// <summary>
        /// The System Under Tests
        /// </summary>
        private readonly ValueController _sut;

        /// <summary>
        /// The value service
        /// </summary>
        private readonly Mock<IValueService> _valueService;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _valueService.VerifyAll();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueControllerTests"/> class.
        /// </summary>
        public ValueControllerTests()
        {
            _valueService = new Mock<IValueService>(MockBehavior.Strict);

            _sut = new ValueController(_valueService.Object);
        }

        /// <summary>
        /// Whens GetAll is called return the values from service.
        /// </summary>
        [Fact]
        public void WhenGetAllIsCalledReturnTheValuesFromService()
        {
            var expected = Enumerable.Range(1, 10)
                .Select(i => Guid.NewGuid().ToString())
                .ToArray();

            _valueService.Setup(x => x.GetAll()).Returns(expected);

            var actual = _sut.GetAll() as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode.Value);
            Assert.Equal(expected, actual.Value);
        }

        /// <summary>
        /// Whens GetAll is called exceptions inside service are unhandled.
        /// </summary>
        [Fact]
        public void WhenGetAllIsCalledExceptionsInsideServiceAreUnhandled()
        {
            Exception expectedException = new InvalidOperationException();
            _valueService.Setup(x => x.GetAll()).Throws(expectedException);

            var actualException = Assert.Throws<InvalidOperationException>(() =>_sut.GetAll());
            Assert.Equal(expectedException, actualException);
        }
    }
}