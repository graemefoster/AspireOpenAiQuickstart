namespace OpenAiQuickstart.BusinessDomain.Domain;

public record Money
{
    public required long Cents { get; init; }
};
