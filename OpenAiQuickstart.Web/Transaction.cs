using System.Drawing;

namespace OpenAiQuickstart.Web;

public class Transaction
{
    public Guid Id { get; init; }
    public required Guid? ToId { get; init; }
    public required string? ToSortCode { get; init; }
    public required string? ToAccount { get; init; }
    public Guid From { get; init; }
    public long Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public Guid? RelatedTo { get; init; }
    public required string Reference { get; init; }
    public bool IsPending { get; init; }
    public string? MerchantName { get; set; }
    public string? MerchantCategory { get; set; }
    public double[]? MerchantLocation { get; set; }
}
