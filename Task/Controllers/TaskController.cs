using System;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TATask.Contracts;

namespace TATask.Controllers
{
    [ApiController]
    [Route("task")]
    [SwaggerTag("Task")]

    public class TaskController : ControllerBase
    {
        private const string DEFAULT_INVERT_TEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        private const int DEFAULT_ITEMS_COUNT = 1000;
        private const int DEFAULT_THREADS_COUNT = 20;
        private const string DEFAULT_FILE_URL = @"https://speed.hetzner.de/10GB.bin";
        private const int DEFAULT_ASSETS_LIST_SIZE = 100;

        private IStringTool StringTool { get; }
        private IThreadTask ThreadTask { get; }
        private IRemoteFile FileTool { get; }
        private IAssetQuery AssetQuery { get; }

        public TaskController(
            IStringTool stringTool,
            IThreadTask threadTask,
            IRemoteFile fileTool,
            IAssetQuery assetQuery)
        {
            StringTool = stringTool;
            ThreadTask = threadTask;
            FileTool = fileTool;
            AssetQuery = assetQuery;
        }

        // GET: task/invert
        /// <summary>
        /// Inverts provided or default string.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /task/invert?input=test
        ///
        /// </remarks>
        /// <param name="input">String to revert. If not set a default one will be used.</param>
        /// <response code="200">Returns inverted string.</response>
        [HttpGet("invert")]
        [Produces("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ContentResult> Invert([FromQuery]string input)
        {
            var inverted = await StringTool.Invert(input ?? DEFAULT_INVERT_TEXT);
            return base.Content(inverted, "text/plain", Encoding.UTF8);
        }

        // POST: task/run-parallel
        /// <summary>
        /// Triggers parallel execution task.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /task/run-parallel
        ///
        /// </remarks>
        /// <param name="itemsCount">Amount of parallel executions.</param>
        /// <param name="threadsCount">Threads amount which suppose to run those executions.</param>
        /// <response code="200">Returns inverted string.</response>
        [HttpGet("run-parallel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeSpan))]
        public Task<TimeSpan> RunParallel([FromQuery]int? itemsCount, [FromQuery]int? threadsCount)
        {
            return ThreadTask.Execute(itemsCount ?? DEFAULT_ITEMS_COUNT,  threadsCount ?? DEFAULT_THREADS_COUNT);
        }

        // GET: task/file-hash
        /// <summary>
        /// Retrieves file hash.
        /// </summary>
        /// <param name="url">Full url to the file.</param>
        /// <response code="200">Returns file hash.</response>
        /// <response code="404">Invalid file url.</response>
        [HttpGet("file-hash")]
        [Produces("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> GetFileHash([FromQuery]string url)
        {
            var hash = await FileTool.GetHash(url ?? DEFAULT_FILE_URL);
            if (string.IsNullOrEmpty(hash))
            {
                return base.NotFound();
            }
            return base.Content(hash, "text/plain", Encoding.UTF8);
        }
        
        // GET: task/assets
        /// <summary>
        /// Retrieve assets with prices.
        /// </summary>
        /// <param name="limit">Top by the market CAP.</param>
        /// <response code="200">Returns Assets with prices.</response>
        [HttpGet("assets")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public Task<Asset[]> GetFileHash([FromQuery]int? limit)
        {
            return AssetQuery.Execute(limit ?? DEFAULT_ASSETS_LIST_SIZE);
        }
    }
}