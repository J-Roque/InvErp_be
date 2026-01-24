namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : Exception
    {

        public string? CustomMessage { get; }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string property, object value, string? customMessage = null)
            : base(customMessage ?? $"Entidad '{property}' ({value}) no fue encontrada")
        {
            CustomMessage = customMessage;
        }


    }
}
