using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace OpenAiQuickstart.BusinessDomain.Domain;

public class Account
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }

    [MaxLength(100)]
    public string AccountNumber { get; private set; }

    [MaxLength(100)]
    public string Name { get; private set; }

    public Account(Guid id, Guid customerId, string accountNumber, string name)
    {
        Id = id;
        CustomerId = customerId;
        AccountNumber = accountNumber;
        Name = name;
    }
}

public class Merchant
{
    public required Guid Id { get; init; }

    [MaxLength(6)]
    public required string SortCode { get; init; }
    [MaxLength(15)]
    public required string AccountNumber { get; init; }
    [MaxLength(200)]
    public required string Category { get; init; }
    [MaxLength(200)]
    public required string Name { get; init; }
    
    [MaxLength(10)]
    public string? Postcode { get; init; }
    public Point? Location { get; init; }
}
