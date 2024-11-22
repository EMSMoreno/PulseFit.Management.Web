using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly string _storagePath;

        public BlobHelper(IConfiguration configuration)
        {
            var keys = configuration["Blob:ConnectionStrings"];
            // TODO: Configure the connection to Azure Blob Storage before going to production WE ARE STORING THE BLOB LOCALLY
            //don't forget to store the blob in AZURE and modify the appsettings with the connection string
            // Make sure the connection key is empty to use local storage
            if (string.IsNullOrEmpty(keys))
            {
                _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(_storagePath))
                {
                    Directory.CreateDirectory(_storagePath);
                }
            }
            else
            {
                // Azure Blob Storage Logic
                // CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
                // _blobClient = storageAccount.CreateCloudBlobClient();
            }
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            using var stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            using var stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            using var stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containerName);
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            var name = Guid.NewGuid();
            var path = Path.Combine(_storagePath, containerName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Adds a default extension, such as .jpg
            var filePath = Path.Combine(path, $"{name}.jpg");
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }

            return name;
        }

    }
}
