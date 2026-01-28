using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace YumBlazor.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
        Task<bool> DeleteImageAsync(string imageUrl);
    }

    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ContainerName = "product-images";

        public BlobStorageService([FromKeyedServices("StorageConnection")] BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            string blobName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType });

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) 
            { 
                return false; 
            }

            try
            {
                Uri uri = new Uri(imageUrl);
                string blobName = uri.Segments[^1];

                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                
                return await blobClient.DeleteIfExistsAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}