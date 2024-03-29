using Microsoft.AspNetCore.Mvc;

namespace Todo.Process.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        
        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Info")]
        public IActionResult Get()
        {
            _logger.LogInformation("Info called");
            return Ok(new { api = "todo-proc", version = "0.3"});
        }
    }
}
