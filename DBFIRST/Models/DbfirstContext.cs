using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBFIRST.Models;

public partial class DbfirstContext : DbContext
{
    public DbfirstContext()
    {
    }

    public DbfirstContext(DbContextOptions<DbfirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBFIRST");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankAcco__3213E83FDD16696B");

            entity.HasIndex(e => e.AccountNumber, "UQ__BankAcco__17D0878A10A27D94").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountBalance)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("accountBalance");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(12)
                .HasColumnName("accountNumber");
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .HasColumnName("clientName");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("PK__Transact__17D0878B36F64E30");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(12)
                .HasColumnName("accountNumber");
            entity.Property(e => e.Deposit).HasColumnName("deposit");
            entity.Property(e => e.Withdraw).HasColumnName("withdraw");

            entity.HasOne(d => d.AccountNumberNavigation).WithOne(p => p.Transaction)
                .HasPrincipalKey<BankAccount>(p => p.AccountNumber)
                .HasForeignKey<Transaction>(d => d.AccountNumber)
                .HasConstraintName("FK__Transacti__accou__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
