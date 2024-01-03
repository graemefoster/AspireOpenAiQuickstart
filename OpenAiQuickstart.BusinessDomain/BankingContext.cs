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

    public DbSet<Merchant> Merchants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountTransaction>(b => { b.ToView("AccountTransactions"); });

        modelBuilder.Entity<Merchant>(b =>
        {
            b.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        });

        modelBuilder.Entity<Customer>(b => { b.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()"); });

        modelBuilder.Entity<Account>(b =>
        {
            b.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            b.HasOne<Customer>().WithMany().IsRequired().HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Transaction>(b =>
        {
            b.Property(x => x.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            b.ComplexProperty(x => x.FinalisedAmountInCents);
            b.ComplexProperty(x => x.PendingAmountInCents);
            b.Property(x => x.From).IsRequired();
            b.HasOne<Account>().WithMany().HasForeignKey(c => c.ToId)
                .OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Account>().WithMany().IsRequired().HasForeignKey(c => c.From)
                .OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Transaction>().WithMany().HasForeignKey(c => c.RelatedTo);
        });
    }
}