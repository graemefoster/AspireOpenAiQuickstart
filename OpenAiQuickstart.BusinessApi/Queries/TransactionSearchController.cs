using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using OpenAiQuickstart.BusinessDomain;

namespace OpenAiQuickstart.BusinessApi.Queries;

[ApiController]
[Route("/account/{accountNumber}/")]
public class TransactionSearchController : ControllerBase
{
    private readonly BankingContext _context;

    public TransactionSearchController(BankingContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Searches for transactions in a customer's account.
    /// </summary>
    /// <param name="accountNumber">The account number to search within</param>
    /// <param name="request">A set of parameters to refine the search</param>
    /// <returns>A TransactionSearchResponse object containing the search results</returns>
    [Route("search")]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TransactionSearchResponse), StatusCodes.Status200OK)]
    public TransactionSearchResponse Search([FromRoute] Guid accountNumber, TransactionSearchRequest request)
    {
        var transactions = _context
            .AccountTransactions
            .AsQueryable()
            .Where(x => x.From == accountNumber);

        if (request.From != default)
        {
            transactions = transactions.Where(x => x.Date >= request.From);
        }

        if (request.To != default)
        {
            transactions = transactions.Where(x => x.Date <= request.To);
        }

        if (!string.IsNullOrWhiteSpace(request.Reference))
        {
            transactions = transactions.Where(x => x.Reference.Contains(request.Reference));
        }

        if (request.Categories?.Any() ?? false)
        {
            var categories = request.Categories.Select(x => x.ToString()).ToArray();
            transactions = transactions.Where(x => categories.Contains(x.MerchantCategory));
        }

        if (request.LowAmountInCents.HasValue)
        {
            transactions = transactions.Where(x => x.Amount >= request.LowAmountInCents);
        }

        if (request.HighAmountInCents.HasValue)
        {
            transactions = transactions.Where(x => x.Amount <= request.HighAmountInCents);
        }

        if (request.BoundingBox != null)
        {
            //Create a linear ring from the latitude, longitude and distance in meters
            const double oneDegreeLatitudeRoughlyInMetres = 110574;
            
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326).CreatePoint(
                new Coordinate(request.BoundingBox.Value.CentreLatitude, request.BoundingBox.Value.CentreLongitude))
                .Buffer(request.BoundingBox.Value.BufferInMetres / oneDegreeLatitudeRoughlyInMetres);

            if (geometryFactory != null)
            {
                //.Revers() - https://github.com/NetTopologySuite/NetTopologySuite/issues/387
                transactions = transactions
                    .Where(x => x.MerchantLocation != null)
                    .Where(x => geometryFactory.Reverse().Contains(x.MerchantLocation));
            }
        }

        return new TransactionSearchResponse(transactions.AsNoTracking().ToArray());
    }
}