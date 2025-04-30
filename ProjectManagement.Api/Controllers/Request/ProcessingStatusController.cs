using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.Request;
using ProjectManagement.Service.Service.Request;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProcessingStatusController
    {
        private readonly IProcessingStatusService processingStatusService;

        public ProcessingStatusController(IProcessingStatusService processingStatusService)
        {
            this.processingStatusService = processingStatusService;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] ProcessingStatusFilter dto) => ResponseHandler.ReturnIActionResponse(await processingStatusService.GetAllAsync(dto));
        
        [HttpPut]
        public async ValueTask<IActionResult> UpdateAsync(ProcessingStatusDTO dto) => ResponseHandler.ReturnIActionResponse(await processingStatusService.UpdateAsync(dto));  
        
        [HttpPost]
        public async ValueTask<IActionResult> CreateAsync(ProcessingStatusDTO dto) => ResponseHandler.ReturnIActionResponse(await processingStatusService.CreateAsync(dto));    
        
        [HttpDelete]
        public async ValueTask<IActionResult> DeleteAsync(int id) => ResponseHandler.ReturnIActionResponse(await processingStatusService.DeleteAsync(id)); 
        
        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetAsync(int id) => ResponseHandler.ReturnIActionResponse(await processingStatusService.GetByIdAsync(id));
    }
}
