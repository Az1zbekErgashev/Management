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


        public static async ValueTask<bool> SaveLogAsync(IGenericRepository<Logs> logRepository, IHttpContextAccessor _httpContextAccessor, Domain.Enum.LogAction logAction)
        {
            var context = _httpContextAccessor.HttpContext;

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                throw new InvalidCredentialException();
            }
            var ipAddress = context?.Connection?.RemoteIpAddress?.ToString();

            var ip = System.Net.IPAddress.Parse(ipAddress); 
            
            var bytes = ip.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes); 

            var longIp = BitConverter.ToInt64(bytes, 0);

            var log = new Logs { Action = logAction, CreatedAt = DateTime.UtcNow, UserId = userId, Ip = longIp };
            await logRepository.CreateAsync(log);
            await logRepository.SaveChangesAsync();
            return true;
        }
    }
}
