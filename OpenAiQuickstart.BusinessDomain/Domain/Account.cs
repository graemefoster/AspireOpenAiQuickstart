using System.ComponentModel.DataAnnotations;

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
