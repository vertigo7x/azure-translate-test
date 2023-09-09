using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Translation.Domain.Models;

namespace Translation.Service.TableStorage
{
    public class TableStorageService : ITableStorageService
    {
        private const string PARTITION_KEY = "translation";
        private readonly TableClient _tableClient;
        private readonly ILogger<TableStorageService> _logger;

        public TableStorageService(TableServiceClient tableServiceClient, IConfiguration configuration, ILogger<TableStorageService> logger)
        {
            _tableClient = tableServiceClient.GetTableClient(configuration["tableStorage:tableName"]);
            _logger = logger;
        }

        public async Task InsertEntity(TranslationEntityModel translationEntity)
        {
            var entity = new TableEntity(PARTITION_KEY, translationEntity.Id)
            {
                { "Status", translationEntity.Status.ToString() }
            };
            _logger.LogInformation($"TableService: Interting entity: {entity}");
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task UpdateEntity(TranslationEntityModel translationEntity)
        {
            var entity = new TableEntity(PARTITION_KEY, translationEntity.Id)
            {
                { "Status", TranslationJobStatusEnum.Completed.ToString() }
            };
            _logger.LogInformation($"TableService: Updating entity: {entity}");
            await _tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        public async Task<TableEntity> GetEntity(string rowKey)
        {
            _logger.LogInformation($"TableService: Reading entity with Id: {rowKey}");
            var entity = await _tableClient.GetEntityAsync<TableEntity>(PARTITION_KEY, rowKey);
            return entity.Value;
        }
    }
}
