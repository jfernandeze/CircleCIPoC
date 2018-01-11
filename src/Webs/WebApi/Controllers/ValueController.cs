using System;
using System.Collections.Generic;
using System.Net;
using DialogWeaver.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSwag;
using NSwag.Annotations;

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
        [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<string>))]
        public IActionResult GetAll()
        {
            return Ok(_valueService.GetAll());
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        [HttpPost("{value}")]
        [SwaggerResponse(HttpStatusCode.Accepted, typeof(void))]
        public IActionResult Add([FromRoute]string value)
        {
            _valueService.Add(value);

            return Accepted();
        }
    }
}