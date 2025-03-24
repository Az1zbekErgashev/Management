using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using ProjectManagement.Service.Service.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using F23.StringSimilarity;
using ClosedXML.Excel;
using System.Globalization;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestStatusService requestStatusService;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> genericRepository;
        private readonly ProjectManagementDB _context;

        public RequestController(IRequestStatusService requestStatusService, IGenericRepository<Domain.Entities.Requests.Request> genericRepository, ProjectManagementDB context)
        {
            this.requestStatusService = requestStatusService;
            this.genericRepository = genericRepository;
            _context = context;
        }

        [HttpGet("category")]
        [Authorize]
        public async ValueTask<IActionResult> GetAsync() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetAsync());


        [HttpGet("requets")]
        [Authorize]
        public async ValueTask<IActionResult> GetRequestsAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequeststAsync(dto));



        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync([FromBody] RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateAsync(dto));



        [HttpPut("update")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateAsync([Required] int id, RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateAsync(id, dto));



        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteAsync(id));


        [HttpPost("create-request-many")]
        public async ValueTask<IActionResult> CreateManyRequest([FromForm] ForCreateManyRequest dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateRequestAsync(dto.RequestStatusId));

        [HttpDelete("delete-all-data")]
        public async ValueTask<IActionResult> DeleteManyRequest()
        {
            var alldata = await genericRepository.GetAll().ToListAsync();

            foreach (var item in alldata)
            {
                await genericRepository.DeleteAsync(item.Id);
            }

            await genericRepository.SaveChangesAsync();

            return ResponseHandler.ReturnIActionResponse("true");
        }

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

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel(int? requestCategoryId, int languageId = 0)
        {
            var query = genericRepository.GetAll().OrderBy(x => x.Id).AsQueryable();

            if (requestCategoryId is not null) query = query.Where(x => x.RequestStatusId == requestCategoryId);

            var requests = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Requests");


            var headers = languageId == 1
               ? new string[] { "No.", "Date", "Inquiry Type", "Company Name", "Department", "Responsible Person", "Inquiry Field", "Client Company", "Project Details", "Client", "Contact Number", "Email", "Processing Status", "Final Result", "Notes" }
               : new string[] { "번호", "접수일", "문의유형", "기업명", "담당부서", "담당자명", "문의분야", "고객사 회사", "프로젝트 내용", "고객사", "연락처", "이메일", "대응 상황", "최종 결과", "비고 (최종결과 사유)" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#3A70B3");
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
            }
            worksheet.Row(1).Height = 20;
            int row = 2;
            int index = 1;
            foreach (var request in requests)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = request.Date;
                worksheet.Cell(row, 3).Value = request.InquiryType;
                worksheet.Cell(row, 4).Value = request.CompanyName;
                worksheet.Cell(row, 5).Value = request.Department;
                worksheet.Cell(row, 6).Value = request.ResponsiblePerson;
                worksheet.Cell(row, 7).Value = request.InquiryField;
                worksheet.Cell(row, 8).Value = request.ClientCompany;
                worksheet.Cell(row, 9).Value = request.ProjectDetails;
                worksheet.Cell(row, 10).Value = request.Client;
                worksheet.Cell(row, 11).Value = request.ContactNumber;
                worksheet.Cell(row, 12).Value = request.Email;
                worksheet.Cell(row, 13).Value = request.ProcessingStatus;
                worksheet.Cell(row, 14).Value = request.FinalResult;
                worksheet.Cell(row, 15).Value = request.Notes;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
        }
    }
}
