using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace StorageService
{
    public class BlobService
    {
        private CloudStorageAccount _cloudStorageAccount;

        public BlobService()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse("COLOQUE AQUI SUA STRING DE CONEXÃO");
        }

        /// <summary>
        /// Retorna o endereço da imagem
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fileName"></param>
        /// <param name="inputStream"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> UploadImage(string container, string fileName, System.IO.Stream fileStream, string contentType)
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

            //Upload não assíncrono
            await cloudBlockBlob.UploadFromStreamAsync(fileStream);

            //Blob URL
            return cloudBlockBlob.Uri.ToString();
        }
    }
}
