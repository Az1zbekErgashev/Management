using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Service.DTOs.MultilingualText;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.MultilingualText;
using System.ComponentModel.DataAnnotations;

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
        [Authorize]
        public async ValueTask<IActionResult> PostDocumentAsync([FromForm] MultilingualTextForCreateJson dto) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.CreateFromJson(dto.File, dto.Language));


        [HttpGet]
        public async ValueTask<IActionResult> GetDictonary([FromQuery] SupportLanguage language) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.GetDictonary(language));


        [HttpGet("all/translations")]
        [Authorize]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] UIContentGetAllAndSearchDTO dto) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.GetTranslations(dto));


        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync([Required] string key) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.DeleteOrRecoverAsync(key));

        [HttpPost("create")]
        [Authorize]
        public async ValueTask<IActionResult> CreateAsync(MultilingualTextForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.CreateAsync(dto));

        [HttpPut("update")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateAsync(MultilingualTextForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await multilingualTextService.UpdateAsync(dto));


    }
}
