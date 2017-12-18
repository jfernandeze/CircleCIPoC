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

        public ValueService(IReadOnlyCollection<string> values)
        {
            _values = new List<string>(values);
        }

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