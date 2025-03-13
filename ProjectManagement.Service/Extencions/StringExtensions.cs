using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.User;
using ProjectManagement.Service.DTOs.Attachment;
using ProjectManagement.Service.Interfaces.IRepositories;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace ProjectManagement.Service.StringExtensions
{
    public static class StringExtensions
    {
        public static string Encrypt(this string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }

        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> source, PaginationParams @params)
        {
            return @params.PageIndex > 0 && @params.PageSize >= 0
                ? source.Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
                : source;
        }

        public static AttachmentForCreationDTO ToAttachmentOrDefault(this IFormFile formFile)
        {

            if (formFile?.Length > 0)
            {
                using var ms = new MemoryStream();
                formFile.CopyTo(ms);
                var fileBytes = ms.ToArray();

                return new AttachmentForCreationDTO()
                {
                    Path = formFile.FileName,
                    Stream = new MemoryStream(fileBytes)
                };
            }

            return null;
        }

        public static string GetIp(HttpContext context)
        {
            return context.Request.Headers.ContainsKey("Cf-Connecting-Ip")
                ? context.Request.Headers["Cf-Connecting-Ip"].ToString()
                : context.Request.Headers.ContainsKey("X-Forwarded-For")
                    ? context.Request.Headers["X-Forwarded-For"].ToString()
                    : context.Connection.RemoteIpAddress.ToString();
        }

        public static async ValueTask<bool> SaveLogAsync(IGenericRepository<Logs> logRepository, IHttpContextAccessor _httpContextAccessor, Domain.Enum.LogAction logAction, int? userIdFromDatabase = null)
        {
            var context = _httpContextAccessor.HttpContext;

            var id = userIdFromDatabase;

            if (int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                id = userId;
            }

            var log = new Logs { Action = logAction, CreatedAt = DateTime.UtcNow, UserId = id ?? 0, Ip = GetIp(context) };
            await logRepository.CreateAsync(log);
            await logRepository.SaveChangesAsync();
            return true;
        }
    }
}
