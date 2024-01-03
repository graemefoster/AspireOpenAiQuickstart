using Microsoft.EntityFrameworkCore;
using OpenAiQuickstart.BusinessDomain.Domain;
using OpenAiQuickstart.BusinessDomain.Views;

namespace OpenAiQuickstart.BusinessDomain;

public class BankingContext : DbContext
{
    public BankingContext(DbContextOptions<BankingContext> optionsBuilderOptions) : base(optionsBuilderOptions)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
    
    public DbSet<AccountTransaction> AccountTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountTransaction>(b =>
        {
            b.ToView("AccountTransactions");
        });
        
        modelBuilder.Entity<Account>(b =>
            b.HasOne<Customer>().WithMany().IsRequired().HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.NoAction));

        modelBuilder.Entity<Transaction>(b =>
        {
            b.ComplexProperty(x => x.FinalisedAmountInCents);
            b.ComplexProperty(x => x.PendingAmountInCents);
            b.Property(x => x.To).IsRequired();
            b.Property(x => x.From).IsRequired();
            b.HasOne<Account>().WithMany().IsRequired().HasForeignKey(c => c.From).OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Transaction>().WithMany().HasForeignKey(c => c.RelatedTo);
        });
    }
}