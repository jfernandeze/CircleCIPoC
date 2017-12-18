using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DialogWeaver.WebApi.Controllers
{
    /// <summary>
    /// Controller for Values
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/value")]
    public class ValueController : Controller
    {
        private static readonly IList<string> Values = new List<string>();

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>All values</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Values);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        [HttpPost("{value}")]
        public IActionResult Add([FromRoute]string value)
        {
            Values.Add(value);

            return Accepted();
        }
    }
}