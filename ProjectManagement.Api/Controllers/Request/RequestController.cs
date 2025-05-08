using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProjectManagement.Domain.Entities.Requests;
using DocumentFormat.OpenXml.Bibliography;
using ProjectManagement.Service.Service.Requests;

namespace ProjectManagement.Api.Controllers.Request
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestStatusService requestStatusService;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> genericRepository;
        private readonly IGenericRepository<Domain.Entities.Requests.ProcessingStatus> processingStatusRepository;
        private readonly ProjectManagementDB _context;

        public RequestController(IRequestStatusService requestStatusService, IGenericRepository<Domain.Entities.Requests.Request> genericRepository, ProjectManagementDB context, IGenericRepository<ProcessingStatus> processingStatusRepository)
        {
            this.requestStatusService = requestStatusService;
            this.genericRepository = genericRepository;
            _context = context;
            this.processingStatusRepository = processingStatusRepository;
        }

        [HttpGet("category")]
        public async ValueTask<IActionResult> GetAsync() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetAsync());


        [HttpGet("requets")]
        public async ValueTask<IActionResult> GetRequestsAsync([FromQuery] RequestForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequeststAsync(dto));



        [HttpGet("requets/{id}")]
        public async ValueTask<IActionResult> GetRequestsByIdAsync(int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequestById(id));


        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync([FromBody] RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateAsync(dto));


        [HttpPut("update")]
        public async ValueTask<IActionResult> UpdateAsync([Required] int id, RequestStatusForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateAsync(id, dto));


        [HttpDelete("delete")]
        public async ValueTask<IActionResult> DeleteAsync([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteAsync(id));

        [HttpPost("create-request")]
        public async ValueTask<IActionResult> CreateRequest(RequestForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.CreateRequest(dto));


        [HttpDelete("delete-request")]
        public async ValueTask<IActionResult> DeleteRequest([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.DeleteRequest(id));

        [HttpPut("recover-request")]
        public async ValueTask<IActionResult> RecoverRequest([Required] int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.RecoverRequest(id));

        [HttpPut("update-request")]
        public async ValueTask<IActionResult> UpdateRequest([Required] int id, RequestForCreateDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UpdateRequest(id, dto));


        [HttpGet("filter-values")]
        public async ValueTask<IActionResult> GetFilterValue([FromQuery] RequestStatusForFilterDTO dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetFilterValue(dto));


        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel(int? requestCategoryId, int languageId = 0)
        {
            var query = genericRepository.GetAll(x => x.IsDeleted == 0).Include(x => x.ProcessingStatus).OrderBy(x => x.Id).AsQueryable();

            if (requestCategoryId is not null) query = query.Where(x => x.RequestStatusId == requestCategoryId);

            var requests = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Requests");

            var headers = languageId == 1
               ? new string[] { "구분", "접수일", 
                   "문의 유형", "기업명", "담당부서",
                   "담당자", "문의분야", "고객사 회사",
                   "프로젝트 내용", "고객사", "연락처",
                   "이메일", "대응 상황", "비고",
                   "메모" }
               : new string[] { "구분", "접수일", 
                   "문의 유형", "기업명", "담당부서",
                   "담당자", "문의분야", "고객사 회사",
                   "프로젝트 내용", "고객사", "연락처",
                   "이메일", "대응 상황", "비고",
                   "메모" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#3A70B3");
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                worksheet.Column(i + 1).Width = 20;
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
                worksheet.Cell(row, 13).Value = request.Status;
                worksheet.Cell(row, 14).Value = request?.ProcessingStatus?.Text;
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
                    { "최종 업데이트", "Last Updated" },
                    { "문의 유형", "Inquiry Type" },
                    { "기업명", "Company Name" },
                    { "담당부서", "Department" },
                    { "담당자", "Responsible Person" },
                    { "문의분야", "Inquiry Field" },
                    { "고객사 회사", "Client Company" },
                    { "프로젝트 내용", "Project Details" },
                    { "고객사", "Client" },
                    { "연락처", "Contact Number" },
                    { "이메일", "Email" },
                    { "대응 상황", "Status" },
                    { "비고", "Detailed Reason" },
                    { "메모", "Notes" }
                };

                var columnIndexes = new Dictionary<string, int>();

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    foreach (var cell in row.Cells)
                    {
                        if (cell != null && (cell.ToString().Trim().Contains("접수일") || cell.ToString().Trim().Contains("Date")))
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
                        LastUpdated = GetSafeCellValue(currentRow, columnIndexes, "Last Updated"),
                        Date = GetSafeCellValue(currentRow, columnIndexes, "Date"),
                        InquiryType = GetSafeCellValue(currentRow, columnIndexes, "Inquiry Type"),
                        CompanyName = existCategory.Title,
                        Department = GetSafeCellValue(currentRow, columnIndexes, "Department"),
                        ResponsiblePerson = GetSafeCellValue(currentRow, columnIndexes, "Responsible Person"),
                        InquiryField = GetSafeCellValue(currentRow, columnIndexes, "Inquiry Field"),
                        ClientCompany = GetSafeCellValue(currentRow, columnIndexes, "Client Company"),
                        ProjectDetails = GetSafeCellValue(currentRow, columnIndexes, "Project Details"),
                        Client = GetSafeCellValue(currentRow, columnIndexes, "Client"),
                        ContactNumber = GetSafeCellValue(currentRow, columnIndexes, "Contact Number"),
                        Email = GetSafeCellValue(currentRow, columnIndexes, "Email"),
                        Status = GetSafeCellValue(currentRow, columnIndexes, "Status"),
                        ProcessingStatusId = await ProccesStatus(GetSafeCellValue(currentRow, columnIndexes, "Detailed Reason")),
                        Notes = GetSafeCellValue(currentRow, columnIndexes, "Notes"),
                        RequestStatusId = requestStatusId,
                        CreatedAt = DateTime.UtcNow,
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
                    if (cell != null && (key == "Date" || key == "Last Updated") && cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime? dateValue = cell.DateCellValue;
                        return dateValue?.Date.ToString("yyyy.MM.dd") ?? "";
                    }
                    return cell != null ? cell.ToString().Trim() : "";
                }
            }
            return "";
        }

        async ValueTask<int?> ProccesStatus(string? text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var existStatus = await processingStatusRepository.GetAsync(x => x.Text == text);
            if (existStatus == null)
            {
                var newExistStatus = new ProcessingStatus
                {
                    Color = "#000000",
                    Text = text
                };
                await processingStatusRepository.CreateAsync(newExistStatus);
                await processingStatusRepository.SaveChangesAsync();
                return newExistStatus.Id;
            }
            else
            {
                return existStatus.Id;
            }
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


        [HttpGet("request-procent")]
        public async Task<IActionResult> GetProcent() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetRequestProcent());
        
        
        [HttpGet("request-status-count")]
        public async Task<IActionResult> GetStatusCounts() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetStatusCounts());
        
        [HttpGet("request-status-years")]
        public async Task<IActionResult> GetStatusYears() => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetAvailableYears()); 
        
        [HttpGet("request-pie-chart")]
        public async Task<IActionResult> GetPieChartData(int? year, int? month) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetPieChartData(year, month));     
        
        [HttpGet("request-line-chart")]
        public async Task<IActionResult> GetLineChartData(int? year, int? month) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetLineChartData(year, month));
        
        [HttpGet("request-line-by-status-chart")]
        public async Task<IActionResult> GetLineByStatusChartData(int? year, string status) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetLineByStatusChartData(year, status));

        [HttpDelete("delete-deleted-request")]
        public async ValueTask<IActionResult> HardDeleteDeletedRequest(List<int> ids) => ResponseHandler.ReturnIActionResponse(await requestStatusService.HardDeleteDeletedRequest(ids));
        
        
        [HttpDelete("soft-delete-open-request")]
        public async ValueTask<IActionResult> SoftDeleteOpenRequest(List<int> ids) => ResponseHandler.ReturnIActionResponse(await requestStatusService.SoftDeleteOpenRequest(ids)); 
        
        [HttpDelete("soft-recover-open-request")]
        public async ValueTask<IActionResult> SoftRecoverOpenRequest(List<int> ids) => ResponseHandler.ReturnIActionResponse(await requestStatusService.SoftRecoverOpenRequest(ids));

        [HttpPost("upload-file")]
        public async ValueTask<IActionResult> UploadFile([FromForm] CreateUploadData dto) => ResponseHandler.ReturnIActionResponse(await requestStatusService.UploadFile(dto));

        [HttpGet("get-uploaded-file")]
        public async ValueTask<IActionResult> GetUploadedFile([FromQuery]int id) => ResponseHandler.ReturnIActionResponse(await requestStatusService.GetUploadedFile(id));
    }
}
