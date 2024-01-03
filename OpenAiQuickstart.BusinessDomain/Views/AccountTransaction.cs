using System.ComponentModel.DataAnnotations;

namespace OpenAiQuickstart.BusinessDomain.Views;

public class AccountTransaction
{
    public Guid Id { get; init; }
    [MaxLength(50)]
    public required string To { get; init; }
    public Guid From { get; init; }
    public required long Amount { get; init; }
    public bool IsPending { get; init; }
    public DateTimeOffset Date { get; init; }
    [MaxLength(255)]
    public required string Reference { get; init; }
    
}