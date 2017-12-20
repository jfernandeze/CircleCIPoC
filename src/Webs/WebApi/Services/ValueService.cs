using System.Collections.Generic;

namespace DialogWeaver.WebApi.Services
{
    /// <summary>
    /// Value Service
    /// </summary>
    /// <seealso cref="DialogWeaver.WebApi.Services.IValueService" />
    public class ValueService : IValueService
    {
        /// <summary>
        /// The values
        /// </summary>
        private static List<string> _values = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueService"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public ValueService(IReadOnlyCollection<string> values)
        {
            _values = new List<string>(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueService"/> class.
        /// </summary>
        public ValueService()
        {
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> GetAll()
        {
            return _values;
        }

        /// <inheritdoc />
        public void Add(string value)
        {
            _values.Add(value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _values.Clear();
        }
    }
}