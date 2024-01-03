using System.ComponentModel.DataAnnotations;

namespace OpenAiQuickstart.BusinessDomain.Domain;

public class Transaction
{
    internal Transaction(Guid id, DateTimeOffset date, bool isCredit, string reference)
    {
        Id = id;
        Date = date;
        IsCredit = isCredit;
        Reference = reference;
    }

    [Key]
    public Guid Id { get; init; }

    public Guid? ToId { get; init; }

    [MaxLength(15)]
    public string? ToAccount { get; init; }

    [MaxLength(6)]
    public string? ToSortCode { get; init; }

    [Required]
    public Guid From { get; init; }

    public required Money PendingAmountInCents { get; init; }
    public required Money FinalisedAmountInCents { get; init; }

    public DateTimeOffset Date { get; init; }
    public required bool IsCredit { get; init; }
    public Guid? RelatedTo { get; init; }
    
    [MaxLength(255)]
    public string Reference { get; init; }
}