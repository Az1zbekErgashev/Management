using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Domain.Entities.Teams;
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

namespace ProjectManagement.Service.Service.User
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Domain.Entities.User.User> _userRepository;
        private readonly IGenericRepository<Domain.Entities.Teams.TeamMember> _teamMemberRepository;
        private readonly IGenericRepository<Domain.Entities.Companies.Companies> _companyRepository;
        private readonly IGenericRepository<Domain.Entities.Teams.Team> _teamRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IAttachmentService attachmentService;
        public UserService(IGenericRepository<
            Domain.Entities.User.User> userRepository,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<Domain.Entities.Teams.TeamMember> teamMemberRepository,
            IGenericRepository<Domain.Entities.Companies.Companies> companyRepository,
            IConfiguration configuration,
            IAttachmentService attachmentService,
            IGenericRepository<Domain.Entities.Teams.Team> teamRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _teamMemberRepository = teamMemberRepository;
            _companyRepository = companyRepository;
            this.configuration = configuration;
            this.attachmentService = attachmentService;
            _teamRepository = teamRepository;
        }

        public async ValueTask<bool> CreateUser(UserForCreationDTO dto)
        {
            var context = _httpContextAccessor.HttpContext;

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                throw new InvalidCredentialException();
            }

            var checkUserRole = await _userRepository.GetAsync(x => x.Id == userId && x.IsDeleted == 0);

            if (checkUserRole.IndividualRole != Domain.Enum.Role.TeamLead && checkUserRole.IndividualRole != Domain.Enum.Role.SuperAdmin) throw new ProjectManagementException(403, "role_incorrect");

            Domain.Entities.Attachment.Attachment attachment = null;

            if(dto.Image is not null)
            {
                attachment = await attachmentService.UploadAsync(dto.Image.ToAttachmentOrDefault());
            }

            var newUser = new Domain.Entities.User.User
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password.Encrypt(),
                PhoneNumber = dto.PhoneNumber,
                Surname = dto.Surname,
                CompanyId = dto.CompanyId,
                CreatedAt = DateTime.UtcNow,
                IndividualRole = dto.Role,
                ImageId = attachment?.Id,
                CountryId = dto.CountryId
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


            if (dto.TeamLeaderId is not null)
            {
                var teamLeader = await _userRepository
                    .GetAll(x => x.Id == dto.TeamLeaderId && x.IsDeleted == 0 && x.TeamMember.IsDeleted == 0)
                    .Include(x => x.TeamMember)
                    .ThenInclude(x => x.Team)
                    .FirstOrDefaultAsync();

                if (teamLeader == null || teamLeader.TeamMember == null)
                {
                    throw new ProjectManagementException(400, "user_not_found");
                }


                var existTeam = teamLeader?.TeamMember?.Team;

                if (teamLeader?.TeamMember == null || existTeam == null)
                {

                    var team = new Team
                    {
                        Location = string.Empty,
                        AssignedCompanyId = dto.CompanyId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    await _teamRepository.CreateAsync(team);
                    await _teamRepository.SaveChangesAsync();

                    List<TeamMember> teamLeaders = new List<TeamMember>
                    {
                        new TeamMember { CreatedAt = DateTime.UtcNow, UserId = teamLeader.Id, TeamId = team.Id },
                        new TeamMember { CreatedAt = DateTime.UtcNow, UserId = createUser.Id, TeamId = team.Id}
                    };

                    team.TeamMembers = teamLeaders;

                    foreach (var item in teamLeaders)
                    {
                        await _teamMemberRepository.CreateAsync(item);
                    }

                    _teamRepository.UpdateAsync(team);
                    await _teamMemberRepository.SaveChangesAsync();
                    await _teamRepository.SaveChangesAsync();


                    return true;
                }

                var newTeamMember = new Domain.Entities.Teams.TeamMember
                {
                    UserId = createUser.Id,
                    TeamId = existTeam.Id,
                    CreatedAt = DateTime.UtcNow,
                };

                await _teamMemberRepository.CreateAsync(newTeamMember);
                await _teamMemberRepository.SaveChangesAsync();
            }
            return true;
        }

        public async ValueTask<string> DeleteUser(int userId)
        {
            var user = await _userRepository.GetAsync(x => x.Id == userId);

            if (user is null) throw new ProjectManagementException(404, "user_not_found");

            bool isDelete = false;
            if(user.IsDeleted == 0)
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

            if(isDelete) return "user_deleted";
            else return "user_recovered";
        }

        public async ValueTask<PagedResult<UserModel>> GetAsync(UserForFilterDTO dto)
        {
            var query = _userRepository.GetAll(null)
                .Include(x => x.Companies)
                .Include(x => x.TeamMember)
                .ThenInclude(x => x.Team)
                .ThenInclude(x => x.TeamMembers)
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
                query = query.Where(x =>  x.IndividualRole == dto.Role);
            }

            if (dto.CompanyId != null)
            {
                query = query.Where(x => x.Companies != null && x.CompanyId == dto.CompanyId);
            }

            if(dto.IsDeleted != null)
            {
                query = query.Where(x => x.IsDeleted == dto.IsDeleted);
            }

            if (dto.TeamLeaderId != null)
            {
                query = query.Where(x =>
                    x.TeamMember != null &&
                    x.TeamMember.Team != null &&
                    x.TeamMember.Team.TeamMembers != null &&
                    x.TeamMember.Team.TeamMembers.Any(member => member.UserId == dto.TeamLeaderId));
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
                .Include(x => x.Companies)
                .Include(x => x.TeamMember)
                .ThenInclude(x => x.Team)
                .ThenInclude(x => x.TeamMembers)
                .Include(x => x.Image)
                .Include(x => x.Country)
                .FirstOrDefaultAsync();

            if (user is null) throw new ProjectManagementException(404, "user_not_found");

            if(user.Image != null)
            {
               await attachmentService.ResizeImage(user, 600);
            }

            return new UserModel().MapFromEntity(user);
        }

        public async ValueTask<List<CompanyNameModel>> GetCompanyName(int? teamLeadrsId)
        {
            var allCompany = await _companyRepository
                .GetAll(x => x.IsDeleted == 0)
                .Include(x => x.Teams)
                .ThenInclude(x => x.TeamMembers)
                .ToListAsync();

            if (teamLeadrsId is not null)
            {
                allCompany = allCompany
                .Where(company => company?.Teams != null &&
                    company.Teams.Any(team => team?.TeamMembers != null &&
                    team.TeamMembers.Any(member => member.UserId == teamLeadrsId)))
                .ToList();
            }

            var model = allCompany.Select(x => new CompanyNameModel().MapFromEntity(x.CompanyName, x.Id)).ToList();

            return model;
        }

        public async ValueTask<List<TeamLeadersNameModel>> GetTeamLeadrsName(int? companyId)
        {
            var teamLiders = await _userRepository.GetAll(x => x.IsDeleted == 0 && x.IndividualRole == Domain.Enum.Role.TeamLead).ToListAsync();

            if(companyId is not null)
            {
                teamLiders = teamLiders.Where(x => x.CompanyId == companyId).ToList();
            }

            var model = teamLiders.Select(x => new TeamLeadersNameModel().MapFromEntity($"{x.Name} {x.Surname}" , x.Id)).ToList();

            return model;
        }

        public async ValueTask<LoginModel> Login(UserForLoginDTO dto)
        {
            var user = await _userRepository.GetAll(u =>
            u.Email == dto.Email && u.Password.Equals(dto.Password.Encrypt()) && u.IsDeleted == 0).Include(x => x.TeamMember).ThenInclude(x => x.Team).FirstOrDefaultAsync();

            if (user is null)
                throw new ProjectManagementException(400, "login_or_password_is_incorrect", false);

            var token = await GenerateToken(user.Id);

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
                .Include(x => x.TeamMember)
                .ThenInclude(x => x.Team)
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

            if(dto.Password is not null)
            {
                existUser.Password = dto.Password.Encrypt();
            }

            if (dto.TeamLeaderId is not null)
            {
                var team = await _teamRepository.GetAsync(x =>
                    x.IsDeleted == 0 &&
                    x.TeamMembers != null &&
                    x.TeamMembers.Any(tm => tm.UserId == dto.TeamLeaderId)
                );

                int? newTeamId = team?.Id;

                if (existUser.TeamMember is not null)
                {
                    bool isAlreadyInCorrectTeam = existUser.TeamMember.TeamId == newTeamId;

                    if (!isAlreadyInCorrectTeam) 
                    {
                        existUser.TeamMember.IsDeleted = 1;
                        _teamMemberRepository.UpdateAsync(existUser.TeamMember);

                        if (newTeamId is not null)
                        {
                            var newTeamMember = new TeamMember
                            {
                                UserId = existUser.Id,
                                TeamId = newTeamId.Value,
                                CreatedAt = DateTime.UtcNow,
                            };

                            await _teamMemberRepository.CreateAsync(newTeamMember);
                        }

                        await _teamMemberRepository.SaveChangesAsync();
                    }
                }
                else  
                {
                    if (newTeamId is not null)
                    {
                        var newTeamMember = new TeamMember
                        {
                            UserId = existUser.Id,
                            TeamId = newTeamId.Value,
                            CreatedAt = DateTime.UtcNow,
                        };

                        await _teamMemberRepository.CreateAsync(newTeamMember);
                    }

                    await _teamMemberRepository.SaveChangesAsync();
                }
            }

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
    }
}
