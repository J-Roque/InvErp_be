namespace Security.Domain.Models;

public class Contact: Aggregate<long>
{
    public long PersonId { get; set; }
    public required string Name { get; set; } = "";
    public  string? Email { get; set; } = "";
    public  List<string>? PhoneNumber { get; set; } = new List<string>();
    public string? Mobile { get; set; } = "";
    public  string? Type { get; set; } = "";
    public string? Position { get; set; } = "";
    public bool IsActive { get; set; } = true;
    
    
    public static Contact Create(string name, string? email, List<string>? phoneNumber, string? type, string? position, long personId)
    {
        var contact = new Contact
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Type = type,
            Position = position,
            PersonId = personId
        };
        
        contact.AddDomainEvent(new ContactCreatedEvent(contact));
        return contact;
    }
    
    public void Update(string name, string? email, List<string>? phoneNumber, string? type, string? position)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Type = type;
        Position = position;
        
        AddDomainEvent(new ContactUpdatedEvent(this));
    }
    
    public void Delete()
    {
        IsActive = false;
        AddDomainEvent(new ContactUpdatedEvent(this));
    }
    
}