namespace OpenAiQuickstart.BusinessDomain.Domain;

public class Customer
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}