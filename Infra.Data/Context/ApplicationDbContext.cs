using Domain.Entities.Account;
using Domain.Entities.ContactUs;
using Domain.Entities.Images;
using Domain.Entities.Product;
using Domain.Entities.Ticket;
using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Dbsets

    public DbSet<User> Users { get; set; }

    #endregion

    #region Product

    public DbSet<ProductCategory> ProductCategories { get; set; }

    #endregion

    //For Conflict relation
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());
    }

    #region Ticket

    public DbSet<TicketsMessage> TicketsMessages { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    #endregion

    #region ContactUs

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    #endregion

    #region Images

    public DbSet<Banner> Banners { get; set; }

    public DbSet<BannerFix> BannerFix { get; set; }

    #endregion
}