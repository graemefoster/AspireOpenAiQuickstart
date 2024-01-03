using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OpenAiQuickstart.BusinessDomain;

namespace OpenAi.Quickstart.BusinessApi.DbBuilder;

public class BankingContextFactory : IDesignTimeDbContextFactory<BankingContext>
{
    public BankingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BankingContext>();
        optionsBuilder.UseSqlServer(
            "Server=.\\SQLEXPRESS;Database=Banking;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true",
            sqlOptions => sqlOptions.MigrationsAssembly(typeof(BankingContextFactory).Assembly.FullName));

        return new BankingContext(optionsBuilder.Options);
    }
}

