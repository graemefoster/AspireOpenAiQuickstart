namespace OpenAiQuickstart.Web;

public class BankingClient(HttpClient httpClient)
{
    public Task<Account> GetAccount()
    {
        return httpClient.GetFromJsonAsync<Account>("/account")!;
    }

    public Task<Transaction[]?> GetTransactions()
    {
        return httpClient.GetFromJsonAsync<Transaction[]>("/account/transactions");
    }
}

