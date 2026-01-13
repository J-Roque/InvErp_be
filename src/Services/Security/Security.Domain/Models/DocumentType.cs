using Security.Domain.Abstractions;

namespace Security.Domain.Models
{
    public class DocumentType: Aggregate<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
