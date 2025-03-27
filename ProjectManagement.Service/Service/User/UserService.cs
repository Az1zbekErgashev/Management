using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.User;
using ProjectManagement.Service.DTOs.User;
using ProjectManagement.Service.DTOs.UserForCreationDTO;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.Attachment;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.User;
using ProjectManagement.Service.StringExtensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using TrustyTalents.Service.Services.Emails;

namespace ProjectManagement.Service.Service.User
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Domain.Entities.User.User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IAttachmentService attachmentService;
        private readonly IGenericRepository<Logs> _logRepository;
        private readonly IEmailInboxService emailInboxService;
        public UserService(IGenericRepository<
            Domain.Entities.User.User> userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IAttachmentService attachmentService,
            IGenericRepository<Logs> logRepository,
            IEmailInboxService emailInboxService)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.attachmentService = attachmentService;
            _logRepository = logRepository;
            this.emailInboxService = emailInboxService;
        }

        public async ValueTask<bool> CreateUser(UserForCreationDTO dto)
        {
            var context = _httpContextAccessor.HttpContext;

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                throw new InvalidCredentialException();
            }
            var ipAddress = context?.Connection?.RemoteIpAddress?.ToString();

            var checkUserRole = await _userRepository.GetAsync(x => x.Id == userId && x.IsDeleted == 0);

            Domain.Entities.Attachment.Attachment attachment = null;

            if (dto.Image is not null)
            {
                attachment = await attachmentService.UploadAsync(dto.Image.ToAttachmentOrDefault());
            }
            string passwordForEmail = dto.Password;
            var newUser = new Domain.Entities.User.User
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password.Encrypt(),
                PhoneNumber = dto.PhoneNumber,
                Surname = dto.Surname,
                CreatedAt = DateTime.UtcNow,
                IndividualRole = dto.Role,
                ImageId = attachment?.Id,
                CountryId = dto.CountryId,
                DateOfBirth = dto.DateOfBirth
            };

            var createUser = await _userRepository.CreateAsync(newUser);
            try
            {
                await _userRepository.SaveChangesAsync();
            }
            catch
            {
                throw new ProjectManagementException(400, "this_user_already_exist");
            }
            var verificationMessage = new EmailMessage();
            verificationMessage = EmailMessage.ForAddNewUser(newUser.Email, $"{newUser.Name} {newUser.Surname}", newUser.IndividualRole == Domain.Enum.Role.SuperAdmin ? "System Administrator" : "Employee", passwordForEmail);

            emailInboxService.EnqueueEmail(verificationMessage);

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.CreateAccount);
            return true;
        }

        public async ValueTask<string> DeleteUser(int userId)
        {
            var user = await _userRepository.GetAsync(x => x.Id == userId);

            if (user is null) throw new ProjectManagementException(404, "user_not_found");

            bool isDelete = false;
            if (user.IsDeleted == 0)
            {
                user.IsDeleted = 1;
                isDelete = true;
            }
            else
            {
                user.IsDeleted = 0;
            }

            _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.UpdateUserInformation);

            if (isDelete) return "user_deleted";
            else return "user_recovered";
        }

        public async ValueTask<PagedResult<UserModel>> GetAsync(UserForFilterDTO dto)
        {
            var query = _userRepository.GetAll(null)
                .Include(x => x.Image)
                .Include(x => x.Country)
                .AsQueryable();

            query = query.OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(dto.Text))
            {
                string searchText = $"%{dto.Text}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchText) ||
                    EF.Functions.Like(x.Surname, searchText) ||
                    EF.Functions.Like(x.Email, searchText));
            }

            if (dto.Role != null)
            {
                query = query.Where(x => x.IndividualRole == dto.Role);
            }

            if (dto.UserId is not null)
            {
                query = query.Where(x => x.Id == dto.UserId);
            }

            if (dto.IsDeleted != null)
            {
                query = query.Where(x => x.IsDeleted == dto.IsDeleted);
            }

            int totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                return PagedResult<UserModel>.Create(
                    Enumerable.Empty<UserModel>(),
                    0,
                    dto.PageSize,
                    0,
                    dto.PageIndex,
                    0
                );
            }


            if (dto.PageIndex == 0)
            {
                dto.PageIndex = 1;
            }

            if (dto.PageSize == 0)
            {
                dto.PageSize = totalCount;
            }

            int itemsPerPage = dto.PageSize;
            int totalPages = (totalCount / itemsPerPage) + (totalCount % itemsPerPage == 0 ? 0 : 1);

            query = query.ToPagedList(dto);

            query = query.AsQueryable();

            var list = await query.ToListAsync();

            list.ForEach(x =>
            {
                attachmentService.ResizeImage(x, 600);
            });

            List<UserModel> models = list.Select(
                f => new UserModel().MapFromEntity(f))
                .ToList();


            var pagedResult = PagedResult<UserModel>.Create(models,
                totalCount,
                itemsPerPage,
                models.Count,
                dto.PageIndex,
                totalPages
                );

            return pagedResult;
        }

        public async ValueTask<UserModel> GetByIdAsync(int userId)
        {
            var user = await _userRepository
                .GetAll(x => x.Id == userId && x.IsDeleted == 0)
                .Include(x => x.Image)
                .Include(x => x.Country)
                .FirstOrDefaultAsync();

            if (user is null) throw new ProjectManagementException(404, "user_not_found");

            if (user.Image != null)
            {
                await attachmentService.ResizeImage(user, 600);
            }

            return new UserModel().MapFromEntity(user);
        }

        public async ValueTask<List<UserEmailsModel>> GetUserEmails()
        {
            var users = await _userRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var model = users.Select(x => new UserEmailsModel().MapFromEntity(x)).ToList();

            return model;
        }

        public async ValueTask<LoginModel> Login(UserForLoginDTO dto)
        {
            var user = await _userRepository.GetAll(u =>
            u.Email == dto.Email && u.Password.Equals(dto.Password.Encrypt()) && u.IsDeleted == 0).FirstOrDefaultAsync();

            if (user is null)
                throw new ProjectManagementException(400, "login_or_password_is_incorrect", false);

            var token = await GenerateToken(user.Id);

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.Login, user.Id);

            return new LoginModel().MapFromEntity(user, user.IndividualRole, token);
        }

        public string TokenGenerator(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddHours(int.Parse(configuration["JWT:Expire"])),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    key: authSigningKey,
                    algorithm: SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async ValueTask<bool> UpdateUser(UserForUpdateDTO dto)
        {
            var existUser = await _userRepository
                .GetAll(x => x.Id == dto.UserId && x.IsDeleted == 0)
                .FirstOrDefaultAsync();

            if (existUser is null)
                throw new ProjectManagementException(404, "user_not_found");

            Domain.Entities.Attachment.Attachment attachment = existUser.Image;

            if (dto.UpdateImage)
            {
                attachment = await attachmentService.UploadAsync(dto.Image.ToAttachmentOrDefault());
            }

            existUser.UpdatedAt = DateTime.UtcNow;
            existUser.CountryId = dto.CountryId;
            existUser.PhoneNumber = dto.PhoneNumber;
            existUser.Surname = dto.Surname;
            existUser.Name = dto.Name;
            existUser.ImageId = attachment?.Id;
            existUser.DateOfBirth = dto.DateOfBirth;
            existUser.UpdatedAt = DateTime.UtcNow;
            existUser.IndividualRole = dto.Role;

            if (dto.Password is not null)
            {
                existUser.Password = dto.Password.Encrypt();
            }
            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.UpdateUserInformation);
            return true;
        }

        private async ValueTask<string> GenerateToken(int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            return await ValueTask.FromResult(TokenGenerator(claims));
        }


        public async ValueTask<UserModel> GetProfile()
        {
            var context = _httpContextAccessor.HttpContext;

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                throw new InvalidCredentialException();
            }

            var user = await _userRepository.GetAll(x => x.Id == userId).Include(x => x.Image).Include(x => x.Country).FirstOrDefaultAsync();

            if (user is null) throw new ProjectManagementException(404, "user_not_found");
            return new UserModel().MapFromEntity(user);
        }
    }
}
