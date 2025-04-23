using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.Request;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IRequestStatusService requestStatusService;

        public CommentController(IRequestStatusService requestStatusService)
        {
            this.requestStatusService = requestStatusService;
        }

        [HttpGet("comments")]
        [Authorize]
        public async ValueTask<IActionResult> GetAsync([FromQuery]CommentsForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetCommentsAsync(dto));   
        
        [HttpGet("history")]
        [Authorize]
        public async ValueTask<IActionResult> GetHistoryAsync([FromQuery] CommentsForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequestHistoryAsync(dto));

        [HttpDelete("delete/comment")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync(int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteComment(id));


        [HttpPost("create/comment")]
        [Authorize]
        public async ValueTask<IActionResult> CreateComment(CommentForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateComment(dto));


        [HttpPut("update/comment")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateComment(CommentForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateComment(dto));
    }
}
