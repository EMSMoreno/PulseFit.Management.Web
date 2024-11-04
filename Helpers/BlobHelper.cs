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
            // TODO: Configure a conexão com o Azure Blob Storage antes de ir para produção ESTAMOS A ARMAZENAR LOCALMENTE TENDO ASSIM O BLOB
            //nao esquecer de por o blob a armazenar no AZURE e modifcar o appsettings com a connection string
            // Verifique se a chave de conexão está vazia para usar o armazenamento local
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
                // Lógica do Azure Blob Storage
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

            // Adiciona uma extensão padrão, como .jpg
            var filePath = Path.Combine(path, $"{name}.jpg");
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }

            return name;
        }

    }
}
