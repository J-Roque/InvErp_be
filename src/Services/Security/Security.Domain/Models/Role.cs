using Security.Domain.Enums;

namespace Security.Domain.Models
{
    public class Role: Aggregate<long>
    {
        public required string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsProtected { get; set; } = false;
        public long? ImageAttachmentId { get; set; }

        public IdentityStatus IdentityStatusId { get; set; } = IdentityStatus.Active;

        public static Role Create(long id, string name, string? description, bool isActive, string? code, long? imageAttachmentId, bool isProtected, IdentityStatus identityStatusId)
        {
            var role = new Role
            {
                Name = name,
                Description = description,
                IsActive = isActive,
                ImageAttachmentId = imageAttachmentId,
                Code = code,
                IsProtected = isProtected,
                IdentityStatusId = identityStatusId
            };

            role.AddDomainEvent(new RoleCreatedEvent(role));
            return role;
        }

        public void Update(string name, string? description, IdentityStatus identityStatusId)
        {
            Name = name;
            Description = description;
            IdentityStatusId = identityStatusId;
            IsActive = identityStatusId == IdentityStatus.Inactive ? false : true;

            AddDomainEvent(new RoleUpdatedEvent(this));
        }

        public void HandleImageAttachment(long attachmentId)
        {
            ImageAttachmentId = attachmentId;

            AddDomainEvent(new RoleUpdatedEvent(this));
        }
    }
}
