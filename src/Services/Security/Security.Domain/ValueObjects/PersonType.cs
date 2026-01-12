namespace Security.Domain.ValueObjects;

public class PersonType
{
    public string Value { get; }
    private PersonType(string value) => Value = value;

    public static PersonType Of(string value)
    {
        if (value != "J" && value != "N")
        {
            throw new DomainException("El tipo de persona no es válido");
        }

        return new PersonType(value);
    }
}