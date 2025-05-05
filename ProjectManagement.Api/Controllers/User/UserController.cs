using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Service.DTOs.User;
using ProjectManagement.Service.DTOs.UserForCreationDTO;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.User;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpPost("login")]
        public async ValueTask<IActionResult> Login(UserForLoginDTO model) => ResponseHandler.ReturnIActionResponse(await userService.Login(model));


        [HttpPost("create-new-user")]
        [Authorize]
        public async ValueTask<IActionResult> CreateNewUser([FromForm] UserForCreationDTO model) => ResponseHandler.ReturnIActionResponse(await userService.CreateUser(model));


        [HttpGet("filter")]
        [Authorize]
        public async ValueTask<IActionResult> GetAllUserAsync([FromQuery] UserForFilterDTO model) => ResponseHandler.ReturnIActionResponse(await userService.GetAsync(model));


        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteOrRecover([Required] int userId, bool isHardDelete) => ResponseHandler.ReturnIActionResponse(await userService.DeleteUser(userId, isHardDelete));


        [HttpPatch("update")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateUser([FromForm] UserForUpdateDTO dto) => ResponseHandler.ReturnIActionResponse(await userService.UpdateUser(dto));


        [HttpGet("{id}")]
        [Authorize]
        public async ValueTask<IActionResult> GetByIdAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await userService.GetByIdAsync(id));

        [HttpGet("user-email")]
        [Authorize]
        public async ValueTask<IActionResult> GetUserEmailsync() => ResponseHandler.ReturnIActionResponse(await userService.GetUserEmails());

        [HttpGet("profile")]
        [Authorize]
        public async ValueTask<IActionResult> GetUserProfile() => ResponseHandler.ReturnIActionResponse(await userService.GetProfile());
    }
}
