namespace Translation.Domain.Models
{
    public class TranslationEntityModel : BaseModel
    {
        public TranslationJobStatusEnum Status { get; set; }
        private const string PARTITION_KEY = "translation";
        public string PartitionKey { get; } = PARTITION_KEY;

        public TranslationEntityModel()
        {
            Status = TranslationJobStatusEnum.Pending;
        }
    }
}
