using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using ProjectManagement.Service.DTOs.Attachment;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.Attachment;
using ProjectManagement.Service.Interfaces.IRepositories;

namespace ProjectManagement.Service.Service.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IGenericRepository<Domain.Entities.Attachment.Attachment> attachmentRepository;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(IGenericRepository<Domain.Entities.Attachment.Attachment> attachmentRepository, IWebHostEnvironment env)
        {
            this.attachmentRepository = attachmentRepository;
            _env = env;
        }

        public class EnvironmentHelper
        {
            public static string WebRootPath { get; set; }
            public static string AttachmentPath => Path.Combine(WebRootPath, "images");
            public static string FilePath => "images";
        }

        public async ValueTask<Domain.Entities.Attachment.Attachment> CreateAsync(string filePath)
        {

            var file = new Domain.Entities.Attachment.Attachment()
            {
                Path = filePath
            };

            file = await attachmentRepository.CreateAsync(file);
            await attachmentRepository.SaveChangesAsync();

            return file;
        }

        void ResizeAndSave(MagickImage image, int dimension, MagickFormat format, string fileName, string folderPath, bool exist)
        {
            using (var memoryStream = new MemoryStream())
            {
                MagickGeometry size = new MagickGeometry((uint)dimension, (uint)dimension)
                {
                    IgnoreAspectRatio = false,
                    FillArea = true
                };

                image.Resize(size);
                image.Extent((uint)dimension, (uint)dimension, Gravity.Center);

                image.Write(memoryStream, format);
                memoryStream.Position = 0;

                string resizedPath;

                if (exist)
                {
                    resizedPath = Path.Combine(folderPath, fileName);
                    using var existResized = new FileStream(resizedPath, FileMode.Create);
                    memoryStream.CopyTo(existResized);
                }
                else
                {
                    var fileDbNameResized = Path.Combine("images", fileName);
                    resizedPath = Path.Combine(folderPath, fileDbNameResized);
                    using var newResized = new FileStream(resizedPath, FileMode.Create);
                    memoryStream.CopyTo(newResized);
                }
            }
        }

        private MagickFormat DetermineImageFormatFromExtension(string fileExtension)
        {
            return fileExtension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => MagickFormat.Jpeg,
                ".png" => MagickFormat.Png
            };
        }

        public async ValueTask<Domain.Entities.Attachment.Attachment> UpdateAsync(int id, Stream stream)
        {
            var existAttachment = await attachmentRepository.GetAsync(a => a.Id == id);

            if (existAttachment is null)
                throw new ProjectManagementException(404, "attachment_not_found.");

            string fileName = existAttachment.Path;
            string filePath = Path.Combine(EnvironmentHelper.WebRootPath, fileName);

            // copy image to the destination as stream
            FileStream fileStream = File.OpenWrite(filePath);
            await stream.CopyToAsync(fileStream);

            // clear
            await fileStream.FlushAsync();
            fileStream.Close();

            await attachmentRepository.SaveChangesAsync();

            return existAttachment;
        }

        public async ValueTask<Domain.Entities.Attachment.Attachment> UploadAsync(AttachmentForCreationDTO dto)
        {
            if (dto is null)
            {
                throw new ProjectManagementException(400, "you_must_upload_the_file");
            }
            string fileName = Guid.NewGuid().ToString("N") + dto.Path;
            string filePath = Path.Combine(EnvironmentHelper.AttachmentPath, fileName);

            if (!Directory.Exists(EnvironmentHelper.AttachmentPath))
                Directory.CreateDirectory(EnvironmentHelper.AttachmentPath);

            // copy image to the destination as stream
            await using FileStream fileStream = File.OpenWrite(filePath);
            await dto.Stream.CopyToAsync(fileStream);

            // clear
            await fileStream.FlushAsync();
            fileStream.Close();

            return await CreateAsync(Path.Combine(EnvironmentHelper.FilePath, fileName));
        }

        public async ValueTask<bool> ResizeImage(Domain.Entities.User.User user, int dimension)
        {
            if (user.Image != null)
            {
                if (!string.IsNullOrEmpty(user.Image.Path))
                {
                    var path = user.Image.Path;

                    var imagePath = Path.Combine(_env.WebRootPath, path);

                    if (File.Exists(imagePath))
                    {
                        path = path.Insert(path.IndexOf(@"\") + 1, @$"{dimension}x{dimension}-");

                        if (!File.Exists(path))
                        {
                            var format = DetermineImageFormatFromExtension(Path.GetExtension(imagePath));

                            string fileName = $"{dimension}x{dimension}-{Path.GetFileName(imagePath)}";

                            using (var image = new MagickImage(imagePath))
                            {
                                ResizeAndSave(image, dimension, format, fileName, Path.Combine(_env.WebRootPath, "images"), true);
                            }
                        }
                    }
                }
            }

            return true;
        }


        public async ValueTask<bool> ResizeImage(Domain.Entities.Attachment.Attachment? attachment, int dimension)
        {
            if (attachment != null)
            {
                if (!string.IsNullOrEmpty(attachment.Path))
                {
                    var path = attachment.Path;

                    var imagePath = Path.Combine(_env.WebRootPath, path);

                    if (File.Exists(imagePath))
                    {
                        path = path.Insert(path.IndexOf(@"\") + 1, @$"{dimension}x{dimension}-");

                        if (!File.Exists(path))
                        {
                            var format = DetermineImageFormatFromExtension(Path.GetExtension(imagePath));

                            string fileName = $"{dimension}x{dimension}-{Path.GetFileName(imagePath)}";

                            using (var image = new MagickImage(imagePath))
                            {
                                ResizeAndSave(image, dimension, format, fileName, Path.Combine(_env.WebRootPath, "images"), true);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
