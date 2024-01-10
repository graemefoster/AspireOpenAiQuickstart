using System.ComponentModel;

namespace OpenAiQuickstart.BusinessApi.Queries;

public class TransactionSearchRequest
{
    /// <summary>
    /// Earliest date to search transactions from
    /// </summary>
    public DateTimeOffset? From { get; set; }
    
    /// <summary>
    /// Latest date to search transactions to. Leave empty for current date
    /// </summary>
    public DateTimeOffset? To { get; set; }

    /// <summary>
    /// Free text search over transaction reference. 
    /// </summary>
    public string? Reference { get; set; } = default!;
    
    /// <summary>
    /// Transaction categories to search for 
    /// </summary>
    public Category[]? Categories { get; set; } = default!;

    /// <summary>
    /// Minimum value of transactions to return
    /// </summary>
    public int? LowAmountInCents { get; set; }

    /// <summary>
    /// Maximum value of transactions to return
    /// </summary>
    public int? HighAmountInCents { get; set; }

    /// <summary>
    /// Geometry coordinates of a bounding box to narrow the search within
    /// </summary>
    public GeoBoundingBox? BoundingBox { get; set; }

}