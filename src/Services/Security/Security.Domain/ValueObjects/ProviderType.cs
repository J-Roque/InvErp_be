namespace Security.Domain.ValueObjects;

public class ProviderType
{
    public string Value { get; }
    private ProviderType(string value) => Value = value;

    public static ProviderType Of(string value)
    {
        if (value != "NA" && value != "EX")
        {
            throw new DomainException("El tipo de proveedor no es válido");
        }

        return new ProviderType(value);
    }
    
}