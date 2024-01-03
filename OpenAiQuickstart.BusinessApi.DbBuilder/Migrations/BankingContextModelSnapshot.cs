﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenAiQuickstart.BusinessDomain;

#nullable disable

namespace OpenAi.Quickstart.BusinessApi.DbBuilder.Migrations
{
    [DbContext(typeof(BankingContext))]
    partial class BankingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Domain.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Domain.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Domain.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("From")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("RelatedTo")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.ComplexProperty<Dictionary<string, object>>("FinalisedAmountInCents", "OpenAiQuickstart.BusinessDomain.Domain.Transaction.FinalisedAmountInCents#Money", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<long>("Cents")
                                .HasColumnType("bigint");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PendingAmountInCents", "OpenAiQuickstart.BusinessDomain.Domain.Transaction.PendingAmountInCents#Money", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<long>("Cents")
                                .HasColumnType("bigint");
                        });

                    b.HasKey("Id");

                    b.HasIndex("From");

                    b.HasIndex("RelatedTo");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Views.AccountTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("From")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPending")
                        .HasColumnType("bit");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("To")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.ToView("AccountTransactions", (string)null);
                });

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Domain.Account", b =>
                {
                    b.HasOne("OpenAiQuickstart.BusinessDomain.Domain.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("OpenAiQuickstart.BusinessDomain.Domain.Transaction", b =>
                {
                    b.HasOne("OpenAiQuickstart.BusinessDomain.Domain.Account", null)
                        .WithMany()
                        .HasForeignKey("From")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OpenAiQuickstart.BusinessDomain.Domain.Transaction", null)
                        .WithMany()
                        .HasForeignKey("RelatedTo");
                });
#pragma warning restore 612, 618
        }
    }
}
