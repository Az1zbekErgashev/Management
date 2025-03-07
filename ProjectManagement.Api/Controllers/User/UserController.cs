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
        public async ValueTask<IActionResult> CreateNewUser([FromBody] UserForCreationDTO model) => ResponseHandler.ReturnIActionResponse(await userService.CreateUser(model));


        [HttpGet("filter")]
        [Authorize]
        public async ValueTask<IActionResult> GetAllUserAsync([FromQuery] UserForFilterDTO model) => ResponseHandler.ReturnIActionResponse(await userService.GetAsync(model));


        [HttpGet("team-leaders")]
        [Authorize]
        public async ValueTask<IActionResult> GetAllUserAsync() => ResponseHandler.ReturnIActionResponse(await userService.GetTeamLeadrsName());



        [HttpGet("all-companys")]
        [Authorize]
        public async ValueTask<IActionResult> GetAllCompanies() => ResponseHandler.ReturnIActionResponse(await userService.GetCompanyName());



        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteOrRecover([Required] int userId) => ResponseHandler.ReturnIActionResponse(await userService.DeleteUser(userId));
    }
}
