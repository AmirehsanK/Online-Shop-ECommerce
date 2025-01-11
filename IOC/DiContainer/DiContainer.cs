using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Security;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.Interface;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.DiContainer;

public static class DiContainer
{
    public static void IOcContainer(this IServiceCollection services)
    {
        #region Repositories

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IContactUsRepository, ContactUsRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IFileHandleRepository, FileHandleRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IProductSpecificationRepository, ProductSpecificationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IFaqRepository, FaqRepository>();
        services.AddScoped<IProductGalleryRepository, ProductGalleryRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IProductColorRepository, ProductColorRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();

        #endregion

        #region Services

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IContactUsService, ContactUsService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IFileHandleService, FileHandleService>();
        services.AddSingleton<IAuthorizationHandler, AdminHandler>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFaqService, FaqService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IProductGalleryService, ProductGalleryService>();
        services.AddScoped<IProductColorService, ProductColorService>();
        services.AddScoped<IProductSpecificationService, ProductSpecificationService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IFavoritesService, FavoritesService>();

        #endregion

        services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
    }
}