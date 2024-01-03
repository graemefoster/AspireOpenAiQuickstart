using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace OpenAi.Quickstart.BusinessApi.DbBuilder;

public static class DataSeeder
{
    public static void Seed(this MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
                                CREATE VIEW AccountTransactions AS
                             	SELECT
                             		T1.Id,
                              		T1.[From],
                                    T1.[ToId],
                                    T1.[ToSortCode],
                                    T1.[ToAccount],
                             		T1.[Date],
                             		T1.[IsCredit],
                             		[Amount] = COALESCE(T2.FinalisedAmountInCents_Cents, T1.FinalisedAmountInCents_Cents),
                             		IsPending = CAST((CASE WHEN T2.FinalisedAmountInCents_Cents IS NULL THEN 1  ELSE 0 END) AS BIT),
                             		Reference = T1.Reference,
                             		M.[Name],
                             		M.[Category],
                             		M.[Location]
                             	FROM
                             		[dbo].[Transactions] T1
                             		LEFT OUTER JOIN [dbo].[Transactions] T2 ON T1.Id = T2.RelatedTo
                             		LEFT OUTER JOIN [dbo].Merchants M ON T1.ToSortCode = M.SortCode AND T1.ToAccount = M.AccountNumber
                             	WHERE
                             		T1.RelatedTo IS NULL
                             """);

        var customers = new List<Guid>();
        foreach (var customer in SeededCustomerData())
        {
            migrationBuilder.InsertData("Customers", ["Id", "FirstName", "LastName"], customer);
            customers.Add((Guid)customer[0]);
        }

        var accounts = new List<(Guid, string)>();
        foreach (var account in SeededAccountData(customers))
        {
            migrationBuilder.InsertData("Accounts", ["Id", "CustomerId", "Name", "AccountNumber"], account);
            accounts.Add(((Guid)account[0], (string)account[3]));
        }

        var merchants = new List<(string, string)>();
        foreach (var merchant in SeededMerchantData())
        {
            migrationBuilder.InsertData("Merchants",
                ["Id", "Name", "Category", "SortCode", "AccountNumber", "Postcode", "Location"], merchant);
            merchants.Add(((string)merchant[3], (string)merchant[4]));
        }

        var transactions = SeededTransactionData(accounts, merchants).ToArray();
        object[,] multiArray = new object[transactions.Length, 11];
        for (var i = 0; i < transactions.Length; i++)
        {
            multiArray[i, 0] = transactions[i][0];
            multiArray[i, 1] = transactions[i][1];
            multiArray[i, 2] = transactions[i][2];
            multiArray[i, 3] = transactions[i][3];
            multiArray[i, 4] = transactions[i][4];
            multiArray[i, 5] = transactions[i][5];
            multiArray[i, 6] = transactions[i][6];
            multiArray[i, 7] = transactions[i][7];
            multiArray[i, 8] = transactions[i][8];
            multiArray[i, 9] = transactions[i][9];
            multiArray[i, 10] = transactions[i][10];
        }

        migrationBuilder.InsertData(
            "Transactions",
            [
                "Id", "ToId", "ToSortCode", "ToAccount", "From", "PendingAmountInCents_Cents",
                "FinalisedAmountInCents_Cents", "Date", "RelatedTo", "Reference", "IsCredit"
            ],
            multiArray);
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
            "Fibnar", "Smith", "Jones", "Akbar", "Higginsworth-Bottomly", "Moober", "Fink", "Lolalol", "Keyes",
            "Gibnar", "Patel",
            "Kliker", "Lo"
        };
        var seed = new Random(23423423);
        for (int i = 0; i < 500; i++)
        {
            var firstName = firstNames[seed.Next(0, firstNames.Length)];
            var lastName = lastNames[seed.Next(0, lastNames.Length)];
            yield return [RT.Comb.Provider.Sql.Create(), firstName, lastName];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} customers");
        }
    }

    public static IEnumerable<object[]> SeededMerchantData()
    {
        var locations = new[]
        {
            ("6000", new Point(115.910, -31.920) { SRID = 4326 }),
            ("6000", new Point(115.910, -31.920) { SRID = 4326 }),
            ("6001", new Point(115.760, -31.990) { SRID = 4326 }),
            ("6003", new Point(115.870, -31.940) { SRID = 4326 }),
            ("6003", new Point(115.870, -31.940) { SRID = 4326 }),
            ("6004", new Point(115.880, -31.940) { SRID = 4326 }),
            ("6005", new Point(115.840, -31.970) { SRID = 4326 }),
            ("6005", new Point(115.840, -31.970) { SRID = 4326 }),
            ("6006", new Point(115.760, -31.980) { SRID = 4326 }),
            ("6007", new Point(115.840, -31.940) { SRID = 4326 }),
            ("6007", new Point(115.840, -31.940) { SRID = 4326 }),
            ("6008", new Point(115.810, -31.950) { SRID = 4326 }),
            ("6008", new Point(115.810, -31.950) { SRID = 4326 }),
            ("6008", new Point(115.810, -31.950) { SRID = 4326 }),
            ("6008", new Point(115.810, -31.950) { SRID = 4326 }),
            ("6009", new Point(115.810, -31.980) { SRID = 4326 }),
            ("6009", new Point(115.810, -31.980) { SRID = 4326 }),
            ("6009", new Point(115.810, -31.980) { SRID = 4326 }),
            ("6009", new Point(115.810, -31.980) { SRID = 4326 }),
            ("6009", new Point(115.810, -31.980) { SRID = 4326 }),
            ("6010", new Point(115.780, -31.980) { SRID = 4326 }),
            ("6010", new Point(115.780, -31.980) { SRID = 4326 }),
            ("6010", new Point(115.780, -31.980) { SRID = 4326 }),
            ("6010", new Point(115.780, -31.980) { SRID = 4326 }),
            ("6010", new Point(115.780, -31.980) { SRID = 4326 }),
            ("6011", new Point(115.760, -32.000) { SRID = 4326 }),
            ("6011", new Point(115.760, -32.000) { SRID = 4326 }),
            ("6012", new Point(115.760, -32.010) { SRID = 4326 }),
            ("6014", new Point(115.790, -31.940) { SRID = 4326 }),
            ("6014", new Point(115.790, -31.940) { SRID = 4326 }),
            ("6014", new Point(115.790, -31.940) { SRID = 4326 }),
            ("6014", new Point(115.790, -31.940) { SRID = 4326 }),
            ("6015", new Point(115.750, -31.940) { SRID = 4326 }),
            ("6016", new Point(115.810, -31.920) { SRID = 4326 }),
            ("6016", new Point(115.810, -31.920) { SRID = 4326 }),
            ("6017", new Point(115.810, -31.910) { SRID = 4326 }),
            ("6017", new Point(115.810, -31.910) { SRID = 4326 }),
            ("6017", new Point(115.810, -31.910) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6018", new Point(115.790, -31.920) { SRID = 4326 }),
            ("6019", new Point(115.760, -31.890) { SRID = 4326 }),
            ("6019", new Point(115.760, -31.890) { SRID = 4326 }),
            ("6020", new Point(115.780, -31.850) { SRID = 4326 }),
            ("6020", new Point(115.780, -31.850) { SRID = 4326 }),
            ("6020", new Point(115.780, -31.850) { SRID = 4326 }),
            ("6020", new Point(115.780, -31.850) { SRID = 4326 }),
            ("6020", new Point(115.780, -31.850) { SRID = 4326 }),
            ("6021", new Point(115.820, -31.860) { SRID = 4326 }),
            ("6021", new Point(115.820, -31.860) { SRID = 4326 }),
            ("6022", new Point(115.800, -31.850) { SRID = 4326 }),
            ("6023", new Point(115.770, -31.830) { SRID = 4326 }),
            ("6024", new Point(115.800, -31.830) { SRID = 4326 }),
            ("6024", new Point(115.800, -31.830) { SRID = 4326 }),
            ("6025", new Point(115.770, -31.780) { SRID = 4326 }),
            ("6025", new Point(115.770, -31.780) { SRID = 4326 }),
            ("6025", new Point(115.770, -31.780) { SRID = 4326 }),
            ("6025", new Point(115.770, -31.780) { SRID = 4326 }),
            ("6026", new Point(115.790, -31.810) { SRID = 4326 }),
            ("6026", new Point(115.790, -31.810) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6027", new Point(115.760, -31.780) { SRID = 4326 }),
            ("6028", new Point(115.720, -31.730) { SRID = 4326 }),
            ("6028", new Point(115.720, -31.730) { SRID = 4326 }),
            ("6028", new Point(115.720, -31.730) { SRID = 4326 }),
            ("6028", new Point(115.720, -31.730) { SRID = 4326 }),
            ("6029", new Point(115.750, -31.880) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6030", new Point(115.720, -31.680) { SRID = 4326 }),
            ("6031", new Point(115.810, -31.700) { SRID = 4326 }),
            ("6031", new Point(115.810, -31.700) { SRID = 4326 }),
            ("6031", new Point(115.810, -31.700) { SRID = 4326 }),
            ("6032", new Point(115.740, -31.640) { SRID = 4326 }),
            ("6033", new Point(115.750, -31.600) { SRID = 4326 }),
            ("6034", new Point(115.810, -31.850) { SRID = 4326 }),
            ("6035", new Point(115.630, -31.550) { SRID = 4326 }),
            ("6036", new Point(115.710, -31.650) { SRID = 4326 }),
            ("6036", new Point(115.710, -31.650) { SRID = 4326 }),
            ("6037", new Point(115.600, -31.510) { SRID = 4326 }),
            ("6038", new Point(115.720, -32.290) { SRID = 4326 }),
            ("6041", new Point(115.530, -31.350) { SRID = 4326 }),
            ("6041", new Point(115.530, -31.350) { SRID = 4326 }),
            ("6041", new Point(115.530, -31.350) { SRID = 4326 }),
            ("6041", new Point(115.530, -31.350) { SRID = 4326 }),
            ("6041", new Point(115.530, -31.350) { SRID = 4326 }),
            ("6042", new Point(115.450, -31.250) { SRID = 4326 }),
            ("6043", new Point(115.420, -31.200) { SRID = 4326 }),
            ("6043", new Point(115.420, -31.200) { SRID = 4326 }),
            ("6044", new Point(115.420, -31.050) { SRID = 4326 }),
            ("6044", new Point(115.420, -31.050) { SRID = 4326 }),
            ("6044", new Point(115.420, -31.050) { SRID = 4326 }),
            ("6044", new Point(115.420, -31.050) { SRID = 4326 }),
            ("6050", new Point(115.860, -31.910) { SRID = 4326 }),
            ("6050", new Point(115.860, -31.910) { SRID = 4326 }),
            ("6050", new Point(115.860, -31.910) { SRID = 4326 }),
            ("6051", new Point(115.890, -31.930) { SRID = 4326 }),
            ("6052", new Point(115.890, -31.910) { SRID = 4326 }),
            ("6052", new Point(115.890, -31.910) { SRID = 4326 }),
            ("6053", new Point(115.910, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6054", new Point(115.940, -31.920) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6055", new Point(115.980, -31.870) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6056", new Point(116.030, -31.800) { SRID = 4326 }),
            ("6057", new Point(116.010, -31.940) { SRID = 4326 }),
            ("6057", new Point(116.010, -31.940) { SRID = 4326 }),
            ("6058", new Point(116.010, -31.990) { SRID = 4326 }),
            ("6059", new Point(115.860, -31.880) { SRID = 4326 }),
            ("6060", new Point(115.850, -31.910) { SRID = 4326 }),
            ("6060", new Point(115.850, -31.910) { SRID = 4326 }),
            ("6060", new Point(115.850, -31.910) { SRID = 4326 }),
            ("6060", new Point(115.850, -31.910) { SRID = 4326 }),
            ("6061", new Point(115.840, -31.860) { SRID = 4326 }),
            ("6061", new Point(115.840, -31.860) { SRID = 4326 }),
            ("6061", new Point(115.840, -31.860) { SRID = 4326 }),
            ("6061", new Point(115.840, -31.860) { SRID = 4326 }),
            ("6062", new Point(115.920, -31.900) { SRID = 4326 }),
            ("6062", new Point(115.920, -31.900) { SRID = 4326 }),
            ("6062", new Point(115.920, -31.900) { SRID = 4326 }),
            ("6063", new Point(115.920, -31.870) { SRID = 4326 }),
            ("6064", new Point(115.870, -31.830) { SRID = 4326 }),
            ("6064", new Point(115.870, -31.830) { SRID = 4326 }),
            ("6064", new Point(115.870, -31.830) { SRID = 4326 }),
            ("6064", new Point(115.870, -31.830) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6065", new Point(115.790, -31.730) { SRID = 4326 }),
            ("6066", new Point(115.890, -31.840) { SRID = 4326 }),
            ("6067", new Point(115.910, -31.820) { SRID = 4326 }),
            ("6068", new Point(115.940, -31.830) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6069", new Point(116.010, -31.770) { SRID = 4326 }),
            ("6070", new Point(116.080, -31.920) { SRID = 4326 }),
            ("6071", new Point(116.100, -31.910) { SRID = 4326 }),
            ("6071", new Point(116.100, -31.910) { SRID = 4326 }),
            ("6072", new Point(116.180, -31.870) { SRID = 4326 }),
            ("6073", new Point(116.170, -31.900) { SRID = 4326 }),
            ("6073", new Point(116.170, -31.900) { SRID = 4326 }),
            ("6074", new Point(116.200, -31.900) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6076", new Point(116.090, -32.010) { SRID = 4326 }),
            ("6081", new Point(116.150, -31.880) { SRID = 4326 }),
            ("6081", new Point(116.150, -31.880) { SRID = 4326 }),
            ("6082", new Point(116.300, -31.740) { SRID = 4326 }),
            ("6082", new Point(116.300, -31.740) { SRID = 4326 }),
            ("6083", new Point(116.200, -31.790) { SRID = 4326 }),
            ("6083", new Point(116.200, -31.790) { SRID = 4326 }),
            ("6084", new Point(116.210, -31.620) { SRID = 4326 }),
            ("6084", new Point(116.210, -31.620) { SRID = 4326 }),
            ("6084", new Point(116.210, -31.620) { SRID = 4326 }),
            ("6084", new Point(116.210, -31.620) { SRID = 4326 }),
            ("6084", new Point(116.210, -31.620) { SRID = 4326 }),
            ("6090", new Point(115.890, -31.860) { SRID = 4326 }),
            ("6100", new Point(115.900, -31.960) { SRID = 4326 }),
            ("6100", new Point(115.900, -31.960) { SRID = 4326 }),
            ("6100", new Point(115.900, -31.960) { SRID = 4326 }),
            ("6101", new Point(115.910, -31.980) { SRID = 4326 }),
            ("6101", new Point(115.910, -31.980) { SRID = 4326 }),
            ("6101", new Point(115.910, -31.980) { SRID = 4326 }),
            ("6101", new Point(115.910, -31.980) { SRID = 4326 }),
            ("6102", new Point(115.920, -32.000) { SRID = 4326 }),
            ("6102", new Point(115.920, -32.000) { SRID = 4326 }),
            ("6102", new Point(115.920, -32.000) { SRID = 4326 }),
            ("6102", new Point(115.920, -32.000) { SRID = 4326 }),
            ("6103", new Point(115.910, -31.960) { SRID = 4326 }),
            ("6104", new Point(115.920, -31.940) { SRID = 4326 }),
            ("6104", new Point(115.920, -31.940) { SRID = 4326 }),
            ("6104", new Point(115.920, -31.940) { SRID = 4326 }),
            ("6105", new Point(115.930, -31.960) { SRID = 4326 }),
            ("6105", new Point(115.930, -31.960) { SRID = 4326 }),
            ("6105", new Point(115.930, -31.960) { SRID = 4326 }),
            ("6106", new Point(115.950, -31.990) { SRID = 4326 }),
            ("6106", new Point(115.950, -31.990) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6107", new Point(115.960, -32.010) { SRID = 4326 }),
            ("6108", new Point(115.960, -32.050) { SRID = 4326 })
        };
        var merchantNames = new[]
        {
            "Coles", "KMart", "Big-W", "Sneakies", "Shoes", "Fishing", "Gym", "Newsagents", "KFC", "McDonalds",
            "Starbucks",
            "Imp", "Golden", "Duck"
        };
        var categories = new[]
        {
            "Restaurant", "Exercise", "Food", "Car", "Entertainment"
        };
        var seed = new Random(34645);
        for (int i = 0; i < 300; i++)
        {
            var merchantFirst = merchantNames[seed.Next(0, merchantNames.Length)];
            var merchantSecond = merchantNames[seed.Next(0, merchantNames.Length)];
            var category = categories[seed.Next(0, categories.Length)];
            var location = locations[seed.Next(0, locations.Length)];
            var sortCode = seed.Next(100000, 999999).ToString();
            var accountNumber = seed.Next(10000000, 99999999).ToString();
            yield return
            [
                RT.Comb.Provider.Sql.Create(), $"{merchantFirst} {merchantSecond}", category, sortCode, accountNumber,
                location.Item1, location.Item2
            ];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} merchants");
        }
    }

    private static IEnumerable<object[]> SeededAccountData(IList<Guid> customerIds)
    {
        var accountNames = new[]
        {
            "transactions", "rainy", "week", "year", "live-in-the-now", "best", "savings", "funky", "bonzer"
        };
        var accountNumber = 1000000;
        var seed = new Random(23423423);
        for (int i = 0; i < 2000; i++)
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

    private static IEnumerable<object[]> SeededTransactionData(IList<(Guid, string)> accountNumbers,
        List<(string, string)> merchants)
    {
        var transactionDescriptors = new[]
        {
            "Ice", "Cream", "Rent", "Shopping", "Mortgage", "Pizza", "Takeaway", "Havianas", "Shoes", "Socks",
            "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon",
            "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity",
            "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music",
            "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet",
            "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks",
            "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon",
            "Books", "Games", "Movies", "Music", "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity",
            "Gas", "Water", "Phone", "Internet", "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music",
            "Clothes", "Shoes", "Socks", "Underwear", "Bills", "Electricity", "Gas", "Water", "Phone", "Internet",
            "Netflix", "Spotify", "Amazon", "Books", "Games", "Movies", "Music"
        };
        var seed = new Random(645234);
        var pending = new Dictionary<Guid, (Guid, Guid?, string?, string?, string, int, DateTimeOffset, bool)>();
        for (int i = 0; i < 10000; i++)
        {
            var id = RT.Comb.Provider.Sql.Create();
            var cancelPending = seed.Next(0, 2) == 0;
            var pendingAmount = seed.Next(1, 99999999);
            var finaliseAmount = 0;
            var isCredit = seed.Next(0, 2) == 0;
            var reference = transactionDescriptors[seed.Next(0, transactionDescriptors.Length)] + (isCredit ? " refund" : string.Empty);

            var transactionType = seed.Next(0, 4);
            Guid? toAccountId = default;
            string? toSortCode = default;
            string? toAccountNumber = default;

            if (transactionType == 0)
            {
                toAccountId = accountNumbers[seed.Next(0, accountNumbers.Count)].Item1;
            }
            else if (transactionType == 1)
            {
                var merchant = seed.Next(merchants.Count);
                toSortCode = merchants[merchant].Item1;
                toAccountNumber = merchants[merchant].Item2;
            }
            else if (transactionType == 2)
            {
                toSortCode = seed.Next(100000, 999999).ToString();
                toAccountNumber = seed.Next(1000000, 9999999).ToString();
            }
            else if (transactionType == 3)
            {
                cancelPending = false;
                finaliseAmount = pendingAmount;
                pendingAmount = 0;
                reference = "Cash";
            }

            var from = accountNumbers[seed.Next(0, accountNumbers.Count)].Item1;
            var date = DateTimeOffset.UtcNow.AddDays(-seed.Next(0, 365));
            Guid? refersTo = null;

            if (cancelPending && pending.Count > 0)
            {
                refersTo = pending.Keys.ToList()[seed.Next(0, pending.Keys.Count)];
                (from, toAccountId, toSortCode, toAccountNumber, reference, finaliseAmount, date, isCredit) = pending[refersTo.Value];
                pending.Remove(refersTo.Value);
                pendingAmount = 0;
                date = date.AddMinutes(seed.Next(0, 5000));
            }
            else
            {
                if (pendingAmount > 0)
                {
                    pending.Add(id, (from, toAccountId, toSortCode, toAccountNumber, reference, pendingAmount, date, isCredit));
                }
            }

            yield return
            [
                id, toAccountId, toSortCode, toAccountNumber, from, pendingAmount, finaliseAmount, date, refersTo, reference, isCredit
            ];
            if (i % 50 == 0) Console.WriteLine($"Seeded {i} transactions");
        }
    }
}