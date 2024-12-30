using Domain.Entities.Account;
using Domain.Entities.Comment;
using Domain.Entities.ContactUs;
using Domain.Entities.Images;
using Domain.Entities.Faq;
using Domain.Entities.Notification;
using Domain.Entities.Product;
using Domain.Entities.Ticket;
using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discount;
using Domain.Entities.Question;

namespace Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

    #region Dbsets

    public DbSet<User> Users { get; set; }
    public DbSet<Banner> Banners { get; set; }

    public DbSet<BannerFix> BannerFix { get; set; }
    #endregion

    #region Product

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Product { get; set; }

    public DbSet<Color> Colors { get; set; }

    public DbSet<ProductColor> ProductColors { get; set; }

    public DbSet<ProductSpecification> ProductSpecifications { get; set; }


    #endregion

    #region ForConflictrelation

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());
        #region Subject
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
        #endregion

        #region Category
        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { Id = 1, Title = "مد و پوشاک", ParentId = null },
            new ProductCategory { Id = 2, Title = "لباس مردانه", ParentId = 1 },
            new ProductCategory { Id = 3, Title = "لباس زنانه", ParentId = 1 },
            new ProductCategory { Id = 4, Title = "لباس بچگانه", ParentId = 1 },
            new ProductCategory { Id = 5, Title = "کالای دیجیتال", ParentId = null },
            new ProductCategory { Id = 6, Title = "موبایل و تبلت", ParentId = 5 },
            new ProductCategory { Id = 7, Title = "لپ‌تاپ", ParentId = 5 },
            new ProductCategory { Id = 8, Title = "لوازم جانبی", ParentId = 5 },
            new ProductCategory { Id = 9, Title = "خانه و آشپزخانه", ParentId = null },
            new ProductCategory { Id = 10, Title = "لوازم آشپزخانه", ParentId = 9 },
            new ProductCategory { Id = 11, Title = "دکوراسیون", ParentId = 9 },
            new ProductCategory { Id = 12, Title = "ابزار و وسایل", ParentId = 9 },
            new ProductCategory { Id = 13, Title = "زیبایی و سلامت", ParentId = null },
            new ProductCategory { Id = 14, Title = "محصولات مراقبتی", ParentId = 13 },
            new ProductCategory { Id = 15, Title = "عطر و ادکلن", ParentId = 13 },
            new ProductCategory { Id = 16, Title = "لوازم آرایشی", ParentId = 13 },
            new ProductCategory { Id = 17, Title = "ورزش و سفر", ParentId = null },
            new ProductCategory { Id = 18, Title = "تجهیزات ورزشی", ParentId = 17 },
            new ProductCategory { Id = 19, Title = "کیف و کوله", ParentId = 17 },
            new ProductCategory { Id = 20, Title = "لوازم کمپینگ", ParentId = 17 });
        #endregion
    }
    #endregion 


    #region Ticket

    public DbSet<TicketsMessage> TicketsMessages { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    #endregion

    #region ContactUs

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    #endregion

    #region Images


    public DbSet<Product> Products { get; set; }

    public DbSet<ProductGallery> ProductGalleries { get; set; }


    #endregion

    #region faq

    public DbSet<FaqCategory> FaqCategories { get; set; }

    public DbSet<FaqQuestion> FaqQuestions { get; set; }

    #endregion

    #region Notification

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<UserNotification> UserNotifications { get; set; }

    #endregion

    #region Discount
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<UserDiscount> UserDiscounts { get; set; }
    public DbSet<ProductDiscount> ProductDiscounts { get; set; }

    #endregion

    #region QuestionAnswer

    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public DbSet<QuestionLiked> QuestionLikes { get; set; }

    #endregion

    #region Comment

    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentRating> CommentRatings { get; set; }

    public DbSet<UserInteraction> UserInteraction { get; set; }

    #endregion
}