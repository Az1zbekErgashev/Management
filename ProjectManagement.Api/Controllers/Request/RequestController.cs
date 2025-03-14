using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.Request;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestStatusService requestStatusService;

        public RequestController(IRequestStatusService requestStatusService)
        {
            this.requestStatusService = requestStatusService;
        }

        [HttpGet("category")]
        public async ValueTask<IActionResult> GetAsync() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetAsync());


        [HttpGet("requets")]
        public async ValueTask<IActionResult> GetRequestsAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequeststAsync(dto));



        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync(RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateAsync(dto));



        [HttpPut("update")]
        public async ValueTask<IActionResult> UpdateAsync([Required] int id, RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateAsync(id, dto));



        [HttpDelete("delete")]
        public async ValueTask<IActionResult> DeleteAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteAsync(id)); 
        

        [HttpPost("create-request-many")]
        public async ValueTask<IActionResult> CreateManyRequest(List<RequestForCreateDTO> dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateRequestAsync(dto));
    }
}
