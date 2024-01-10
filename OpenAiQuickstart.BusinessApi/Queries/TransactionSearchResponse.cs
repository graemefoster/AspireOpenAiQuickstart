using OpenAiQuickstart.BusinessDomain.Views;

namespace OpenAiQuickstart.BusinessApi.Queries;

public record TransactionSearchResponse(AccountTransaction[] Transactions);