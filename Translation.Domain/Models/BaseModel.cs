namespace Translation.Domain.Models
{
    public class BaseModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
