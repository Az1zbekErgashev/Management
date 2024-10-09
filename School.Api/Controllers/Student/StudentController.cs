﻿using Microsoft.AspNetCore.Mvc;
using School.Domain.Models.Response;
using School.Domain.Models.Student;
using School.Service.DTOs.Student;
using School.Service.Extencions;
using School.Service.Interfaces.Student;

namespace School.Api.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet("get/all")]
        [ProducesResponseType(typeof(ResponseModel<StudentModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<>), StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetAsync() => ResponseHandler.ReturnIActionResponse(await studentService.GetAsync());


        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseModel<StudentModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<>), StatusCodes.Status400BadRequest)]

        public async ValueTask<IActionResult> CreateAsync(StudentForCreationDTO @dto) => ResponseHandler.ReturnIActionResponse(await studentService.CreateAsync(@dto));


        [HttpGet("get")]
        [ProducesResponseType(typeof(ResponseModel<StudentModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<>), StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetByIdAsync(int id) => ResponseHandler.ReturnIActionResponse(await studentService.GetById(id));


        [HttpPost("update")]
        [ProducesResponseType(typeof(ResponseModel<StudentModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<>), StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> UpdateAsync(int id, StudentForCreationDTO @dto) => ResponseHandler.ReturnIActionResponse(await studentService.UpdateAsync(id, @dto));

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<>), StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> DeleteAsync(int id) => ResponseHandler.ReturnIActionResponse(await studentService.DeleteAsync(id));

    }
}
