using System;
using System.Linq;
using DialogWeaver.WebApi.Services;
using Xunit;

namespace DialogWeaver.WebApi.Tests
{
    /// <summary>
    /// Value Service Tests
    /// </summary>
    public class ValueServiceTests
    {
        /// <summary>
        /// The System Under Tests
        /// </summary>
        private IValueService _sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueServiceTests"/> class.
        /// </summary>
        public ValueServiceTests()
        {
            _sut = new ValueService(Array.Empty<string>());
        }

        /// <summary>
        /// When GetAll is called after instantiation empty collection is returned.
        /// </summary>
        [Fact]
        public void WhenGetAllIsCalledAfterInstantiationEmptyCollectionIsReturned()
        {
            var actual = _sut.GetAll();

            Assert.Empty(actual);
        }

        /// <summary>
        /// Whens GetAll is called after adding a value the added value is returned.
        /// </summary>
        [Fact]
        public void WhenGetAllIsCalledAfterAddingAValueTheAddedValueIsReturned()
        {
            var valueToAdd = Guid.NewGuid().ToString();

            _sut.Add(valueToAdd);

            var actual = _sut.GetAll();

            Assert.Equal(valueToAdd, actual.Single());
        }

        /// <summary>
        /// Whens GetAll is called after clearing values empty collection is returned.
        /// </summary>
        [Fact]
        public void WhenGetAllIsCalledAfterClearingValuesEmptyCollectionIsReturned()
        {
            var valueToAdd = Guid.NewGuid().ToString();

            _sut.Add(valueToAdd);

            var actual = _sut.GetAll();

            Assert.Equal(valueToAdd, actual.Single());

            _sut.Clear();

            actual = _sut.GetAll();

            Assert.Empty(actual);
        }
    }
}