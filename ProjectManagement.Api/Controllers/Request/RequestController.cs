using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Globalization;
using ProjectManagement.Domain.Entities.Requests;
using NPOI.HSSF.Record.Aggregates;

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


        [HttpGet("deleted-requets")]
        [Authorize]
        public async ValueTask<IActionResult> GetDeletedRequeststAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetDeletedRequeststAsync(dto));


        [HttpGet("pending-requets")]
        [Authorize]
        public async ValueTask<IActionResult> GetPendingRequeststAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetPendingRequeststAsync(dto));



        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync([FromBody] RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateAsync(dto));



        [HttpPut("update")]
        [Authorize]
        public async ValueTask<IActionResult> UpdateAsync([Required] int id, RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateAsync(id, dto));



        [HttpDelete("delete")]
        [Authorize]
        public async ValueTask<IActionResult> DeleteAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteAsync(id));


        [HttpPut("change-pending-request")]
        [Authorize]
        public async ValueTask<IActionResult> ChangePendingAsync([Required] int id, bool status) => ResponseHandler.ReturnIActionResponse(await requestStatusService.ChangeRequestStatus(id, status));


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
        public async ValueTask<IActionResult> GetFilterValue([FromQuery] RequestStatusForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetFilterValue(dto));


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
            var query = genericRepository.GetAll(x => x.IsDeleted == 0).OrderBy(x => x.Id).AsQueryable();

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

            var requestStatus = await _context.RequestStatuses.FirstOrDefaultAsync(x => x.Id == requestCategoryId);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{(requestStatus != null ? requestStatus.Title : "All")}.xlsx");
        }


        [HttpGet("counts")]
        public async Task<IActionResult> GetCounts() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequestsCount());


        [HttpPost("upload")]
        public async ValueTask<IActionResult> UploadExcel(IFormFile file, int requestStatusId)
        {
            if (file == null || file.Length == 0 || requestStatusId == null)
            {
                return BadRequest("Файл не загружен.");
            }

            var existCategory = await _context.RequestStatuses.FirstOrDefaultAsync(x => x.Id == requestStatusId);

            if(existCategory is null) return BadRequest("Request Category Is Not Correct");

            using (var stream = file.OpenReadStream())
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                int rowCount = sheet.LastRowNum;
                int startRow = -1;

                var columnMapping = new Dictionary<string, string>
                {
                    { "접수일", "Date" },
                    { "최종 업데이트", "LastUpdated"},
                    { "유입경로", "InquiryType" },
                    { "법인명", "CompanyName" },
                    { "법인부서", "Department" },
                    { "담당자", "ResponsiblePerson" },
                    { "문의분야", "InquiryField" },
                    { "고객사 회사", "ClientCompany" },
                    { "프로젝트 내용", "ProjectDetails" },
                    { "고객사 담당자", "Client" },
                    { "고객사 연락처", "ContactNumber" },
                    { "고객사 이메일", "Email" },
                    { "처리 상태", "Status" },
                    { "상세사유", "DetailedReason" },
                    { "메모", "Notes" }
                };

                var columnIndexes = new Dictionary<string, int>();

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    foreach (var cell in row.Cells)
                    {
                        if (cell != null && cell.ToString().Trim().Contains("접수일"))
                        {
                            startRow = i + 1;
                            break;
                        }
                    }

                    if (startRow != -1) break;
                }

                if (startRow == -1)
                {
                    throw new Exception("Не найдены заголовки таблицы.");
                }

                IRow headerRow = sheet.GetRow(startRow - 1);
                if (headerRow != null)
                {
                    for (int j = 0; j < headerRow.LastCellNum; j++)
                    {
                        string columnName = headerRow.GetCell(j) != null ? headerRow.GetCell(j).ToString().Trim() : "";
                        if (columnMapping.ContainsKey(columnName))
                        {
                            columnIndexes[columnMapping[columnName]] = j;
                        }
                    }
                }

                var existingRecords = await _context.Requests.Where(x => x.RequestStatusId == requestStatusId).ToListAsync();
                if (existingRecords.Any())
                {
                    _context.Requests.RemoveRange(existingRecords);
                    await _context.SaveChangesAsync();
                }

                for (int row = startRow; row <= rowCount; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null) continue;

                    var record = new Domain.Entities.Requests.Request
                    {
                        Date = GetSafeCellValue(currentRow, columnIndexes, "Date"),
                        InquiryType = GetSafeCellValue(currentRow, columnIndexes, "InquiryType"),
                        CompanyName = GetSafeCellValue(currentRow, columnIndexes, "CompanyName"),
                        Department = GetSafeCellValue(currentRow, columnIndexes, "Department"),
                        ResponsiblePerson = GetSafeCellValue(currentRow, columnIndexes, "ResponsiblePerson"),
                        InquiryField = GetSafeCellValue(currentRow, columnIndexes, "InquiryField"),
                        ClientCompany = GetSafeCellValue(currentRow, columnIndexes, "ClientCompany"),
                        ProjectDetails = GetSafeCellValue(currentRow, columnIndexes, "ProjectDetails"),
                        Client = GetSafeCellValue(currentRow, columnIndexes, "Client"),
                        ContactNumber = GetSafeCellValue(currentRow, columnIndexes, "ContactNumber"),
                        Email = GetSafeCellValue(currentRow, columnIndexes, "Email"),
                        ProcessingStatus = GetSafeCellValue(currentRow, columnIndexes, "ProcessingStatus"),
                        FinalResult = GetSafeCellValue(currentRow, columnIndexes, "FinalResult"),
                        Notes = GetSafeCellValue(currentRow, columnIndexes, "Notes"),
                        RequestStatusId = requestStatusId,
                        AdditionalInformation = string.Empty,
                        Deadline = null,
                        Priority = GetSafeCellValue(currentRow, columnIndexes, "Client"),
                        CreatedAt = DateTime.UtcNow,
                        InquirySource = string.Empty,
                        Status = GetSafeCellValue(currentRow, columnIndexes, "Client"),
                        ProjectBudget = string.Empty,
                        LastUpdated = GetSafeCellValue(currentRow, columnIndexes, "LastUpdated")
                    };

                    await genericRepository.CreateAsync(record);
                }
            }

            await genericRepository.SaveChangesAsync();
            return Ok(new { message = "Файл успешно загружен" });
        }

        string GetSafeCellValue(IRow row, Dictionary<string, int> columnIndexes, string key)
        {
            if (columnIndexes.ContainsKey(key))
            {
                int index = columnIndexes[key];
                if (index >= 0 && index < row.LastCellNum)
                {
                    ICell cell = row.GetCell(index);
                    if (cell != null && (key == "Date" || key == "LastUpdated") && cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime? dateValue = cell.DateCellValue;
                        return dateValue?.Date.ToString("d", CultureInfo.InvariantCulture) ?? "";
                    }
                    return cell != null ? cell.ToString().Trim() : "";
                }
            }
            return "";
        }


        [HttpDelete("hard-delete")]
        public async Task<IActionResult> HardDeleteAsync()
        {
            var allRequests = await _context.Requests.ToListAsync();

            foreach(var item in allRequests)
            {
                _context.Requests.RemoveRange(item);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "File is deleted" });
        }
    }
}
