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
    public DbSet<Banner> Banners { get; set; }

    public DbSet<BannerFix> BannerFix { get; set; }
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

        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "پیشنهاد" },
            new Subject { Id = 2, Name = "شکایت" },
            new Subject { Id = 3, Name = "سفارش" },
            new Subject { Id = 4, Name = "فروش" },
            new Subject { Id = 5, Name = "گارانتی" },
            new Subject { Id = 6, Name = "مدیریت" },
            new Subject { Id = 7, Name = "مالی" },
            new Subject { Id = 8, Name = "موضوعات" }

        );
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



    #endregion
}