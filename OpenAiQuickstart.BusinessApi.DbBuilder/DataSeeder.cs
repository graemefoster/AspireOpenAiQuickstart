using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenAi.Quickstart.BusinessApi.DbBuilder;

public static class DataSeeder
{
    public static void Seed(this MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
                             CREATE VIEW AccountTransactions
                             WITH SCHEMABINDING
                             AS
                             	SELECT
                             		T1.Id,
                             		T1.[From],
                             		T1.[To],
                             		T1.[Date],
                             		[Amount] = T1.PendingAmountInCents_Cents,
                             		IsPending = CAST((CASE WHEN T2.FinalisedAmountInCents_Cents IS NULL THEN 1  ELSE 0 END) AS BIT),
                             		Reference = T1.Reference
                             	FROM
                             		[dbo].[Transactions] T1
                             		LEFT OUTER JOIN [dbo].[Transactions] T2 ON T1.Id = T2.RelatedTo
                             	WHERE
                             		T1.RelatedTo IS NULL
                             """);
            

        var customers = new List<Guid>();
        foreach (var customer in SeededCustomerData())
        {
            migrationBuilder.InsertData("Customers", ["Id", "FirstName", "LastName"], customer);
            customers.Add((Guid) customer[0]);
        }

        var accounts = new List<(Guid, string)>();
        foreach (var account in SeededAccountData(customers))
        {
            migrationBuilder.InsertData("Accounts", ["Id", "CustomerId", "Name", "AccountNumber"], account);
            accounts.Add(((Guid)account[0], (string)account[3]));
        }

        foreach (var transaction in SeededTransactionData(accounts))
        {
            migrationBuilder.InsertData("Transactions", ["Id", "To", "From", "PendingAmountInCents_Cents", "FinalisedAmountInCents_Cents", "Date", "RelatedTo", "Reference"], transaction);
        }
    }
    
    public static IEnumerable<object[]> SeededCustomerData()
    {
        var firstNames = new[]
        {
            "Alice", "Graeme", "Fred", "Oliver", "Fiona", "Ali", "Faisal", "Clare", "Parveen", "Bindi", "Paul",
            "Peter", "Audrey"
        };
        var lastNames = new[]
        {
            "Fibnar", "Smith", "Jones", "Akbar", "Higginsworth-Bottomly", "Moober", "Fink", "Lolalol", "Keyes", "Gibnar", "Patel",
            "Kliker", "Lo"
        };
        var seed = new Random(23423423);
        for (int i = 0; i < 100; i++)
        {
            var firstName = firstNames[seed.Next(0, firstNames.Length)];
            var lastName = lastNames[seed.Next(0, lastNames.Length)];
            yield return [RT.Comb.Provider.Sql.Create(),firstName, lastName];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} customers");
        }
    }    
    
    private static IEnumerable<object[]> SeededAccountData(IList<Guid> customerIds)
    {
        var accountNames = new[]
        {
            "transactions", "rainy", "week", "year", "live-in-the-now" , "best", "savings", "funky", "bonzer"
        };
        var accountNumber = 1000000;
        var seed = new Random(23423423);
        for (int i = 0; i < 500; i++)
        {
            yield return
            [
                RT.Comb.Provider.Sql.Create(), 
                customerIds[seed.Next(0, customerIds.Count)], 
                $"{accountNames[seed.Next(0, accountNames.Length)]} {accountNames[seed.Next(0, accountNames.Length)]}", 
                (accountNumber++).ToString()
            ];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} accounts");
        }
    }    
    private static IEnumerable<object[]> SeededTransactionData(IList<(Guid, string)> accountNumbers)
    {
        var transactionDescriptors = new[]
        {
            "Ice", "Cream", "Rent", "Shopping", "Mortgage", "Pizza", "Takeaway", "Havianas", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music"
        };
        var seed = new Random(645234);
        var pending = new Dictionary<Guid, (Guid, string, string, int, DateTimeOffset)>();
        for (int i = 0; i < 1000; i++)
        {
            var id = RT.Comb.Provider.Sql.Create(); 
            var cancelPending = seed.Next(0, 2) == 0;
            var pendingAmount = seed.Next(0, 99999999);
            var finaliseAmount = 0;
            var reference = transactionDescriptors[seed.Next(0, transactionDescriptors.Length)];
            var from = accountNumbers[seed.Next(0, accountNumbers.Count)].Item1;
            var to = accountNumbers[seed.Next(0, accountNumbers.Count)].Item2;
            var date = DateTimeOffset.UtcNow.AddDays(-seed.Next(0, 365));
            Guid? refersTo = null;
                
            if (cancelPending && pending.Count > 0)
            {
                refersTo = pending.Keys.ToList()[seed.Next(0, pending.Keys.Count)];
                (from, to, reference, finaliseAmount, date) = pending[refersTo.Value];
                pending.Remove(refersTo.Value);
                pendingAmount = 0;
                date = date.AddMinutes(seed.Next(0, 5000));
            }
            else
            {
                pending.Add(id, (from, to, reference, pendingAmount, date));
            }
            
            yield return
            [
                id, to, from, pendingAmount, finaliseAmount, date, refersTo, reference
            ];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} transactions");
        }
    }
}