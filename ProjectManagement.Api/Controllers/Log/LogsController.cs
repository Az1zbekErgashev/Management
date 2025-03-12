using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Service.DTOs.Log;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.Log;

namespace ProjectManagement.Api.Controllers.Log
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService logService;
        public LogsController(ILogService logService)
        {
            this.logService = logService;
        }

        [HttpGet("filter")]
        [Authorize]
        public async ValueTask<IActionResult> GetAlllogsAsync([FromQuery] LogsForFilterDTO model) => ResponseHandler.ReturnIActionResponse(await logService.GetAsync(model));
    }
}
