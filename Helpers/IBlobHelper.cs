namespace PulseFit.Management.Web.Helpers
{
    public interface IBlobHelper
    {
        // File upload via form
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        // File upload via byte array
        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        // File upload via URL
        Task<Guid> UploadBlobAsync(string image, string containerName);
    }
}