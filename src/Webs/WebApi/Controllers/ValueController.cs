using System;
using DialogWeaver.WebApi.Services;
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
        /// <summary>
        /// The value service
        /// </summary>
        private readonly IValueService _valueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueController"/> class.
        /// </summary>
        /// <param name="valueService">The value service.</param>
        /// <exception cref="ArgumentNullException">valueService</exception>
        public ValueController(IValueService valueService)
        {
            _valueService = valueService ?? throw new ArgumentNullException(nameof(valueService));
        }

        /// <summary>
        /// Get all values
        /// </summary>
        /// <returns>All values</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_valueService.GetAll());
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        [HttpPost("{value}")]
        public IActionResult Add([FromRoute]string value)
        {
            _valueService.Add(value);

            return Accepted();
        }
    }
}