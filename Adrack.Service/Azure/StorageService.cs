using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Billing;
using Adrack.Core.Infrastructure.Data;

using Adrack.Data;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;

namespace Adrack.Service.Content
{
    public partial class StorageService : IStorageService
    {
        #region Fields

        public StorageService()
        {
         
        }

        #endregion Constructor

        #region Methods
        private static string ConnectionSting
        {
            get
            {
                return "DefaultEndpointsProtocol=https;AccountName=adrackleaddistribution;AccountKey=76+E68KMlqAMo+ZZow3cxTcy45fKYO3DfeKYfOWMRmk28djHp30uW6xemmcdxe5aohZs8tww5RZZn6MoI9q3hA==;EndpointSuffix=core.windows.net";
            }
        }

        public Uri Upload(string blobContainerName, Stream content, string contentType, string fileName)
        {
            try
            {
                content.Position = 0;
                BlobServiceClient _blobServiceClient = new BlobServiceClient(ConnectionSting);
                var containerClient = GetContainerClient(blobContainerName,_blobServiceClient);
                var blobClient = containerClient.GetBlobClient(fileName);
                var asyncTask =  blobClient.Upload(content, new BlobHttpHeaders { ContentType = contentType });
                return blobClient.Uri;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteFile(string blobContainerName, string uniqueFileIdentifier)
        {

            var _containerName = blobContainerName;
           
            
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConnectionSting);
            CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer _cloudBlobContainer = _blobClient.GetContainerReference(_containerName);
            CloudBlockBlob _blockBlob = _cloudBlobContainer.GetBlockBlobReference(uniqueFileIdentifier);
            //delete blob from container    
            _blockBlob.Delete();
        }


        public string DownloadFile(string blobContainerName, string fileName, Stream stream)
        {
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(ConnectionSting);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);

            cloudBlockBlob.FetchAttributes();

            cloudBlockBlob.DownloadToStream(stream);

            return cloudBlockBlob.Properties.ContentType;
        }

        public class UploadManager
        {
            CloudBlobContainer _container;
            public UploadManager(string connectionString)
            {
                _container = new CloudBlobContainer(new Uri(connectionString));
            }
        }

        private BlobContainerClient GetContainerClient(string blobContainerName, BlobServiceClient _blobServiceClient)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            return containerClient;
        }

        // Merged From linked CopyStream below and Jon Skeet's ReadFully example
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        #endregion Methods
    }
}