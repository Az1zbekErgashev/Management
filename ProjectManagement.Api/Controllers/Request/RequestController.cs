using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestStatusService requestStatusService;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> genericRepository;

        public RequestController(IRequestStatusService requestStatusService, IGenericRepository<Domain.Entities.Requests.Request> genericRepository)
        {
            this.requestStatusService = requestStatusService;
            this.genericRepository = genericRepository;
        }

        [HttpGet("category")]
        [Authorize]
        public async ValueTask<IActionResult> GetAsync() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetAsync());


        [HttpGet("requets")]
        [Authorize]
        public async ValueTask<IActionResult> GetRequestsAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequeststAsync(dto));



        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync(RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateAsync(dto));



        [HttpPut("update")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateAsync([Required] int id, RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateAsync(id, dto));



        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteAsync(id));


        [HttpPost("create-request-many")]
        public async ValueTask<IActionResult> CreateManyRequest([FromForm] ForCreateManyRequest dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateRequestAsync(dto.RequestStatusId));

        [HttpPost("create-request")]
        [Authorize]
        public async ValueTask<IActionResult> CreateRequest(RequestForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateRequest(dto));


        [HttpDelete("delete-request")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteRequest([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteRequest(id));

        [HttpPut("recover-request")]
        [Authorize]
        public async ValueTask<IActionResult> RecoverRequest([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.RecoverRequest(id));

        [HttpPut("update-request")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateRequest([Required] int id, RequestForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateRequest(id, dto));


        [HttpGet("filter-values")]
        public async ValueTask<IActionResult> GetFilterValue() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetFilterValue());



        [HttpGet("get-json")]
        public async Task<IActionResult> GetJson()
        {
            var allRequests = await genericRepository.GetAll().ToListAsync();

            var json = JsonSerializer.Serialize(allRequests, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            var fileName = "requests.json";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            await System.IO.File.WriteAllTextAsync(filePath, json, Encoding.UTF8);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/json", fileName);
        }
    }
}
