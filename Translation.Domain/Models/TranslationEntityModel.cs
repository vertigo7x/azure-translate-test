namespace Translation.Domain.Models
{
    public class TranslationEntityModel : BaseModel
    {
        public TranslationJobStatusEnum Status { get; set; }

        public TranslationEntityModel()
        {
            Status = TranslationJobStatusEnum.Pending;
        }
    }
}
