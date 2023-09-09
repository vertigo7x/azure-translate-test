using Azure.Data.Tables;
using Translation.Domain.Models;

namespace Translation.Service.TableStorage
{
    public interface ITableStorageService
    {
        public Task InsertEntity(TranslationEntityModel translationEntity);
        public Task UpdateEntity(TranslationEntityModel translationEntity);
        public Task<TableEntity> GetEntity(string rowKey);
    }
}
