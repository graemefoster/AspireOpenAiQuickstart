using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using OpenAiQuickstart.BusinessApi;

namespace OpenAiQuickstart.BusinessDomain.Views;

/// <summary>
/// Information about a transaction
/// </summary>
public class AccountTransaction
{
    /// <summary>
    /// Unique Identifier representing a transaction
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Sort Code of the destination account (if an external transaction, e.g. pay anyone or a merchant)
    /// </summary>
    [MaxLength(6)] public string? ToSortCode { get; init; }
    /// <summary>
    /// Account Number of the destination account (if an external transaction, e.g. pay anyone or a merchant)
    /// </summary>
    [MaxLength(15)] public string? ToAccount { get; init; }

    /// <summary>
    /// Id of the account that made the transaction 
    /// </summary>
    public Guid From { get; init; }
    
    /// <summary>
    /// Amount in cents transferred
    /// </summary>
    public required long Amount { get; init; }

    /// <summary>
    /// True if the transaction has not been finalised yet
    /// </summary>
    public bool IsPending { get; init; }

    /// <summary>
    /// Date, time and timezone that the transaction was made at
    /// </summary>
    public DateTimeOffset Date { get; init; }
    
    /// <summary>
    /// Customer supplied reference for the transaction
    /// </summary>
    [MaxLength(255)] public required string Reference { get; init; }

    /// <summary>
    /// If the transaction is to a Merchant, the name of the Merchant
    /// </summary>
    [MaxLength(500)] public string? MerchantName { get; init; }

    /// <summary>
    /// If the transaction is to a Merchant, the category of the Merchant
    /// </summary>
    [MaxLength(100)] public string? MerchantCategory { get; init; }

    /// <summary>
    /// If the transaction is to a Merchant, the latitude of the merchant
    /// </summary>
    public Point? MerchantLocation { get; init; }
}
