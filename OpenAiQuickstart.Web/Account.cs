namespace OpenAiQuickstart.Web;

public class Account
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required string Name { get; init; }
}