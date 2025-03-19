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


        [HttpPost("upload")]
        public async ValueTask<IActionResult> UploadExcel(IFormFile file, [FromForm] int requestStatusId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не загружен.");
            }

            using (var stream = file.OpenReadStream())
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                int rowCount = sheet.LastRowNum;
                int startRow = -1;
                int startIndex = 0;


                var columnMapping = new Dictionary<string, string>
                {
                    { "접수일", "Date" },
                    { "문의\n유형", "InquiryType" },  
                    { "문의 유형", "InquiryType" },
                    { "기업명", "CompanyName" },
                    { "담당부서", "Department" },
                    { "담당자명", "ResponsiblePerson" },
                    { "문의분야", "InquiryField" },
                    { "고객사 회사", "ClientCompany" },
                    { "프로젝트 내용", "ProjectDetails" },
                    { "고객사", "Client" },
                    { "연락처", "ContactNumber" },
                    { "이메일", "Email" },
                    { "대응 상황", "ProcessingStatus" },
                    { "최종 결과", "FinalResult" },
                    { "비고 (최종결과 사유)", "Notes" }
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
                            startIndex++;
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
                else
                {
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
                }

                var existingRecords = await _context.Requests.ToListAsync();
                var jw = new JaroWinkler();

                for (int row = startRow; row <= rowCount; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null) continue;

                    var newRecordData = string.Join("|", new[]
                    {
                        GetSafeCellValue(currentRow, columnIndexes, "Date"),
                        GetSafeCellValue(currentRow, columnIndexes, "InquiryType"),
                        GetSafeCellValue(currentRow, columnIndexes, "CompanyName"),
                        GetSafeCellValue(currentRow, columnIndexes, "Department"),
                        GetSafeCellValue(currentRow, columnIndexes, "ResponsiblePerson"),
                        GetSafeCellValue(currentRow, columnIndexes, "InquiryField"),
                        GetSafeCellValue(currentRow, columnIndexes, "ClientCompany"),
                        GetSafeCellValue(currentRow, columnIndexes, "ProjectDetails"),
                        GetSafeCellValue(currentRow, columnIndexes, "Client"),
                        GetSafeCellValue(currentRow, columnIndexes, "ContactNumber"),
                        GetSafeCellValue(currentRow, columnIndexes, "Email"),
                        GetSafeCellValue(currentRow, columnIndexes, "ProcessingStatus"),
                        GetSafeCellValue(currentRow, columnIndexes, "FinalResult"),
                        GetSafeCellValue(currentRow, columnIndexes, "Notes"),
                    });

                    Domain.Entities.Requests.Request? bestMatch = null;
                    double maxSimilarity = 0.0;

                    foreach (var existingRecord in existingRecords)
                    {
                        var existingRecordData = string.Join("|", new[]
                        {
                            existingRecord.Date,
                            existingRecord.InquiryType,
                            existingRecord.CompanyName,
                            existingRecord.Department,
                            existingRecord.ResponsiblePerson,
                            existingRecord.InquiryField,
                            existingRecord.ClientCompany,
                            existingRecord.ProjectDetails,
                            existingRecord.Client,
                            existingRecord.ContactNumber,
                            existingRecord.Email,
                            existingRecord.ProcessingStatus,
                            existingRecord.FinalResult,
                            existingRecord.Notes
                        });

                        double similarity = jw.Similarity(existingRecordData, newRecordData);
                        if (similarity > maxSimilarity)
                        {
                            maxSimilarity = similarity;
                            bestMatch = existingRecord;
                        }
                    }

                    if (maxSimilarity >= 0.8 && bestMatch != null)
                    {
                        bestMatch.Date = GetSafeCellValue(currentRow, columnIndexes, "Date");
                        bestMatch.InquiryType = GetSafeCellValue(currentRow, columnIndexes, "InquiryType");
                        bestMatch.CompanyName = GetSafeCellValue(currentRow, columnIndexes, "CompanyName");
                        bestMatch.Department = GetSafeCellValue(currentRow, columnIndexes, "Department");
                        bestMatch.ResponsiblePerson = GetSafeCellValue(currentRow, columnIndexes, "ResponsiblePerson");
                        bestMatch.InquiryField = GetSafeCellValue(currentRow, columnIndexes, "InquiryField");
                        bestMatch.ClientCompany = GetSafeCellValue(currentRow, columnIndexes, "ClientCompany");
                        bestMatch.ProjectDetails = GetSafeCellValue(currentRow, columnIndexes, "ProjectDetails");
                        bestMatch.Client = GetSafeCellValue(currentRow, columnIndexes, "Client");
                        bestMatch.ContactNumber = GetSafeCellValue(currentRow, columnIndexes, "ContactNumber");
                        bestMatch.Email = GetSafeCellValue(currentRow, columnIndexes, "Email");
                        bestMatch.ProcessingStatus = GetSafeCellValue(currentRow, columnIndexes, "ProcessingStatus");
                        bestMatch.FinalResult = GetSafeCellValue(currentRow, columnIndexes, "FinalResult");
                        bestMatch.Notes = GetSafeCellValue(currentRow, columnIndexes, "Notes");
                        bestMatch.RequestStatusId = requestStatusId;

                        genericRepository.UpdateAsync(bestMatch);
                        continue;
                    }

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
                    if (cell != null && key == "Date" && cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime? dateValue = cell.DateCellValue;
                        return dateValue?.Date.ToString("d", CultureInfo.InvariantCulture) ?? "";
                    }
                    return cell != null ? cell.ToString().Trim() : "";
                }
            }
            return ""; 
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel(int? requestCategoryId)
        {
            var query = genericRepository.GetAll().OrderBy(x => x.Id).AsQueryable();

            if (requestCategoryId is not null) query.Where(x => x.RequestStatusId == requestCategoryId);

            var requests = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Requests");

            // Заголовки на корейском
            var headers = new string[]
            {
                "번호", // Номер
                "접수일", // Дата
                "문의유형", // Тип запроса
                "기업명", // Название компании
                "담당부서", // Ответственный отдел
                "담당자명", // Имя ответственного
                "문의분야", // Область запроса
                "고객사 회사", // Компания клиента
                "프로젝트 내용", // Описание проекта
                "고객사", // Клиент
                "연락처", // Контактный номер
                "이메일", // Электронная почта
                "대응 상황", // Статус обработки
                "최종 결과", // Итоговый результат
                "비고 (최종결과 사유)" // Примечания
            };

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
