namespace OpenAiQuickstart.Web;

public class Transaction
{
    public Guid Id { get; init; }
    public required string To { get; init; }
    public Guid From { get; init; }
    public long Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public Guid? RelatedTo { get; init; }
    public required string Reference { get; init; }
    public bool IsPending { get; init; }
}