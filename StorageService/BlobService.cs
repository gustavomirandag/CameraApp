using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace StorageService
{
    public class BlobService
    {
        private CloudStorageAccount _cloudStorageAccount;

        public BlobService()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=xamarincamapp;AccountKey=jqg/dLYmXBEbAYn60k3fK26m33i3wmnnfXTAcINv97jm74iN0cWzWUTNfkKHFLixidi40mqQi7xCJVRXeTyF0A==;EndpointSuffix=core.windows.net");
        }

        /// <summary>
        /// Retorna o endereço da imagem
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fileName"></param>
        /// <param name="inputStream"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(string container, string fileName, System.IO.Stream fileStream, string contentType)
        {
            //Classe que faz acesso ao Azure Storage Blob
            CloudBlobClient blobClient = _cloudStorageAccount.CreateCloudBlobClient();

            //Classe que faz referência a um Container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);

            //Cria um container novo se não existe
            await blobContainer.CreateIfNotExistsAsync();

            //Altera a configuração do container para permitir o acesso anônimo
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //Referência a uma imagem
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = contentType;

            //Upload assíncrono
            await cloudBlockBlob.UploadFromStreamAsync(fileStream);

            //Blob URL
            return cloudBlockBlob.Uri.ToString();
        }
    }
}
