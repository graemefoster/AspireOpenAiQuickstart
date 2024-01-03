using System.ComponentModel.DataAnnotations;

namespace OpenAiQuickstart.BusinessDomain.Domain;

public class Transaction
{
    internal Transaction(Guid id, DateTimeOffset date, string reference)
    {
        Id = id;
        Date = date;
        Reference = reference;
    }

    [Key]
    public Guid Id { get; init; }
    
    [Required]
    [MaxLength(30)]
    public string To { get; init; }

    [Required]
    public Guid From { get; init; }

    public required Money PendingAmountInCents { get; init; }
    public required Money FinalisedAmountInCents { get; init; }

    public DateTimeOffset Date { get; init; }
    public Guid? RelatedTo { get; init; }
    
    [MaxLength(255)]
    public string Reference { get; init; }
}