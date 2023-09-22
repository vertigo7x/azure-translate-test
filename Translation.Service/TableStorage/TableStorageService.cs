using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Translation.Domain.Models;
using Translation.Service.Extensions;

namespace Translation.Service.TableStorage
{
    public class TableStorageService : ITableStorageService
    {

        private readonly TableClient _tableClient;
        private readonly ILogger<TableStorageService> _logger;

        public TableStorageService(TableServiceClient tableServiceClient, IConfiguration configuration, ILogger<TableStorageService> logger)
        {
            _tableClient = tableServiceClient.GetTableClient(configuration["tableStorage:tableName"]);
            _logger = logger;
        }

        public async Task InsertEntity(TranslationEntityModel translationEntity)
        {
            var entity = MapTranslationEntityToTableEntity(translationEntity);
            _logger.LogInformation($"TableService: Interting entity: {entity}");
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task UpdateEntity(TranslationEntityModel translationEntity)
        {
            var entity = new TableEntity(translationEntity.AsDictionary())
            {
                { "Status", TranslationJobStatusEnum.Completed.ToString() }
            };
            _logger.LogInformation($"TableService: Updating entity: {entity}");
            await _tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        public async Task<TableEntity> GetEntity(TranslationEntityModel translationEntity)
        {
            _logger.LogInformation($"TableService: Reading entity with Id: {translationEntity.Id}");
            var entity = await _tableClient.GetEntityAsync<TableEntity>(translationEntity.PartitionKey, translationEntity.Id);
            return entity.Value;
        }

        private TableEntity MapTranslationEntityToTableEntity(TranslationEntityModel translationEntity)
        {
            var tableEntity =  new TableEntity(translationEntity.AsDictionary())
            {
                { "Status",translationEntity.Status.ToString() }
            };
            tableEntity.RowKey = translationEntity.Id;
            return tableEntity;
        }
    }
}
