using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Service.DTOs.MultilingualText;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.MultilingualText;

namespace ProjectManagement.Api.Controllers.MultilingualText
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultilingualTextController : ControllerBase
    {
        private readonly IMultilingualTextInterface multilingualTextService;

        public MultilingualTextController(IMultilingualTextInterface multilingualTextService)
        {
            this.multilingualTextService = multilingualTextService;
        }

        [HttpPost("create-from-json")]
        public async ValueTask<IActionResult> PostDocumentAsync([FromForm] MultilingualTextForCreateJson dto) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.CreateFromJson(dto.File, dto.Language));


        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] SupportLanguage language) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.GetDictonary(language));

    }
}
