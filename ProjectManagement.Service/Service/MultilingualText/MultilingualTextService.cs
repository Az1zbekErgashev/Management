using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProjectManagement.Domain.Entities.Teams;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Service.DTOs.MultilingualText;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.MultilingualText;
using System.Text.Json;


namespace ProjectManagement.Service.Service.MultilingualText
{
    public class MultilingualTextService : IMultilingualTextInterface
    {
        private readonly IGenericRepository<Domain.Entities.MultilingualText.MultilingualText> multilingualRepository;
        private readonly IConfiguration configuration;
        public MultilingualTextService(IGenericRepository<Domain.Entities.MultilingualText.MultilingualText> multilingualRepository, IConfiguration configuration)
        {
            this.multilingualRepository = multilingualRepository;
            this.configuration = configuration;
        }

        public async ValueTask<bool> CreateFromJson(IFormFile formFile, SupportLanguage language)
        {
            if (formFile == null || formFile.Length == 0)
                return false;

            try
            {
                using var stream = formFile.OpenReadStream();
                using var reader = new StreamReader(stream);
                string jsonContent = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

                if (data == null)
                    return false;

                foreach (var item in data)
                {
                    var existText = await multilingualRepository.GetAsync(x => (x.Key != null && x.SupportLanguage != null) && x.Key.ToLower() == item.Key.ToLower() && x.SupportLanguage == language);

                    if (existText is null)
                    {
                        if (language == SupportLanguage.Ko)
                        {
                            var newText = new Domain.Entities.MultilingualText.MultilingualText
                            {
                                Key = item.Key,
                                Text = item.Value,
                                SupportLanguage = language,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            await multilingualRepository.CreateAsync(newText);
                        }

                        if (language == SupportLanguage.En)
                        {
                            var newText = new Domain.Entities.MultilingualText.MultilingualText
                            {
                                Key = item.Key,
                                Text = item.Value,
                                SupportLanguage = language,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            await multilingualRepository.CreateAsync(newText);
                        }

                    }
                    else
                    {
                        if (language == SupportLanguage.Ko)
                        {
                            existText.Text = item.Value;
                            existText.UpdatedAt = DateTime.UtcNow;
                        }

                        if (language == SupportLanguage.En)
                        {
                            existText.Text = item.Value;
                            existText.UpdatedAt = DateTime.UtcNow;
                        }

                        multilingualRepository.UpdateAsync(existText);
                    }
                }

                await multilingualRepository.SaveChangesAsync();

                return true;
            }
            catch
            {
                Console.WriteLine($"Ошибка при обработке JSON");
                return false;
            }
        }


        public async ValueTask<Dictionary<string, string>> GetDictonary(SupportLanguage language)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "SELECT \"Key\", \"Text\" FROM \"MultilingualText\" WHERE \"SupportLanguage\" = @Language";

            var result = (await connection.QueryAsync<Domain.Entities.MultilingualText.MultilingualText>(query, new { Language = language })).ToList();

            return language == SupportLanguage.Ko
                ? result.ToDictionary(x => x.Key, x => x.Text)
                : result.ToDictionary(x => x.Key, x => x.Text);
        }


        public async ValueTask<bool> CreateAsync(MultilingualTextForCreateDTO dto)
        {
            var existText = await multilingualRepository.GetAsync(x => x.Key.ToLower() == dto.Key.ToLower());


            var newTextKo = new Domain.Entities.MultilingualText.MultilingualText
            {
                Key = dto.Key,
                Text = dto.TextKo,
                SupportLanguage = SupportLanguage.Ko,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var newTextEn = new Domain.Entities.MultilingualText.MultilingualText
            {
                Key = dto.Key,
                Text = dto.TextEn,
                SupportLanguage = SupportLanguage.En,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await multilingualRepository.CreateAsync(newTextKo);
            await multilingualRepository.CreateAsync(newTextEn);


            await multilingualRepository.SaveChangesAsync();
            return true;
        }



        public async ValueTask<bool> DeleteOrRecoverAsync(string key)
        {
            var existText = await multilingualRepository.GetAll(x => x.Key.ToLower() == key.ToLower()).ToListAsync();

            foreach (var item in existText)
            {
                if(item.IsDeleted == 0)
                {
                    item.IsDeleted = 1;
                    multilingualRepository.UpdateAsync(item);
                }
                else
                {
                    item.IsDeleted = 0;
                    multilingualRepository.UpdateAsync(item);
                }
            }

            await multilingualRepository.SaveChangesAsync();

            return true;
        }
    }
}
