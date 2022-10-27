using System.Text;

namespace SaveYourMoneyMVC.Common.Helpers
{
    public interface IFileStorage
    {
        Task<string> EditFile(byte[] content, string extension, string containerName, string path);
        Task DeleteFile(string path, string containerName);
        Task<string> SaveFile(byte[] content, string extension, string containerName);
    }
    public class FileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string path, string containerName)
        {
            var filename = Path.GetFileName(path);
            var file = Path.Combine(_env.WebRootPath, containerName, filename);

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string containerName, string originalPath)
        {
            if (!string.IsNullOrWhiteSpace(originalPath))
            {
                await DeleteFile(originalPath, containerName);
            }

            return await SaveFile(content, extension, containerName);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName)
        {
            var filename = $"{Guid.NewGuid()}.{extension}";
            var folder = Path.Combine(_env.WebRootPath, containerName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string savePath = Path.Combine(folder, filename);
            await File.WriteAllBytesAsync(savePath, content);

            var urlActual = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var dbPath = Path.Combine(urlActual, containerName, filename);

            return dbPath;
        }
    }
}
