using System.Collections.Generic;

namespace DialogWeaver.WebApi.Services
{
    /// <summary>
    /// Service for values
    /// </summary>
    public interface IValueService
    {
        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>all values</returns>
        IReadOnlyCollection<string> GetAll();

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Add(string value);

        /// <summary>
        /// Cleans all values.
        /// </summary>
        void Clear();
    }
}