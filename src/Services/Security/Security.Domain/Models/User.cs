
using Security.Domain.Enums;
using Security.Domain.Abstractions;

namespace Security.Domain.Models
{
    public class User: Aggregate<long>
    {
        public long PersonId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
        public long? ImageAttachmentId { get; set; }
        public long? ProfileId { get; set; }
        public IdentityStatus IdentityStatusId { get; set; } = IdentityStatus.Active;
       
        // Lista de Roles
        private readonly List<UserRole> _userRoles = [];
        public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();

        // Lista de Proveedores
        private readonly List<UserProvider> _userProviders = [];
        public IReadOnlyList<UserProvider> UserProviders => _userProviders.AsReadOnly();
        
        // Lista de Clientes
        private readonly List<UserClient> _userClients = [];
        public IReadOnlyList<UserClient> UserClients => _userClients.AsReadOnly();

        public static User Create(long personId, string username, string password, string salt,
             IdentityStatus identityStatusId, long? imageAttachmentId, long profileId)
        {
            var user = new User
            {
                // FirstName = firstName,
                // LastName = lastName,
                // DocumentTypeId = documentTypeId,
                // DocumentNumber = documentNumber,
                // Email = email,
                PersonId = personId,
                Username = username,
                Salt = salt,
                Password = password,
                IdentityStatusId = identityStatusId,
                ImageAttachmentId = imageAttachmentId,
                ProfileId = profileId
            };

            user.AddDomainEvent(new UserCreatedEvent(user));

            return user;
        }
        public void Update(string username, IdentityStatus identityStatusId, long profileId)
        {
            Username = username;
            IdentityStatusId = identityStatusId;
            ProfileId = profileId;

            AddDomainEvent(new UserUpdatedEvent(this));
        }

        public void Delete()
        {
            IdentityStatusId = IdentityStatus.Inactive;
            AddDomainEvent(new UserUpdatedEvent(this));
        }

        public void SetRoles(long roleId)
        {
            var userRole = new UserRole { UserId = Id, RoleId = roleId };
            _userRoles.Add(userRole);
        }

        public void RemoveRoles(long roleId)
        {
            var userRole = _userRoles.FirstOrDefault(x => x.RoleId == roleId);
            if (userRole == null) return;
            _userRoles.Remove(userRole);
        }

        public void AddProviders(long providerId, long providerPersonId)
        {
            // Se busca si ya existe
            var userProvider = _userProviders.FirstOrDefault(x => x.ProviderId == providerId);
            if (userProvider != null && !userProvider.IsActive)
            {
                // Se habilita
                userProvider.IsActive = true;
                AddDomainEvent(new UserProviderUpdatedEvent(userProvider));
            }
            else
            {
                var newUserProvider = new UserProvider
                { UserId = Id, ProviderId = providerId, ProviderPersonId = providerPersonId, IsActive = true };
                _userProviders.Add(newUserProvider);
                AddDomainEvent(new UserProviderCreatedEvent(newUserProvider));
            }
        }

        public void HandleProvider(long providerId, bool isActive)
        {
            var userProvider = _userProviders.FirstOrDefault(x => x.ProviderId == providerId);
            if (userProvider == null) return;
            userProvider.IsActive = isActive;
            AddDomainEvent(new UserProviderUpdatedEvent(userProvider));
        }

        public void AddClients(long clientId, long clientPersonId)
        {
            // Se busca si ya existe
            var userClient = _userClients.FirstOrDefault(x => x.ClientId == clientId);
            if (userClient is { IsActive: false })
            {
                // Se habilita
                userClient.IsActive = true;
                AddDomainEvent(new UserClientUpdatedEvent(userClient));
            }
            else
            {
                var newUserClient = new UserClient
                { UserId = Id, ClientId = clientId, ClientPersonId = clientPersonId, IsActive = true };
                _userClients.Add(newUserClient);
                AddDomainEvent(new UserClientCreatedEvent(newUserClient));
            }
        }

        public void HandleClient(long clientId, bool isActive)
        {
            var userClient = _userClients.FirstOrDefault(x => x.ClientId == clientId);
            if (userClient == null) return;
            userClient.IsActive = isActive;
            AddDomainEvent(new UserClientUpdatedEvent(userClient));
        }

        public void HandleImageAttachment(long imageAttachmentId)
        {
            ImageAttachmentId = imageAttachmentId;
            AddDomainEvent(new UserUpdatedEvent(this));
        }

        public void UpdateMyPassword(string password, string salt)
        {
            Password = password;
            Salt = salt;
            LastModifiedBy = Id;
            AddDomainEvent(new UserUpdatedEvent(this));
        }
    }
}
