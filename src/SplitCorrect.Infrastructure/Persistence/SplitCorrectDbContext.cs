using Microsoft.EntityFrameworkCore;
using SplitCorrect.Domain.Common;
using SplitCorrect.Domain.Entities;

namespace SplitCorrect.Infrastructure.Persistence;

public class SplitCorrectDbContext : DbContext
{
    public SplitCorrectDbContext(DbContextOptions<SplitCorrectDbContext> options)
        : base(options)
    {
    }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseSplit> ExpenseSplits => Set<ExpenseSplit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Group configuration
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(200);
            entity.Property(g => g.Currency).IsRequired().HasMaxLength(3);
            entity.Property(g => g.Description).HasMaxLength(1000);

            entity.HasMany(g => g.Members)
                .WithMany()
                .UsingEntity(j => j.ToTable("GroupMembers"));

            entity.HasMany(g => g.Expenses)
                .WithOne()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Member configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(m => m.Email);
        });

        // Expense configuration
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.GroupId).IsRequired();
            entity.Property(e => e.PaidById).IsRequired();

            entity.OwnsOne(e => e.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 2).IsRequired();
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            entity.HasOne(e => e.PaidBy)
                .WithMany()
                .HasForeignKey(e => e.PaidById)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Splits)
                .WithOne()
                .HasForeignKey(s => s.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ExpenseSplit configuration
        modelBuilder.Entity<ExpenseSplit>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.ExpenseId).IsRequired();
            entity.Property(s => s.MemberId).IsRequired();

            entity.Property<decimal>("SplitAmount").HasPrecision(18, 2).IsRequired();
            entity.Property<string>("SplitCurrency").HasMaxLength(3).IsRequired();

            entity.Ignore(s => s.Amount);

            entity.HasOne(s => s.Member)
                .WithMany()
                .HasForeignKey(s => s.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
