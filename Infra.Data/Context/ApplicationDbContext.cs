using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Dbsets

    public DbSet<Users> Users { get; set; }

    #endregion
}