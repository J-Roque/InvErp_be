namespace Security.Domain.Models;

public class Person: Aggregate<long>
{
    public required string FirstName { get; set; } = "";
    public required string LastName { get; set; } = "";
    public required string BusinessName { get; set; } = "";
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = "";
    public string? Email { get; set; }
    public required PersonType PersonType { get; set; }
    public bool IsActive { get; set; } = true;
    
    
    // Relaciones
    private readonly List<Contact> _contacts = [];
    public IReadOnlyList<Contact> Contacts => _contacts.AsReadOnly();
    
    public static Person Create( string firstName, string lastName, string businessName, int documentTypeId, string documentNumber, string? email, bool isActive, PersonType personType)
    {
        var person = new Person
        {
            FirstName = firstName,
            LastName = lastName,
            BusinessName = businessName,
            DocumentTypeId = documentTypeId,
            DocumentNumber = documentNumber,
            Email = email,
            PersonType = personType,
            IsActive = isActive
        };

        person.AddDomainEvent(new PersonCreatedEvent(person));
        return person;
    }
    
    public void Update(string firstName, string lastName, string businessName, int documentTypeId, string documentNumber, string? email, bool isActive, PersonType personType, long? updatedBy = null)
    {
        FirstName = firstName;
        LastName = lastName;
        BusinessName = businessName;
        DocumentTypeId = documentTypeId;
        DocumentNumber = documentNumber;
        Email = email;
        IsActive = isActive;
        PersonType = personType;
        LastModifiedBy = updatedBy;

        AddDomainEvent(new PersonUpdatedEvent(this));
    }
    
    public void AddContact(string name, string? email, List<string>? phoneNumber, string? type, string? position)
    {
        var contact = new Contact
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Type = type,
            Position = position,
            PersonId = Id
        };
        
        contact.AddDomainEvent(new ContactCreatedEvent(contact));
        _contacts.Add(contact);
    }
    
    public void UpdateContact(long contactId, string name, string? email, List<string>? phoneNumber, string? type)
    {
        var contact = _contacts.FirstOrDefault(x => x.Id == contactId);
        if (contact == null) return;
        
        contact.Name = name;
        contact.Email = email;
        contact.PhoneNumber = phoneNumber;
        contact.Type = type;
        
        contact.AddDomainEvent(new ContactUpdatedEvent(contact));
    }
    
    
}