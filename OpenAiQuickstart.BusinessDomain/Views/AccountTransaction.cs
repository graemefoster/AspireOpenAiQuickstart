using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace OpenAiQuickstart.BusinessDomain.Views;

public class AccountTransaction
{
    public Guid Id { get; init; }
    [MaxLength(6)] public string? ToSortCode { get; init; }
    [MaxLength(15)] public string? ToAccount { get; init; }
    public Guid From { get; init; }
    public required long Amount { get; init; }
    public bool IsPending { get; init; }
    public DateTimeOffset Date { get; init; }
    [MaxLength(255)] public required string Reference { get; init; }
    [MaxLength(500)] public string? MerchantName { get; init; }
    [MaxLength(100)] public string? MerchantCategory { get; init; }
    public Point? MerchantLocation { get; init; }
}