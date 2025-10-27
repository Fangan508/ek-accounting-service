using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Represents the database context for managing accountings and related entities.
/// </summary>
public class AccountingDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the BankBooks table.
    /// </summary>
    public virtual DbSet<BankBook> BankBooks { get; set; }

    /// <summary>
    /// Gets or sets the BankBookPosition table.
    /// </summary>
    public virtual DbSet<BankBookPosition> BankBookPositions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountingDbContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }
}