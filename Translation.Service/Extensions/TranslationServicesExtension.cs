using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Translation.Service.BlobStorage;
using Translation.Service.QueueStorage;
using Translation.Service.TableStorage;
using Translation.Service.TranslationService;

namespace Translation.Service.Extensions
{
    public static class TranslationServicesExtension
    {
        public static void AddTranslationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(configuration["ConnectionStrings:Azurite"]);
                clientBuilder.AddQueueServiceClient(configuration["ConnectionStrings:Azurite"]);
                clientBuilder.AddTableServiceClient(configuration["ConnectionStrings:Azurite"]);
            });

            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            services.AddSingleton<ITableStorageService, TableStorageService>();
            services.AddSingleton<IQueueStorageService, QueueStorageService>();
            services.AddSingleton<IAzureTranslationService, AzureTranslationService>();

        }
        public static void UseAzureStorageServices(this IApplicationBuilder builder, IConfiguration configuration)
        {
            // Blob Storage Setup
            var blobServiceClient = builder.ApplicationServices.GetRequiredService<BlobServiceClient>();
            try
            {
                // Create the container
                BlobContainerClient container = blobServiceClient.CreateBlobContainer(configuration["blobStorage:storageName"]);

                if (container.Exists())
                {
                    Console.WriteLine("Created container {0}", container.Name);
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
                Console.WriteLine(e.Message);
            }
            // Table Storage Setup
            var tableServiceClient = builder.ApplicationServices.GetRequiredService<TableServiceClient>();
            tableServiceClient.CreateTableIfNotExists(configuration["tableStorage:tableName"]);

            // Queue Storage Setup
            var queueServiceClient = builder.ApplicationServices.GetRequiredService<QueueServiceClient>();
            queueServiceClient.CreateQueue(configuration["queueStorage:queueName"]);
        }
    }
}
