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
}