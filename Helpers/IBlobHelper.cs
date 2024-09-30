namespace PulseFit.Management.Web.Helpers
{
    public interface IBlobHelper
    {
        // Upload de ficheiro via formulário
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        // Upload de ficheiro via array de bytes
        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        // Upload de ficheiro via URL
        Task<Guid> UploadBlobAsync(string image, string containerName);
    }
}