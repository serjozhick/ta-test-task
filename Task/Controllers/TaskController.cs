using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TATask.Contracts;

namespace TATask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Task")]

    public class TaskController : ControllerBase
    {
        private const string DEFAULT_INVERT_TEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        private IStringTool StringTool { get; }
        private IThreadTask ThreadTask { get; }

        public TaskController(
            IStringTool stringTool,
            IThreadTask threadTask)
        {
            StringTool = stringTool;
            ThreadTask = threadTask;
        }

        // GET: Task/Invert
        /// <summary>
        /// Inverts provided or default string.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Task/Invert?input=test
        ///
        /// </remarks>
        /// <param name="input">String to revert. If not set a default one will be used.</param>
        /// <response code="200">Returns inverted string.</response>
        [HttpGet("Invert")]
        [Produces("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ContentResult> Invert([FromQuery]string input)
        {
            var inverted = await StringTool.Invert(input ?? DEFAULT_INVERT_TEXT);
            return base.Content(inverted, "text/plain", Encoding.UTF8);
        }

        // POST: Task/RunParallel
        /// <summary>
        /// Triggers parallel execution.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Task/RunParallel
        ///
        /// </remarks>
        [HttpGet("RunParallel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task RunParallel()
        {
            return ThreadTask.Execute(1000, 20);
        }
    }
}